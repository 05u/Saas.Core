using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 远程指令服务
    /// </summary>
    public class BusRemoteCommandService : Repository<BusRemoteCommand>
    {
        private readonly IMapper _mapper;
        private readonly BusNoticeMessageService _noticeMessageService;
        private readonly ILogger<BusRemoteCommandService> _logger;
        private IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        /// <summary>
        /// 
        /// </summary>
        public BusRemoteCommandService(MainDbContext context,
            IMapper mapper,
            BusNoticeMessageService noticeMessageService,
            ILogger<BusRemoteCommandService> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ICurrentUserService currentUserService,
            EmailService emailService
            ) : base(context, currentUserService)
        {
            _mapper = mapper;
            _noticeMessageService = noticeMessageService;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _emailService = emailService;
        }

        /// <summary>
        /// 客户端心跳更新
        /// </summary>
        /// <returns></returns>
        public async Task<BusRemoteCommand> ClientHeartbeat(string clientName)
        {
            var dbRemoteCommand = await Queryable().AsNoTracking().Where(c => c.ClientName == clientName).FirstOrDefaultAsync();
            var nowtime = DateTime.Now;
            if (dbRemoteCommand == null) //注册新客户端
            {
                var newRecord = new BusRemoteCommand
                {
                    ClientName = clientName,
                    CreateTime = nowtime,
                    LastHeartTime = nowtime,
                    HeartbeatCycle = 60,
                };
                await InsertAsync(newRecord);
                return newRecord;
            }
            else
            {

                var result = _mapper.Map<ClientHeartbeatOutput>(dbRemoteCommand);
                if (dbRemoteCommand.RebootAction == true)
                {
                    //if (dbRemoteCommand.IsOnline)
                    //{
                    //    //如果客户端在线则返回重启或关机指令
                    //    dbRemoteCommand.LastRebootTime = nowtime;
                    //}
                    //else
                    //{
                    //    //如果客户端不在线,则首次心跳不返回重启或关机指令
                    //    result.RebootAction = false;
                    //}
                    dbRemoteCommand.LastRebootTime = nowtime;
                    dbRemoteCommand.RebootAction = false;

                }
                if (dbRemoteCommand.ShutdownAction == true)
                {
                    //if (dbRemoteCommand.IsOnline)
                    //{
                    //    //如果客户端在线则返回重启或关机指令
                    //    dbRemoteCommand.LastShutdownTime = nowtime;
                    //}
                    //else
                    //{
                    //    //如果客户端不在线,则首次心跳不返回重启或关机指令
                    //    result.ShutdownAction = false;
                    //}
                    dbRemoteCommand.LastShutdownTime = nowtime;
                    dbRemoteCommand.ShutdownAction = false;

                }
                if (dbRemoteCommand.ActionType != ActionType.None)
                {
                    //客户端收到远程指令后,将后台记录重置,避免指令重复执行
                    dbRemoteCommand.ActionType = ActionType.None;
                }
                dbRemoteCommand.HeartbeatCycle = (int)(nowtime - (DateTime)dbRemoteCommand.LastHeartTime).TotalSeconds;//自动计算心跳周期
                dbRemoteCommand.LastHeartTime = nowtime;
                await UpdateAsync(dbRemoteCommand);
                return result;
            }


        }


        /// <summary>
        /// 服务监控预警
        /// </summary>
        /// <returns></returns>
        public async Task MonitoringServices()
        {
            var nowTime = DateTime.Now;
            if ((nowTime - SystemInfo.SystemStartTime).TotalMinutes > 2)//系统启动的前2分钟不执行监控
            {
                var list = await Queryable().Where(x => x.LastHeartTime != null).ToListAsync();
                var outageServices = list.Where(x => x.IsMonitoringAlarm == false)
                    .Where(x => (nowTime - (DateTime)x.LastHeartTime).TotalSeconds > (x.HeartbeatCycle * 2 >= 60 ? x.HeartbeatCycle * 2 : 60)).ToList();//连续两个心跳周期无心跳判定为离线,最小值限为60
                if (outageServices.Any())
                {
                    var text = $"检测到下列服务离线,请及时关注:{Environment.NewLine}";
                    outageServices.ForEach(x =>
                    {
                        _logger.LogWarning($"检测到{x.ClientName}服务离线,上次心跳时间:{x.LastHeartTime},差值(秒):{(nowTime - (DateTime)x.LastHeartTime).TotalSeconds}");

                        text += $"{x.ClientName}{Environment.NewLine}";
                        x.IsMonitoringAlarm = true;
                    });

                    //如果机器人服务出现异常,先通过邮件发送提醒,然后再尝试其他机器人通道发送
                    if (text.Contains("WeixinRobot") || text.Contains("QQRobot"))
                    {
                        try
                        {
                            List<MailboxAddress> ToMail = new List<MailboxAddress>();
                            ToMail.Add(new MailboxAddress("admin", _configuration.GetSection("AdminInfo:Email").Value));
                            await _emailService.SendAsync(ToMail, "机器人服务异常", new TextPart("html") { Text = text });
                        }
                        catch (Exception ex) { }
                    }
                    await _noticeMessageService.PublishNoticeMessageToGroup("双人组", text, false);
                    await BatchUpdateAsync(outageServices);

                }

                var backServices = list.Where(x => x.IsMonitoringAlarm == true)
                    .Where(x => (nowTime - (DateTime)x.LastHeartTime).TotalSeconds <= (x.HeartbeatCycle < 60 ? 60 : x.HeartbeatCycle)).ToList();

                if (backServices.Any())
                {
                    var text = $"检测到下列服务已恢复:{Environment.NewLine}";
                    backServices.ForEach(x =>
                    {
                        _logger.LogWarning($"检测到{x.ClientName}服务恢复,上次心跳时间{x.LastHeartTime},差值(秒):{(nowTime - (DateTime)x.LastHeartTime).TotalSeconds}");

                        text += $"{x.ClientName}{Environment.NewLine}";
                        x.IsMonitoringAlarm = false;
                    });

                    await _noticeMessageService.PublishNoticeMessageToGroup("双人组", text, false);
                    await BatchUpdateAsync(backServices);
                }
            }


        }

        /// <summary>
        /// 获取和更新QQ机器人心跳
        /// </summary>
        /// <returns></returns>
        public async Task GetQQRobotHeartbeat()
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            var response = await client.GetAsync($"http://{_configuration.GetSection("MiraiBot:Address").Value}/about");
            var miraiResponse = (await response.Content.ReadAsStringAsync()).FromJSON<MiraiAbout>();
            if (miraiResponse?.Code == 0)
            {
                await ClientHeartbeat("QQRobot");
            }
        }


        /// <summary>
        /// 发送远程指令
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> SendRemoteCommand(SendRemoteCommandInput input)
        {
            var dbRemoteCommand = await Queryable().Where(c => c.ClientName == input.ClientName).FirstOrDefaultAsync();
            if (dbRemoteCommand == null)
            {
                throw new BusinessException("电脑不存在,请确认客户端服务已运行并联网!");
            }
            if (!dbRemoteCommand.IsOnline)
            {
                throw new BusinessException("电脑当前不在线,无法发送控制指令!");
            }
            if (input.ActionType == ActionType.RebootAction)
            {
                dbRemoteCommand.RebootAction = true;
            }
            if (input.ActionType == ActionType.ShutdownAction)
            {
                dbRemoteCommand.ShutdownAction = true;
            }
            dbRemoteCommand.ActionType = input.ActionType;
            await UpdateAsync(dbRemoteCommand);
            return true;

        }
    }
}
