using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;
using Websocket.Client;

namespace Saas.Core.Service.Background
{
    /// <summary>
    /// 微信机器人服务
    /// </summary>
    public class WeixinRobotService : BackgroundService
    {
        private readonly ILogger<WeixinRobotService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration Configuration;

        /// <summary>
        /// ctor
        /// </summary>
        public WeixinRobotService(ILogger<WeixinRobotService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            Configuration = configuration;
        }

        /// <summary>
        /// 启动微信机器人服务
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool isActive = Convert.ToBoolean(Configuration["WeixinBot:IsActive"]);
            if (isActive)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var robotService = scope.ServiceProvider.GetService<RobotService>();
                        var eatMedicineService = scope.ServiceProvider.GetService<BusPregnantWomanEatMedicineRecordService>();
                        var remoteCommandService = scope.ServiceProvider.GetService<BusRemoteCommandService>();
                        var redisStackExchangeService = scope.ServiceProvider.GetService<IRedisStackExchangeService>();

                        var exitEvent = new ManualResetEvent(false);
                        var url = new Uri(Configuration.GetSection("UrlConfig:WeixinRobotWs").Value);

                        using (var client = new WebsocketClient(url))
                        {
                            client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                            client.ReconnectionHappened.Subscribe(info =>
                                _logger.LogInformation($"WebsocketClient发生重连, type: {info.Type}"));

                            client.MessageReceived.Subscribe(async msg =>
                            {
                                try
                                {
                                    if (!msg.ToString().Contains("heart beat"))
                                    {
                                        _logger.LogInformation($"收到微信消息: {msg}");
                                    }
                                    else
                                    {
                                        await remoteCommandService.ClientHeartbeat("WeixinRobot");
                                    }

                                    //解析消息
                                    var receive = msg.ToString().FromJSON<ReceiveWeixinDto>();
                                    if (receive != null && receive.wxid.IsNotBlank())
                                    {
                                        var resultMsg = "";
                                        //处理个人消息
                                        if (receive.wxid.Contains("wxid_"))
                                        {
                                            _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:WeixinRobotReceive", TimeSpan.FromDays(365), 1);
                                            if (receive.content.ToLower().Contains("查询我的微信id"))
                                            {
                                                resultMsg = receive.wxid;
                                            }
                                            else
                                            {
                                                resultMsg = await robotService.GeneralMessageProcess(receive.content, receive.wxid);
                                            }
                                            //回复个人消息
                                            if (resultMsg.IsNotBlank())
                                            {
                                                var sendWeixinDto = new WeixinApiDto()
                                                {
                                                    para = new WeixinPara()
                                                    {
                                                        id = DateTime.Now.ToString("yyyyMMddHHmmss"),
                                                        type = 555,
                                                        roomid = "",
                                                        wxid = receive.wxid,
                                                        content = resultMsg,
                                                        nickname = "",
                                                        ext = "",
                                                    }
                                                };
                                                Random ra = new Random();
                                                Thread.Sleep(ra.Next(500, 1000));
                                                //随机延迟,避免风控
                                                await Task.Run(() => client.Send(sendWeixinDto.para.ToJSON()));
                                                _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:WeixinRobotSend", TimeSpan.FromDays(365), 1);
                                            }
                                        }
                                        //处理群里被@的消息
                                        if (receive.wxid.Contains("@chatroom") && receive.content.Contains("@小烧饼"))
                                        {
                                            _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:WeixinRobotReceive", TimeSpan.FromDays(365), 1);
                                            if (receive.content.ToLower().Contains("查询我的微信id"))
                                            {
                                                resultMsg = $"{receive.wxid},{receive.id1}";
                                            }
                                            else
                                            {
                                                resultMsg = await robotService.GeneralMessageProcess(receive.content.Replace("@小烧饼 ", string.Empty), receive.id1, receive.wxid);
                                            }
                                            //回复群里被@的消息
                                            if (resultMsg.IsNotBlank())
                                            {
                                                var sendWeixinDto = new WeixinApiDto()
                                                {
                                                    para = new WeixinPara()
                                                    {
                                                        id = DateTime.Now.ToString("yyyyMMddHHmmss"),
                                                        type = 550,
                                                        roomid = receive.wxid,
                                                        wxid = receive.id1,
                                                        content = resultMsg,
                                                        ext = "",
                                                    }
                                                };

                                                //查询缓存
                                                var contactList = await redisStackExchangeService.StringGetAsync<WeixinResult<List<ContactListContent>>>("ContactList");
                                                if (contactList == null)
                                                {
                                                    contactList = await robotService.GetContactList(new ContactListContent());
                                                    //设置缓存
                                                    await redisStackExchangeService.StringSetAsync<WeixinResult<List<ContactListContent>>>("ContactList", contactList, TimeSpan.FromDays(1));
                                                }
                                                sendWeixinDto.para.nickname = contactList?.content?.FirstOrDefault(c => c.wxid == sendWeixinDto.para.wxid)?.name ?? "";
                                                //如果不是机器人好友,且有群id,则通过下列接口获取昵称
                                                if (sendWeixinDto.para.nickname.IsBlank() && sendWeixinDto.para.roomid.IsNotBlank())
                                                {
                                                    sendWeixinDto.para.nickname = (await robotService.GetMemberNick(sendWeixinDto.para.wxid, sendWeixinDto.para.roomid))?.content?.nick ?? "";
                                                }

                                                Random ra = new Random();
                                                Thread.Sleep(ra.Next(500, 1000));
                                                //随机延迟,避免风控
                                                await Task.Run(() => client.Send(sendWeixinDto.para.ToJSON()));
                                                _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:WeixinRobotSend", TimeSpan.FromDays(365), 1);
                                            }
                                        }



                                    }


                                }
                                catch (Exception e)
                                {
                                    _logger.LogError($"发生异常:{e.Message}");
                                }
                            });
                            await client.Start();
                            _logger.LogInformation($"微信机器人启动...");
                            exitEvent.WaitOne();
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"发生异常:{e.Message}");
                }
            }
        }

    }
}
