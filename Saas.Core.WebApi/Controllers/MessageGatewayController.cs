using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 消息网关
    /// </summary>
    public class MessageGatewayController : BaseApiController
    {

        private readonly ILogger<MessageGatewayController> _logger;
        private readonly MainDbContext _dbContext;
        private IHttpClientFactory _httpClientFactory;
        private readonly BusNoticeMessageService _noticeMessageService;
        private readonly IConfiguration Configuration;


        /// <summary>
        /// 
        /// </summary>
        public MessageGatewayController(
            ILogger<MessageGatewayController> logger,
            MainDbContext myDbContext,
            IHttpClientFactory httpClientFactory,
            BusNoticeMessageService noticeMessageService,
            IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = myDbContext;
            _httpClientFactory = httpClientFactory;
            _noticeMessageService = noticeMessageService;
            Configuration = configuration;
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<BusNoticeMessage>> Get()
        {

            List<BusNoticeMessage> list = await _noticeMessageService.Queryable().OrderByDescending(c => c.Id).Take(100).ToListAsync();
            return list;

        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<BusNoticeMessage>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
        {

            var list = await _noticeMessageService.Queryable().OrderByDescending(c => c.Id).PagingResultAsync(filter.PageIndex, filter.PageSize, filter.SortField, filter.SortType);
            return list;

        }

        /// <summary>
        /// 发送通知消息到消息网关(弃用)
        /// </summary>
        /// <param name="msg">消息内容(发给家庭成员)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Obsolete]
        public async Task<bool> PublishNoticeMessage(string msg)
        {
            if (msg.IsBlank())
            {
                throw new BusinessException("消息内容为空,无法发送!");
            }
            if (MqttHelper.PublishMqtt(msg, Configuration.GetSection("MqTopicConfig:MsgGateway").Value))
            {
                var newRecord = new BusNoticeMessage
                {
                    Id = SnowFlake.NewId(),
                    MsgText = msg,
                    SendTime = DateTime.Now,
                    MessageType = MessageType.MessageGateway,
                    Receiver = "网关指定接收者",
                    Sender = "消息网关",

                };
                //await _dbContext.NoticeMessage.AddAsync(newRecord);
                //await _dbContext.SaveChangesAsync();
                await _noticeMessageService.InsertAsync(newRecord);

                return true;
            }
            else
            {
                throw new BusinessException("消息发送失败,请查看日志!");
            }
        }

        /// <summary>
        /// 发送通知消息到消息网关(使用pushplus相同入参)(弃用)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Obsolete]
        public async Task<bool> PublishNoticeMessagePost(PublishNoticeMessagePostInput input)
        {
            if (input.template != "json")
            {
                throw new BusinessException("不支持的消息体,无法发送");
            }
            var msg = "";
            var content = input.content;
            switch (input.title)
            {
                case "您的电脑被锁定了!":
                    msg = $"{content.电脑}({content.IP})被锁定了!";
                    break;
                case "您的电脑被解锁了!":
                    msg = $"{content.电脑}({content.IP})被解锁了!";
                    break;
                case "您的电脑开机了!":
                    msg = $"{content.电脑}({content.IP})开机了!";
                    break;

                default:
                    break;
            }

            if (MqttHelper.PublishMqtt(msg, Configuration.GetSection("MqTopicConfig:MsgGateway").Value))
            {

                var newRecord = new BusNoticeMessage
                {
                    Id = SnowFlake.NewId(),
                    MsgText = msg,
                    SendTime = DateTime.Now,
                    MessageType = MessageType.ComputerOperation,
                    Receiver = "网关指定接收者",
                    Sender = "消息网关",

                };
                //await _dbContext.NoticeMessage.AddAsync(newRecord);
                //await _dbContext.SaveChangesAsync();
                await _noticeMessageService.InsertAsync(newRecord);

                return true;
            }
            else
            {
                throw new BusinessException("消息发送失败,请查看日志!");
            }
        }


        /// <summary>
        /// 发送通知消息到指定通道
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<object> PublishNoticeMessageTo(PublishNoticeMessageToInput input)
        {

            var result = await _noticeMessageService.PublishNoticeMessageTo(input);
            if (string.IsNullOrEmpty(result.Message))
            {
                return result.Data;
            }
            else
            {
                throw new BusinessException(result.Message);
            }


        }


        /// <summary>
        /// 发送通知消息到指定通道
        /// </summary>
        /// <param name="text">消息内容</param>
        /// <param name="qq_Sender">QQ通道-发送者(可选)</param>
        /// <param name="qq_Receiver">QQ通道-接收者</param>
        /// <param name="wx_Wxid">微信通道-接收用户Id</param>
        /// <param name="wx_Roomid">微信通道-接收群Id(可选)</param>
        /// <param name="wx_Nickname">微信通道-接收人群内昵称(可选)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<object> GetPublishNoticeMessageTo(string text, string qq_Sender, string qq_Receiver, string wx_Wxid, string wx_Roomid, string wx_Nickname)
        {

            try
            {
                var result = await _noticeMessageService.PublishNoticeMessageTo(new PublishNoticeMessageToInput()
                {
                    Text = text,
                    Qq_Sender = qq_Sender,
                    Qq_Receiver = qq_Receiver,
                    Wx_Wxid = wx_Wxid,
                    Wx_Roomid = wx_Roomid,
                    Wx_Nickname = wx_Nickname,
                });
                if (string.IsNullOrEmpty(result.Message))
                {
                    return result.Data;
                }
                else
                {
                    throw new BusinessException(result.Message);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// 发送通知消息到指定群组
        /// </summary>
        /// <param name="groupName">群组名称</param>
        /// <param name="text">消息内容</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<bool> GetPublishNoticeMessageToGroup(string groupName, string text)
        {

            try
            {
                await _noticeMessageService.PublishNoticeMessageToGroup(groupName, text);
                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }



    }
}





