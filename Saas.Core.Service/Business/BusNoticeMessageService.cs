using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;
using static XiaoFeng.Data.DataConfig;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 通知消息服务
    /// </summary>
    public class BusNoticeMessageService : Repository<BusNoticeMessage>
    {
        private IHttpClientFactory _httpClientFactory;
        private readonly MdmMessageGroupService _mdmMessageGroupService;
        private readonly IConfiguration Configuration;
        private readonly ICapPublisher _capPublisher;
        private readonly IRedisStackExchangeService _redisStackExchangeService;
        //private readonly RobotService _robotService;
        private readonly MdmMessageReceiverService _mdmMessageReceiverService;

        /// <summary>
        /// 
        /// </summary>
        public BusNoticeMessageService(MainDbContext context, IHttpClientFactory httpClientFactory, MdmMessageGroupService mdmMessageGroupService, IConfiguration configuration, ICurrentUserService currentUserService, ICapPublisher capPublisher, IRedisStackExchangeService redisStackExchangeService, MdmMessageReceiverService mdmMessageReceiverService) : base(context, currentUserService)
        {
            _httpClientFactory = httpClientFactory;
            _mdmMessageGroupService = mdmMessageGroupService;
            Configuration = configuration;
            _capPublisher = capPublisher;
            _redisStackExchangeService = redisStackExchangeService;
            _mdmMessageReceiverService = mdmMessageReceiverService;
        }

        /// <summary>
        /// 发送通知消息到指定通道(多条批量发送)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task PublishNoticeMessageListTo(List<PublishNoticeMessageToInput> input)
        {
            if (input != null && input.Count() != 0)
            {
                Random ra = new();
                foreach (var noticeMessage in input)
                {
                    await PublishNoticeMessageTo(noticeMessage);
                    //随机延迟,避免风控
                    Thread.Sleep(ra.Next(1000, 2000));
                }
            }
        }

        /// <summary>
        /// 发送通知消息到指定群组
        /// </summary>
        /// <param name="groupName">群组名称</param>
        /// <param name="text">消息内容</param>
        /// <param name="isSaveRecord">是否保存消息记录(默认保存)</param>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        public async Task PublishNoticeMessageToGroup(string groupName, string text, bool isSaveRecord = true, string groupId = null)
        {
            MdmMessageGroup group = null;
            if (groupId.IsNotBlank())
            {
                group = await _mdmMessageGroupService.FindAsync(groupId);
            }
            else if (groupName.IsNotBlank())
            {
                group = await _mdmMessageGroupService.Queryable().Where(c => c.Name == groupName).FirstOrDefaultAsync();
            }

            if (group?.MessageGroupReceivers != null && group?.MessageGroupReceivers.Count > 0)
            {
                Random ra = new();
                foreach (var item in group.MessageGroupReceivers)
                {
                    try
                    {
                        var noticeMessage = new PublishNoticeMessageToInput();
                        if (item.MessageReceiver?.MessageType == MessageType.QQ)
                        {
                            noticeMessage.Qq_Receiver = item.MessageReceiver?.Identification;
                        }
                        else if (item.MessageReceiver?.MessageType == MessageType.Weixin)
                        {
                            noticeMessage.Wx_Wxid = item.MessageReceiver?.Identification;
                        }
                        else if (item.MessageReceiver?.MessageType == MessageType.WeixinGroup)
                        {
                            noticeMessage.Wx_Roomid = item.MessageReceiver?.Identification;
                            noticeMessage.Wx_Nickname = "所有人";
                        }
                        noticeMessage.Text = text;
                        noticeMessage.IsSaveRecord = isSaveRecord;

                        await PublishNoticeMessageTo(noticeMessage);
                        //随机延迟,避免风控
                        Thread.Sleep(ra.Next(1000, 2000));
                        //_capPublisher.Publish(SystemConst.NoticeMessage, noticeMessage);}

                    }
                    catch (Exception ex) { }
                }
            }
        }

        /// <summary>
        /// 根据接收者Id发送消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<bool> PublishNoticeMessageByMessageReceiverId(string id, string text)
        {
            var receiver = await _mdmMessageReceiverService.FindAsNoTrackingAsync(id);
            var sendInput = new PublishNoticeMessageToInput();
            sendInput.Text = text;
            if (receiver.MessageType == MessageType.QQ)
            {
                sendInput.Qq_Receiver = receiver.Identification;
            }
            if (receiver.MessageType == MessageType.Weixin)
            {
                sendInput.Wx_Wxid = receiver.Identification;
            }
            if (receiver.MessageType == MessageType.WeixinGroup)
            {
                sendInput.Wx_Roomid = receiver.Identification;
            }
            await PublishNoticeMessageTo(sendInput);
            return true;
        }

        /// <summary>
        /// 发送通知消息到指定通道
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ReturnDto> PublishNoticeMessageTo(PublishNoticeMessageToInput input)
        {

            try
            {
                if (input.Text.IsBlank())
                {
                    throw new BusinessException("消息内容为空,无法发送!");
                }
                if (input.Qq_Receiver.IsBlank() && input.Wx_Wxid.IsBlank() && input.Wx_Roomid.IsBlank())
                {
                    throw new BusinessException("消息接收者(接收通道)为空,无法发送!");
                }
                if (input.Qq_Sender.IsBlank())
                {
                    input.Qq_Sender = Configuration.GetSection("MiraiBot:QQ").Value;
                }
                //去除消息收尾换行
                input.Text = input.Text.Trim();
                input.Text = input.Text.TrimStart(Environment.NewLine.ToCharArray());
                input.Text = input.Text.TrimEnd(Environment.NewLine.ToCharArray());

                var noticeMessages = new List<BusNoticeMessage>();

                var errorMsg = "";

                try
                {
                    //QQ通道
                    if (input.Qq_Receiver.IsNotBlank())
                    {
                        var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);


                        //认证
                        var paramVerify = new MiraiVerify { };
                        HttpContent contentVerify = new StringContent(paramVerify.ToJSON());
                        var responseVerify = await client.PostAsync($"http://{Configuration.GetSection("MiraiBot:Address").Value}/verify", contentVerify);
                        var miraiVerifyResponse = (await responseVerify.Content.ReadAsStringAsync()).FromJSON<MiraiVerifyResponse>();

                        //绑定
                        var paramBind = new MiraiBindAndRelease
                        {
                            sessionKey = miraiVerifyResponse.session,
                            qq = input.Qq_Sender,
                        };
                        HttpContent contentBind = new StringContent(paramBind.ToJSON());
                        var responseBind = await client.PostAsync($"http://{Configuration.GetSection("MiraiBot:Address").Value}/bind", contentBind);
                        var miraiBindResponse = await responseBind.Content.ReadAsStringAsync();

                        //发消息
                        var messageChain = new List<MessageChain>();
                        messageChain.Add(new MessageChain
                        {
                            text = input.Text
                        });
                        var paramSend = new MiraisSendFriendMessage
                        {
                            sessionKey = miraiVerifyResponse.session,
                            target = input.Qq_Receiver,
                            qq = input.Qq_Receiver,
                            messageChain = messageChain,
                        };
                        HttpContent contentSend = new StringContent(paramSend.ToJSON());
                        var responseSend = await client.PostAsync($"http://{Configuration.GetSection("MiraiBot:Address").Value}/sendFriendMessage", contentSend);
                        var miraisSendResponse = (await responseSend.Content.ReadAsStringAsync()).FromJSON<MiraisSendFriendMessageResponse>();

                        //释放
                        var paramRelease = new MiraiBindAndRelease
                        {
                            sessionKey = miraiVerifyResponse.session,
                            qq = input.Qq_Sender,
                        };
                        HttpContent contentRelease = new StringContent(paramRelease.ToJSON());
                        var responseRelease = await client.PostAsync($"http://{Configuration.GetSection("MiraiBot:Address").Value}/release", contentRelease);
                        var miraiReleaseResponse = await responseRelease.Content.ReadAsStringAsync();

                        noticeMessages.Add(new BusNoticeMessage
                        {
                            Id = SnowFlake.NewId(),
                            MsgText = input.Text,
                            SendTime = DateTime.Now,
                            MessageType = MessageType.QQ,
                            Receiver = input.Qq_Receiver,
                            Sender = input.Qq_Sender,
                            IsSendSuccess = miraisSendResponse.code == 0 ? true : false,
                            FailureReasons = miraisSendResponse.code == 0 ? null : miraisSendResponse.msg,
                        });


                    }
                }
                catch (Exception ex)
                {
                    errorMsg += $"QQ通道发送失败:{ex.Message}";

                }


                try
                {
                    //微信通道
                    if (input.Wx_Wxid.IsNotBlank() || input.Wx_Roomid.IsNotBlank())
                    {
                        var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);

                        int type = 0;
                        string url = Configuration.GetSection("UrlConfig:WeixinRobotHttp").Value;
                        WeixinResult<List<ContactListContent>> contactList = null;
                        if (input.Wx_Wxid.IsNotBlank() && input.Wx_Roomid.IsBlank())
                        {
                            type = 555;//私聊消息
                            url += "/api/sendtxtmsg";
                        }
                        else
                        {
                            type = 550;//群内@消息
                            url += "/api/sendatmsg";

                            //查询缓存
                            contactList = await _redisStackExchangeService.StringGetAsync<WeixinResult<List<ContactListContent>>>("ContactList");
                            if (contactList == null)
                            {
                                contactList = await GetContactList(new ContactListContent());
                                //设置缓存
                                await _redisStackExchangeService.StringSetAsync<WeixinResult<List<ContactListContent>>>("ContactList", contactList, TimeSpan.FromDays(1));
                            }
                        }

                        //发消息
                        var sendWeixinDto = new WeixinApiDto()
                        {
                            para = new WeixinPara()
                            {
                                id = DateTime.Now.ToString("yyyyMMddHHmmss"),
                                type = type,
                                roomid = input.Wx_Roomid ?? "",
                                wxid = input.Wx_Wxid ?? "",
                                content = input.Text ?? "",
                                nickname = input.Wx_Nickname,
                                ext = "",
                            }
                        };
                        if (sendWeixinDto.para.nickname.IsBlank())
                        {
                            if (sendWeixinDto.para.wxid.IsNotBlank())
                            {
                                sendWeixinDto.para.nickname = contactList?.content?.FirstOrDefault(c => c.wxid == input.Wx_Wxid)?.name ?? "";
                                //如果不是机器人好友,且有群id,则通过下列接口获取昵称
                                if (sendWeixinDto.para.nickname.IsBlank() && sendWeixinDto.para.roomid.IsNotBlank())
                                {
                                    sendWeixinDto.para.nickname = (await GetMemberNick(sendWeixinDto.para.wxid, sendWeixinDto.para.roomid))?.content?.nick ?? "";
                                }
                            }
                            else
                            {
                                sendWeixinDto.para.nickname = "所有人";
                            }
                        }

                        HttpContent content = new StringContent(sendWeixinDto.ToJSON());
                        var response = await client.PostAsync(url, content);
                        var httpResponse = await response.Content.ReadAsStringAsync();


                        noticeMessages.Add(new BusNoticeMessage
                        {
                            Id = SnowFlake.NewId(),
                            MsgText = input.Text,
                            SendTime = DateTime.Now,
                            MessageType = MessageType.Weixin,
                            Receiver = input.Wx_Wxid,
                            Sender = "",
                            IsSendSuccess = httpResponse.Contains("SUCCSESSED") ? true : false,
                            FailureReasons = httpResponse.Contains("SUCCSESSED") ? null : httpResponse,

                        });
                    }
                }
                catch (Exception ex)
                {
                    errorMsg += $"微信通道发送失败:{ex.Message}";
                }


                if (input.IsSaveRecord)
                {
                    await BatchInsertAsync(noticeMessages);
                }
                if (errorMsg.IsNotBlank())
                {
                    throw new BusinessException(errorMsg);
                }

                return new ReturnDto() { Data = noticeMessages };
            }
            catch (Exception ex)
            {

                Insert(new BusNoticeMessage
                {
                    Id = SnowFlake.NewId(),
                    MsgText = input.Text,
                    SendTime = DateTime.Now,
                    MessageType = MessageType.Exception,
                    Receiver = $"{input.Qq_Receiver},{input.Wx_Wxid}",
                    Sender = $"{input.Qq_Sender}",
                    IsSendSuccess = false,
                    FailureReasons = ex.Message,
                });
                throw new BusinessException(ex.Message);
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
            var response = await client.PostAsync($"{Configuration.GetSection("UrlConfig:WeixinRobotHttp").Value}/api/getcontactlist", content);
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
            var response = await client.PostAsync($"{Configuration.GetSection("UrlConfig:WeixinRobotHttp").Value}/api/getmembernick", content);
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
    }
}
