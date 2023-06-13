using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using MimeKit;
using Mirai.Net.Data.Messages;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Org.BouncyCastle.Asn1.Ocsp;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;
using System.Net.Http.Json;
using System.Reactive.Linq;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 机器人公共服务
    /// </summary>
    public class RobotService
    {
        private readonly ILogger<RobotService> _logger;

        private IHttpClientFactory _httpClientFactory;

        private readonly BusPregnantWomanEatMedicineRecordService _pregnantWomanEatMedicineRecordService;
        private readonly BusWakeOnLanService _wakeOnLanService;
        private readonly IConfiguration _configuration;
        private readonly BusRemoteCommandService _remoteCommandService;
        private readonly BusBookSubscriptionService _bookSubscriptionService;
        private readonly BusPregnantWomanEventRecordService _pregnantWomanEventRecordService;
        private readonly BusJieParkService _jieParkService;
        private readonly MdmMessageGroupService _mdmMessageGroupService;
        private readonly MdmMessageReceiverService _mdmMessageReceiverService;
        private readonly MdmMessageGroupReceiverService _mdmMessageGroupReceiverService;
        private readonly MdmCameraService _cameraService;
        private readonly BusBlockKeywordService _blockKeywordService;
        private readonly IRedisStackExchangeService _redisStackExchangeService;
        private readonly BusLuckyDrawService _luckyDrawService;
        private readonly BusNoticeTaskService _noticeTaskService;
        private readonly BusChatGptContextService _busChatGptContextService;

        /// <summary>
        /// ctor
        /// </summary>
        public RobotService(ILogger<RobotService> logger,
            IHttpClientFactory httpClientFactory,
            BusPregnantWomanEatMedicineRecordService pregnantWomanEatMedicineRecordService,
            BusWakeOnLanService wakeOnLanService,
            IConfiguration configuration,
            BusRemoteCommandService remoteCommandService,
            BusBookSubscriptionService bookSubscriptionService,
            BusPregnantWomanEventRecordService pregnantWomanEventRecordService,
            BusJieParkService jieParkService,
            MdmMessageGroupService mdmMessageGroupService,
            MdmMessageReceiverService mdmMessageReceiverService,
            MdmMessageGroupReceiverService mdmMessageGroupReceiverService,
            MdmCameraService cameraService,
            BusBlockKeywordService blockKeywordService,
            IRedisStackExchangeService redisStackExchangeService,
            BusLuckyDrawService luckyDrawService,
            BusNoticeTaskService noticeTaskService,
            BusChatGptContextService busChatGptContextService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _pregnantWomanEatMedicineRecordService = pregnantWomanEatMedicineRecordService;
            _wakeOnLanService = wakeOnLanService;
            _configuration = configuration;
            _remoteCommandService = remoteCommandService;
            _bookSubscriptionService = bookSubscriptionService;
            _pregnantWomanEventRecordService = pregnantWomanEventRecordService;
            _jieParkService = jieParkService;
            _mdmMessageGroupService = mdmMessageGroupService;
            _mdmMessageReceiverService = mdmMessageReceiverService;
            _mdmMessageGroupReceiverService = mdmMessageGroupReceiverService;
            _cameraService = cameraService;
            _blockKeywordService = blockKeywordService;
            _redisStackExchangeService = redisStackExchangeService;
            _luckyDrawService = luckyDrawService;
            _noticeTaskService = noticeTaskService;
            _busChatGptContextService = busChatGptContextService;
        }

        /// <summary>
        /// 图灵AI聊天API
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetTulingAiChat(string msg, string userId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);

                var param = new TulingSendDto()
                {
                    perception = new Perception() { inputText = new InputText() { text = msg }, selfInfo = new SelfInfo() { location = new Location() } },
                    userInfo = new UserInfo() { userId = userId }
                };
                var json = param.ToJSON();
                HttpContent content = new StringContent(json);
                var response = await client.PostAsync(_configuration.GetSection("UrlConfig:TuringApi").Value, content);
                var result = (await response.Content.ReadAsStringAsync()).FromJSON<TulingReceiveDto>();
                _ = await _redisStackExchangeService.CacheCountCheck("CountAnalysis:Tuling", TimeSpan.FromDays(365), 1);
                return result?.results?.First()?.values?.text;
            }
            catch (Exception ex)
            {
                _logger.LogError("图灵接口请求过程出现问题:" + ex.Message);
                return "图灵接口异常,请稍后再试!";
            }

        }

        /// <summary>
        /// 获取微信机器人好友和群列表(实时)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WeixinResult<List<ContactListContent>>> GetContactList(ContactListContent input)
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            var sendWeixinDto = new WeixinApiDto()
            {
                para = new WeixinPara()
                {
                    id = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    type = 5000,
                }
            };
            HttpContent content = new StringContent(sendWeixinDto.ToJSON());
            var response = await client.PostAsync($"{_configuration.GetSection("UrlConfig:WeixinRobotHttp").Value}/api/getcontactlist", content);
            var httpResponse = (await response.Content.ReadAsStringAsync()).FromJSON<WeixinResult<List<ContactListContent>>>();
            var dataList = httpResponse.content
                .WhereIf(input.name.IsNotBlank() && input.name != "string", c => c.name.ToLower().Contains(input.name.ToLower()))
                .WhereIf(input.remarks.IsNotBlank() && input.remarks != "string", c => c.remarks.Contains(input.remarks))
                .WhereIf(input.wxcode.IsNotBlank() && input.wxcode != "string", c => c.wxcode.Contains(input.wxcode))
                .WhereIf(input.wxid.IsNotBlank() && input.wxid != "string", c => c.wxid.Contains(input.wxid))
                .ToList();
            httpResponse.content = dataList;
            return httpResponse;
        }

        /// <summary>
        /// 获取微信群成员昵称
        /// </summary>
        /// <param name="wx_id">群成员id</param>
        /// <param name="roomid">群id</param>
        /// <returns></returns>
        public async Task<WeixinResult<MemberNick>> GetMemberNick(string wx_id, string roomid)
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            var sendWeixinDto = new WeixinApiDto()
            {
                para = new WeixinPara()
                {
                    id = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    type = 5020,
                    wxid = wx_id,
                    roomid = roomid,
                }
            };
            HttpContent content = new StringContent(sendWeixinDto.ToJSON());
            var response = await client.PostAsync($"{_configuration.GetSection("UrlConfig:WeixinRobotHttp").Value}/api/getmembernick", content);
            var httpResponse = (await response.Content.ReadAsStringAsync()).FromJSON<WeixinResult<string>>();

            return new WeixinResult<MemberNick>()
            {
                id = httpResponse.id,
                type = httpResponse.type,
                receiver = httpResponse.receiver,
                sender = httpResponse.sender,
                srvid = httpResponse.srvid,
                status = httpResponse.status,
                time = httpResponse.time,
                content = httpResponse.content.FromJSON<MemberNick>()
            };
        }


        /// <summary>
        /// 通用机器人消息处理
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<string> GeneralMessageProcess(string msg, string userId, string groupId = null)
        {
            _logger.LogInformation($"收到机器人消息:{msg},发送人:{userId}");

            #region 有条件逻辑

            if (msg.IsContainsAll(new string[] { "我", "已", "吃", "完" }) && userId.IsRobotAdmin())
            {
                return await _pregnantWomanEatMedicineRecordService.SubmitSuccess();
            }
            if (msg.ToLower().Contains("查询微信id") && userId.IsRobotAdmin())
            {
                var vxDto = await GetContactList(new ContactListContent()
                {
                    name = msg
                    .Replace("查询微信", string.Empty)
                    .Replace("id", string.Empty)
                    .Replace("ID", string.Empty)
                    .Replace("：", string.Empty)
                    .Replace(":", string.Empty)
                });
                return vxDto.content.FirstOrDefault()?.wxid;
            }
            if (msg.IsContainsAll(new string[] { "开", "主卧", "电脑" }) && userId.IsRobotAdmin())
            {
                await _wakeOnLanService.WOL("80:fa:5b:53:87:68", "通过机器人服务唤醒");
                return "已发送指令~";
            }
            if (msg.IsContainsAll(new string[] { "开", "书房", "电脑" }) && userId.IsRobotAdmin())
            {
                await _wakeOnLanService.WOL("8c:82:b9:51:14:21", "通过机器人服务唤醒");
                return "已发送指令~";
            }
            if (msg.IsContainsAll(new string[] { "开", "服务器" }) && userId.IsRobotAdmin())
            {
                await _wakeOnLanService.WOL("8c:82:b9:51:04:03", "通过机器人服务唤醒");
                return "已发送指令~";
            }
            if (msg.IsContainsAll(new string[] { "开", "工控机" }) && userId.IsRobotAdmin())
            {
                await _wakeOnLanService.WOL("00:e0:4c:68:d5:2d", "通过机器人服务唤醒");
                return "已发送指令~";
            }
            if (msg.IsContainsAll(new string[] { "开", "客厅", "电脑" }) && userId.IsRobotAdmin())
            {
                await _wakeOnLanService.WOL("00:e0:4c:68:d5:2d", "通过机器人服务唤醒");
                return "已发送指令~";
            }
            if (msg.IsContainsAll(new string[] { "关", "电脑" }) && userId.IsRobotAdmin())
            {
                try
                {
                    if (msg.Contains("主卧"))
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.ShutdownAction, ClientName = "XZM-Hasee" });
                        return "电脑将在一分钟内关闭~";
                    }
                    if (msg.Contains("书房"))
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.ShutdownAction, ClientName = "XZM-X58" });
                        return "电脑将在一分钟内关闭~";
                    }
                    if (msg.Contains("仪征"))
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.ShutdownAction, ClientName = "YZ-HOME" });
                        return "电脑将在一分钟内关闭~";
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            if (msg.IsContainsAll(new string[] { "重启", "电脑" }) && userId.IsRobotAdmin())
            {
                try
                {
                    if (msg.Contains("主卧"))
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.RebootAction, ClientName = "XZM-Hasee" });
                        return "电脑将在一分钟内重启~";
                    }
                    if (msg.Contains("书房"))
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.RebootAction, ClientName = "XZM-X58" });
                        return "电脑将在一分钟内重启~";
                    }

                    if (msg.Contains("仪征"))
                    {
                        await _remoteCommandService.SendRemoteCommand(new SendRemoteCommandInput() { ActionType = ActionType.RebootAction, ClientName = "YZ-HOME" });
                        return "电脑将在一分钟内重启~";
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            if ((msg.IsContainsAll(new string[] { "人", "在家" }) || msg.IsContainsAll(new string[] { "成员", "在家" })) && userId.IsRobotAdmin())
            {
                MqttHelper.PublishMqtt("PensionAtHome", _configuration.GetSection("MqTopicConfig:RemoteCommand").Value);
                return null;
            }
            if (msg.Contains("图书订阅"))
            {
                var bookName = msg.Replace("图书订阅", string.Empty).Replace(":", string.Empty).Replace("：", string.Empty);
                return await _bookSubscriptionService.CreateBookSubscription(bookName, userId.IsInt() ? MessageType.QQ : MessageType.Weixin, userId);
            }
            if (msg.Contains("儿子") && userId.IsRobotAdmin())
            {
                var records = _pregnantWomanEventRecordService.Queryable();

                var xyzInfo = "儿子相关信息如下:" + Environment.NewLine;
                xyzInfo += $"实时年龄:{(DateTime.Now - new DateTime(2022, 10, 5, 17, 48, 0)).TotalDays.ToString("0")}天" + Environment.NewLine;
                var eatMilkRecord = records.Where(c => c.PregnantWomanEventType == PregnantWomanEventType.EatMilkPowder || c.PregnantWomanEventType == PregnantWomanEventType.EatMotherMilk).OrderByDescending(c => c.CreateTime).FirstOrDefault();
                xyzInfo += $"上次喝奶时间:{eatMilkRecord?.CreateTime.ToString("yyyy-MM-dd HH:mm")}({eatMilkRecord?.PregnantWomanEventType.GetDescription()})" + Environment.NewLine;
                var eatVitaminRecord = records.Where(c => c.PregnantWomanEventType == PregnantWomanEventType.EatVD || c.PregnantWomanEventType == PregnantWomanEventType.EatVAD).OrderByDescending(c => c.CreateTime).FirstOrDefault();
                xyzInfo += $"上次吃维生素时间:{eatVitaminRecord?.CreateTime.ToString("yyyy-MM-dd HH:mm")}({eatVitaminRecord?.PregnantWomanEventType.GetDescription()})" + Environment.NewLine;
                var openMilkPowderRecord = records.Where(c => c.PregnantWomanEventType == PregnantWomanEventType.OpenMilkPowder).OrderByDescending(c => c.CreateTime).FirstOrDefault();
                xyzInfo += $"距离上次开奶粉已过 {(DateTime.Now - openMilkPowderRecord?.CreateTime)?.TotalDays.ToString("0")} 天" + Environment.NewLine;
                var opendiapersRecord = records.Where(c => c.PregnantWomanEventType == PregnantWomanEventType.Opendiapers).OrderByDescending(c => c.CreateTime).FirstOrDefault();
                xyzInfo += $"距离上次开尿不湿已过 {(DateTime.Now - opendiapersRecord?.CreateTime)?.TotalDays.ToString("0")} 天";
                return xyzInfo;
            }
            if (msg.IsContainsAny(new string[] { "停车", "车位", "汇智大厦" }))
            {
                var parkInfo = await _jieParkService.GetParkInfo("p181106653");
                return $"{parkInfo?.ParkName}:当前空车位{parkInfo?.EmptySpaces}个,总车位{parkInfo?.TotalSpaces}个";
            }
            if (msg.Contains("加入群组"))
            {
                var groupName = msg.Replace("加入群组", string.Empty).Replace(":", string.Empty).Replace("：", string.Empty);
                //群组是否允许快捷加入
                var group = await _mdmMessageGroupService.Queryable().AsNoTracking().Where(c => c.Name == groupName).FirstOrDefaultAsync();
                if (group == null)
                {
                    return $"群组[{groupName}]没有找到!";
                }
                if (!group.AllowQuickJoin)
                {
                    return $"群组[{group.Name}]配置了不允许快捷加入,请联系管理员!";
                }
                string receiverId = null;
                var receiver = await _mdmMessageReceiverService.Queryable().AsNoTracking().Where(c => c.Identification == userId).FirstOrDefaultAsync();
                if (receiver == null)
                {
                    //return $"接收者[{userId}]没有找到,请先进行登记操作!";
                    //自动创建接收者
                    receiverId = await _mdmMessageReceiverService.InsertAsync(new MdmMessageReceiver()
                    {
                        Code = "RobotUser",
                        Name = "群组订阅用户",
                        MessageType = userId.IsQq() ? MessageType.QQ : MessageType.Weixin,
                        Identification = userId,
                    });
                }
                else
                {
                    receiverId = receiver.Id;
                }
                if (await _mdmMessageGroupReceiverService.ExistsAsync(c => c.MessageGroupId == group.Id && c.MessageReceiverId == receiverId))
                {
                    return $"您已加入[{group.Name}]群组,无需重复加入!";
                }
                await _mdmMessageGroupReceiverService.InsertAsync(new MdmMessageGroupReceiver() { MessageGroupId = group.Id, MessageReceiverId = receiverId });
                return $"您已成功加入[{group.Name}]群组!";
            }
            if (msg.IsContainsAll(new string[] { "关闭", "监控" }) && userId.IsRobotAdmin())
            {
                var cameraName = msg.Replace("关闭", string.Empty).Replace("监控", string.Empty).Replace(":", string.Empty).Replace("：", string.Empty);
                var result = await _cameraService.SetCameraStatus(false, cameraName);
                if (result.IsBlank())
                {
                    result = $"{cameraName}监控已关闭";
                }
                return result;
            }
            if (msg.IsContainsAll(new string[] { "开启", "监控" }) && userId.IsRobotAdmin())
            {
                var cameraName = msg.Replace("开启", string.Empty).Replace("监控", string.Empty).Replace(":", string.Empty).Replace("：", string.Empty);
                var result = await _cameraService.SetCameraStatus(true, cameraName);
                if (result.IsBlank())
                {
                    result = $"{cameraName}监控已开启";
                }
                return result;
            }
            if (msg.IsContainsAny(new string[] { "新增违禁词", "添加违禁词" }) && userId.IsRobotAdmin())
            {
                var blockKeyword = msg.Replace("新增违禁词", string.Empty).Replace("添加违禁词", string.Empty).Replace(":", string.Empty).Replace("：", string.Empty);
                if (!await _blockKeywordService.ExistsAsync(c => c.Value == blockKeyword))
                {
                    await _blockKeywordService.InsertAsync(new BusBlockKeyword() { Value = blockKeyword });
                    return "添加成功";
                }
                else
                {
                    return "已存在此关键词,请勿重复添加";
                }
            }
            if (msg.IsContainsAny(new string[] { "创建提醒:", "创建提醒：" }))
            {
                var noticeTaskName = msg.Replace("创建提醒:", string.Empty).Replace("创建提醒：", string.Empty);
                string receiverId = null;
                MdmMessageReceiver receiver = null;
                if (groupId.IsBlank())
                {
                    receiver = await _mdmMessageReceiverService.Queryable().AsNoTracking().Where(c => c.Identification == userId).FirstOrDefaultAsync();
                }
                else
                {
                    receiver = await _mdmMessageReceiverService.Queryable().AsNoTracking().Where(c => c.Identification == groupId).FirstOrDefaultAsync();
                }
                if (receiver == null)
                {
                    //return $"接收者[{userId}]没有找到,请先进行登记操作!";
                    //自动创建接收者
                    receiverId = await _mdmMessageReceiverService.InsertAsync(new MdmMessageReceiver()
                    {
                        Code = "RobotUser",
                        Name = "提醒用户",
                        MessageType = groupId == null ? (userId.IsQq() ? MessageType.QQ : MessageType.Weixin) : MessageType.WeixinGroup,
                        Identification = groupId ?? userId,
                    });
                }
                else
                {
                    receiverId = receiver.Id;
                }
                var noticeTaskDto = new BusNoticeTask()
                {
                    Name = noticeTaskName,
                    MessageReceiverId = receiverId,
                };
                await _noticeTaskService.InsertAsync(noticeTaskDto);
                return $"提醒任务创建成功,请及时完善提醒时间~";
            }
            if (msg.IsContainsAny(new string[] { "创建抽奖:", "创建抽奖：" }))
            {
                var luckyDrawCode = msg.Replace("创建抽奖:", string.Empty).Replace("创建抽奖：", string.Empty);
                if (await _luckyDrawService.ExistsAsync(x => x.Code == luckyDrawCode))
                {
                    return "抽奖代码重复";
                }
                var luckyDrawDto = new BusLuckyDraw()
                {
                    Code = luckyDrawCode,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddDays(1),
                    MaxCount = 1,
                    WinCount = 1,
                };
                await _luckyDrawService.InsertAsync(luckyDrawDto);
                return $"抽奖活动创建成功{Environment.NewLine}开始时间:{luckyDrawDto.StartTime.ToString("yy-MM-dd HH:mm")}{Environment.NewLine}结束时间:{luckyDrawDto.EndTime.ToString("yy-MM-dd HH:mm")}{Environment.NewLine}每人可抽:{luckyDrawDto.MaxCount}次{Environment.NewLine}奖品总数:{luckyDrawDto.WinCount}个{Environment.NewLine}{Environment.NewLine}请发送\"{luckyDrawCode}\"进行抽奖吧~";
            }
            if (msg.IsContainsAll(new string[] { "开", "上下文" }))
            {
                BusChatGptContext context = null;
                context = await _busChatGptContextService.Queryable().Where(c => c.Identification == userId).FirstOrDefaultAsync();
                if (context == null)
                {
                    context = new BusChatGptContext()
                    {
                        Identification = userId,
                        IsOpen = true,
                        AvailableCount = 20,
                    };
                    await _busChatGptContextService.InsertAsync(context);
                }
                else
                {
                    context.IsOpen = true;
                    await _busChatGptContextService.UpdateAsync(context);
                }
                return $"已开启上下文功能,剩余可用{context.AvailableCount}次";
            }
            if (msg.IsContainsAll(new string[] { "关", "上下文" }))
            {
                BusChatGptContext context = null;
                context = await _busChatGptContextService.Queryable().Where(c => c.Identification == userId).FirstOrDefaultAsync();
                if (context == null)
                {
                    return "您没有开通过上下文功能,如需使用请对我说\"开启上下文\"";
                }
                else
                {
                    context.IsOpen = false;
                    await _busChatGptContextService.UpdateAsync(context);
                }
                return $"已关闭上下文功能,剩余可用{context.AvailableCount}次";
            }
            if (msg.Contains("查询上下文"))
            {
                BusChatGptContext context = null;
                context = await _busChatGptContextService.Queryable().Where(c => c.Identification == userId).FirstOrDefaultAsync();
                if (context == null)
                {
                    return "您没有开通过上下文功能,如需使用请对我说\"开启上下文\"";
                }
                else
                {
                    var chatContext = await _redisStackExchangeService.StringGetAsync($"ChatGPTContext:{userId}");
                    return $"您的上下文状态:{(context.IsOpen ? "已开启" : "已关闭")}{Environment.NewLine}已缓存长度:{chatContext?.Length}{Environment.NewLine}剩余可用{context.AvailableCount}次";
                }
            }
            if (msg.Contains("清空上下文"))
            {
                _ = await _redisStackExchangeService.StringSetAsync($"ChatGPTContext:{userId}", "", TimeSpan.FromSeconds(1));
                return "你的上下文缓存已清空";
            }

            #endregion


            #region 无条件逻辑
            var luckyDraw = await _luckyDrawService.GetLuckyDraw(msg, userId, groupId);
            if (luckyDraw.IsNotBlank())
            {
                return luckyDraw;
            }
            if (await _blockKeywordService.ExistsAsync(c => msg.Contains(c.Value) || userId == c.Value || groupId == c.Value))
            {
                return "存在违禁词,请稍后再试";
            }

            #endregion


            #region 机器人学讲话
            if (userId == "wxid_qglfsx2ntr5312")
            {
                return msg;
            }
            #endregion

            //return await GetTulingAiChat(msg, userId.Replace("wxid_", string.Empty));
            return await GetChatGPTAiChat(msg, userId);
        }

        /// <summary>
        /// ChatGPT聊天API
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetChatGPTAiChat(string msg, string userId)
        {
            var msgId = SnowFlake.NewId();
            try
            {
                _logger.LogInformation($"即将请求ChatGPT接口:{msg},本条消息唯一标识:{msgId}");
                var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
                client.DefaultRequestHeaders.Add("Authorization", _configuration.GetSection("OpenAiToken").Value);

                var stopTag = "<|im_end|>";
                var param = new ChatGPTInput();
                string chatContextNew = string.Empty;
                var context = await _busChatGptContextService.Queryable().Where(c => c.Identification == userId).FirstOrDefaultAsync();
                if (context?.IsOpen == true)
                {
                    if (context.AvailableCount <= 0)
                    {
                        return "你的上下文次数已用完,请明日再试!可以对我说\"关闭上下文\"使用普通模式~";
                    }
                    _logger.LogInformation($"已开启上下文,本条消息唯一标识:{msgId}");
                    //查询上下文缓存
                    var chatContext = await _redisStackExchangeService.StringGetAsync($"ChatGPTContext:{userId}");
                    chatContextNew = chatContext.IsBlank() ? $"Q: {msg}" : $"{chatContext}{Environment.NewLine}{stopTag}{Environment.NewLine}Q: {msg}";
                    param.prompt = chatContextNew + stopTag;
                    param.user = userId;
                    //param.stop = new string[] { "<|im_end|>" };
                    param.stop = stopTag;
                    _logger.LogInformation($"上下文组装参数:{param.prompt},本条消息唯一标识:{msgId}");
                }
                else
                {
                    param.prompt = msg + stopTag;
                    param.user = userId;
                    param.stop = stopTag;
                }

                var content = JsonContent.Create(param);
                var response = await client.PostAsync(_configuration.GetSection("UrlConfig:ChatGPTApi").Value, content);
                var result = (await response.Content.ReadAsStringAsync()).FromJSON<ChatGPTOutput>();
                _logger.LogInformation($"接收到ChatGPT回应:{result.ToJSON()},本条消息唯一标识:{msgId}");
                if (result.Error != null)
                {
                    //如果失败,延迟3秒重新请求一次
                    _logger.LogError($"ChatGPT请求过程出现问题(接口报错,第1次),本条消息唯一标识:{msgId}");
                    Thread.Sleep(3000);
                    _logger.LogInformation($"即将重试,ChatGPT请求参数:{param.ToJSON()},本条消息唯一标识:{msgId}");
                    response = await client.PostAsync(_configuration.GetSection("UrlConfig:ChatGPTApi").Value, content);
                    result = (await response.Content.ReadAsStringAsync()).FromJSON<ChatGPTOutput>();
                    if (result.Error != null)
                    {
                        //再次失败,使用备用接口
                        _logger.LogError($"ChatGPT请求过程出现问题(接口报错,第2次),本条消息唯一标识:{msgId}");
                        _logger.LogInformation($"ChatGPT机器人接口异常,即将使用备用机器人通道,本条消息唯一标识:{msgId}");
                        return await GetTulingAiChat(msg, userId.Replace("wxid_", string.Empty));
                    }
                    else
                    {
                        _logger.LogInformation($"接收到ChatGPT回应(重试成功):{result.ToJSON()},本条消息唯一标识:{msgId}");
                    }
                }
                var answer = result?.Choices.FirstOrDefault()?.Text.Trim();

                if (context?.IsOpen == true)
                {
                    _logger.LogInformation($"上下文收到回答:{answer},本条消息唯一标识:{msgId}");
                    answer = answer.Replace("A:", string.Empty).Replace("答：", string.Empty);
                    //设置缓存 追加本次问题和回答
                    _ = await _redisStackExchangeService.StringSetAsync($"ChatGPTContext:{userId}", $"{chatContextNew}{Environment.NewLine}{stopTag}{Environment.NewLine}A: {answer}", TimeSpan.FromDays(7));
                    _logger.LogInformation($"上下文缓存:{chatContextNew}{Environment.NewLine}A: {answer},本条消息唯一标识:{msgId}");
                    context.UsedCount = context.UsedCount + 1;
                    context.AvailableCount = context.AvailableCount - 1;
                    await _busChatGptContextService.UpdateAsync(context);
                }

                _ = await _redisStackExchangeService.CacheCountCheck("CountAnalysis:ChatGPT", TimeSpan.FromDays(365), 1);
                return answer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ChatGPT请求过程出现问题:{ex.Message},本条消息唯一标识:{msgId}");
                //return "ChatGPT接口异常,请稍后再试!";
                _logger.LogInformation($"ChatGPT机器人接口异常,即将使用备用机器人通道,本条消息唯一标识:{msgId}");
                return await GetTulingAiChat(msg, userId.Replace("wxid_", string.Empty));
            }


        }
    }
}
