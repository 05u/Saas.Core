using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 小爱音箱
    /// </summary>
    public class XiaoaiController : BaseApiController
    {

        private readonly ILogger<XiaoaiController> _logger;
        private readonly MdmXiaoaiService _xiaoaiService;
        private readonly MainDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly BusNoticeMessageService _busNoticeMessageService;

        /// <summary>
        /// 
        /// </summary>
        public XiaoaiController(ILogger<XiaoaiController> logger, MdmXiaoaiService xiaoaiService, MainDbContext dbContext, IConfiguration configuration)
        {
            _logger = logger;
            _xiaoaiService = xiaoaiService;
            _dbContext = dbContext;
            _configuration = configuration;
        }


        /// <summary>
        /// 发送通知消息到指定小爱
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="code">小爱代码</param>
        /// <param name="name">小爱名称</param>
        /// <param name="deviceID">小米平台小爱设备标识</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<bool> PublishXiaoaiTTS(string msg, string code, string name, string deviceID)
        {
            if (msg.IsBlank())
            {
                throw new BusinessException("消息内容为空, 无法发送!");
            }
            if (code.IsBlank() && name.IsBlank() && deviceID.IsBlank())
            {
                throw new BusinessException("未指定小爱音箱,无法发送!");
            }

            var xiaoai = await _dbContext.MdmXiaoaiSpeaker
                .WhereIf(deviceID.IsNotBlank(), c => c.DeviceID == deviceID)
                .WhereIf(code.IsNotBlank(), c => c.Code.ToUpper().Contains(code.ToUpper()))
                .WhereIf(name.IsNotBlank(), c => c.Name.Contains(name))
                .ToListAsync();
            if (xiaoai.Count == 0 && deviceID.IsBlank())
            {
                throw new BusinessException("未查询到小爱音箱,无法发送!");
            }
            if (xiaoai.Count > 1)
            {
                throw new BusinessException($"查询到多个小爱音箱,请明确:{string.Join(",", xiaoai.ConvertAll(c => c.Name).ToArray())}");
            }

            var sendMsg = new SendXiaoaiMsg
            {
                name = xiaoai[0]?.Name ?? name,
                text = msg,
            };


            if (MqttHelper.PublishMqtt(JsonNewtonsoft.ToJSON(sendMsg), _configuration.GetSection("MqTopicConfig:XiaoaiTTSv2").Value))
            {
                var newRecord = new BusNoticeMessage
                {
                    Id = SnowFlake.NewId(),
                    MsgText = msg,
                    SendTime = DateTime.Now,
                    MessageType = MessageType.XiaoaiTTS,
                    Receiver = $"{xiaoai[0]?.Name ?? name},{xiaoai[0]?.DeviceID}",
                    Sender = "小爱TTS分发网关",

                };
                //await _dbContext.NoticeMessage.AddAsync(newRecord);
                //await _dbContext.SaveChangesAsync();
                await _busNoticeMessageService.InsertAsync(newRecord);

                return true;
            }
            else
            {
                throw new BusinessException("消息发送失败,请查看日志!");
            }
        }


        /// <summary>
        /// 发送通知消息到全部小爱
        /// </summary>
        /// <param name="msg">消息内容(发给家庭成员)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> PublishAllXiaoaiTTS(string msg)
        {
            if (msg.IsBlank())
            {
                throw new BusinessException("消息内容为空,无法发送!");
            }
            if (MqttHelper.PublishMqtt(msg, _configuration.GetSection("MqTopicConfig:XiaoaiTTSAll").Value))
            {
                var newRecord = new BusNoticeMessage
                {
                    Id = SnowFlake.NewId(),
                    MsgText = msg,
                    SendTime = DateTime.Now,
                    MessageType = MessageType.XiaoaiTTS,
                    Receiver = "全部SGD小爱",
                    Sender = "小爱TTS分发网关",

                };
                //await _dbContext.NoticeMessage.AddAsync(newRecord);
                //await _dbContext.SaveChangesAsync();
                await _busNoticeMessageService.InsertAsync(newRecord);

                return true;
            }
            else
            {
                throw new BusinessException("消息发送失败,请查看日志!");
            }
        }


        /// <summary>
        /// 小爱消息处理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> XiaoaiMessageProcess(string msg, string name)
        {
            return await _xiaoaiService.GeneralMessageProcess(msg, name);
        }





    }
}





