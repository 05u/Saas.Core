using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 小爱消息处理服务
    /// </summary>
    public class MdmXiaoaiService : Repository<MdmXiaoaiSpeaker>
    {
        private readonly ILogger<MdmXiaoaiService> _logger;
        private readonly BusPregnantWomanEatMedicineRecordService _pregnantWomanEatMedicineRecordService;
        private readonly BusWakeOnLanService _wakeOnLanService;
        private readonly IConfiguration _configuration;
        private readonly BusRemoteCommandService _remoteCommandService;

        /// <summary>
        /// ctor
        /// </summary>
        public MdmXiaoaiService(MainDbContext context,
            ILogger<MdmXiaoaiService> logger,
            BusPregnantWomanEatMedicineRecordService pregnantWomanEatMedicineRecordService,
            BusWakeOnLanService wakeOnLanService,
            IConfiguration configuration,
            BusRemoteCommandService remoteCommandService,
            ICurrentUserService currentUserService
            ) : base(context, currentUserService)
        {
            _logger = logger;
            _pregnantWomanEatMedicineRecordService = pregnantWomanEatMedicineRecordService;
            _wakeOnLanService = wakeOnLanService;
            _configuration = configuration;
            _remoteCommandService = remoteCommandService;
        }

        /// <summary>
        /// 通用小爱消息处理
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="name">小爱名称</param>
        /// <returns></returns>
        public async Task<bool> GeneralMessageProcess(string msg, string name)
        {
            _logger.LogInformation($"收到来自小爱的消息:{msg}");
            string replyMsg = null;
            if (msg.IsContainsAll(new string[] { "我", "已", "吃", "完" }) && name.IsRobotAdmin())
            {
                replyMsg = await _pregnantWomanEatMedicineRecordService.SubmitSuccess();
            }
            else if (msg.IsContainsAll(new string[] { "开", "主卧", "电脑" }) && name.IsRobotAdmin())
            {
                await _wakeOnLanService.WOL("80:fa:5b:53:87:68", "通过小爱唤醒");
                replyMsg = "已发送指令~";
            }
            else if (msg.IsContainsAll(new string[] { "开", "书房", "电脑" }) && name.IsRobotAdmin())
            {
                await _wakeOnLanService.WOL("8c:82:b9:51:14:21", "通过小爱唤醒");
                replyMsg = "已发送指令~";
            }
            else if (msg.IsContainsAll(new string[] { "开", "服务器" }) && name.IsRobotAdmin())
            {
                await _wakeOnLanService.WOL("8c:82:b9:51:04:03", "通过机器人服务唤醒");
                replyMsg = "已发送指令~";
            }
            else if (msg.IsContainsAll(new string[] { "开", "工控机" }) && name.IsRobotAdmin())
            {
                await _wakeOnLanService.WOL("00:e0:4c:68:d5:2d", "通过机器人服务唤醒");
                replyMsg = "已发送指令~";
            }
            else if (msg.IsContainsAll(new string[] { "开", "客厅", "电脑" }) && name.IsRobotAdmin())
            {
                await _wakeOnLanService.WOL("00:e0:4c:68:d5:2d", "通过机器人服务唤醒");
                replyMsg = "已发送指令~";
            }
            else if (msg.IsContainsAll(new string[] { "开", "电脑" }) && name.IsRobotAdmin() && name == "南京主卧")
            {
                await _wakeOnLanService.WOL("80:fa:5b:53:87:68", "通过小爱唤醒");
                replyMsg = "已发送指令~";
            }
            else if (msg.IsContainsAll(new string[] { "开", "电脑" }) && name.IsRobotAdmin() && name == "南京书房")
            {
                await _wakeOnLanService.WOL("8c:82:b9:51:14:21", "通过小爱唤醒");
                replyMsg = "已发送指令~";
            }
            else if (msg.IsContainsAll(new string[] { "开", "电脑" }) && name.IsRobotAdmin() && name == "南京客厅")
            {
                await _wakeOnLanService.WOL("00:e0:4c:68:d5:2d", "通过小爱唤醒");
                replyMsg = "已发送指令~";
            }
            else if (msg.IsContainsAll(new string[] { "关", "电脑" }))
            {
                try
                {
                    if (msg.Contains("主卧") && name.IsRobotAdmin())
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.ShutdownAction, ClientName = "XZM-Hasee" });
                        replyMsg = "电脑将在一分钟内关闭";
                    }
                    else if (msg.Contains("书房") && name.IsRobotAdmin())
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.ShutdownAction, ClientName = "XZM-X58" });
                        replyMsg = "电脑将在一分钟内关闭";
                    }
                    else if (name.IsRobotAdmin() && name == "南京主卧")
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.ShutdownAction, ClientName = "XZM-Hasee" });
                        replyMsg = "电脑将在一分钟内关闭";
                    }
                    else if (name.IsRobotAdmin() && name == "南京书房")
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.ShutdownAction, ClientName = "XZM-X58" });
                        replyMsg = "电脑将在一分钟内关闭";
                    }
                    else if (name.Contains("仪征"))
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.ShutdownAction, ClientName = "YZ-HOME" });
                        replyMsg = "电脑将在一分钟内关闭";
                    }
                }
                catch (Exception ex)
                {
                    replyMsg = ex.Message;
                }
            }
            else if (msg.IsContainsAll(new string[] { "重启", "电脑" }))
            {
                try
                {
                    if (msg.Contains("主卧") && name.IsRobotAdmin())
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.RebootAction, ClientName = "XZM-Hasee" });
                        replyMsg = "电脑将在一分钟内重启";
                    }
                    else if (msg.Contains("书房") && name.IsRobotAdmin())
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.RebootAction, ClientName = "XZM-X58" });
                        replyMsg = "电脑将在一分钟内重启";
                    }
                    else if (name.IsRobotAdmin() && name == "南京主卧")
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.RebootAction, ClientName = "XZM-Hasee" });
                        replyMsg = "电脑将在一分钟内重启";
                    }
                    else if (name.IsRobotAdmin() && name == "南京书房")
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.RebootAction, ClientName = "XZM-X58" });
                        replyMsg = "电脑将在一分钟内重启";
                    }
                    else if (name.Contains("仪征"))
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.RebootAction, ClientName = "YZ-HOME" });
                        replyMsg = "电脑将在一分钟内重启";
                    }
                }
                catch (Exception ex)
                {
                    replyMsg = ex.Message;
                }
            }

            if (replyMsg.IsNotBlank())
            {
                if (PublishXiaoaiMessage(replyMsg, name))
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// 发送小爱消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="name">小爱名称</param>
        /// <returns></returns>
        public bool PublishXiaoaiMessage(string msg, string name)
        {
            var sendMsg = new SendXiaoaiMsg
            {
                name = name,
                text = msg,
            };

            if (MqttHelper.PublishMqtt(JsonNewtonsoft.ToJSON(sendMsg), _configuration.GetSection("MqTopicConfig:XiaoaiTTSv2").Value))
            {
                _logger.LogInformation($"小爱消息发送成功:{msg}");
                return true;
            }
            else
            {
                _logger.LogError($"小爱消息发送失败:{msg}");
                return false;
            }
        }
    }
}
