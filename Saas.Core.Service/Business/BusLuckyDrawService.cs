using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 幸运抽奖
    /// </summary>
    public class BusLuckyDrawService : Repository<BusLuckyDraw>
    {
        private readonly ILogger<BusLuckyDrawService> _logger;
        private readonly BusLuckyDrawRecordService _luckyDrawRecordService;
        private readonly MdmMessageReceiverService _mdmMessageReceiverService;
        private readonly IRedisStackExchangeService _redisStackExchangeService;
        private readonly BusNoticeMessageService _noticeMessageService;

        /// <summary>
        /// ctor
        /// </summary>
        public BusLuckyDrawService(MainDbContext context, ILogger<BusLuckyDrawService> logger, ICurrentUserService currentUserService, BusLuckyDrawRecordService luckyDrawRecordService, MdmMessageReceiverService mdmMessageReceiverService, IRedisStackExchangeService redisStackExchangeService, BusNoticeMessageService noticeMessageService) : base(context, currentUserService)
        {
            _logger = logger;
            _luckyDrawRecordService = luckyDrawRecordService;
            _mdmMessageReceiverService = mdmMessageReceiverService;
            _redisStackExchangeService = redisStackExchangeService;
            _noticeMessageService = noticeMessageService;
        }

        /// <summary>
        /// 抽奖一次
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetLuckyDraw(string msg, string userId, string groupId = null)
        {
            try
            {
                _logger.LogInformation($"即将抽奖,{userId},{groupId}");
                string result = "";
                var now = DateTime.Now;
                if (await ExistsAsync(c => msg.Contains(c.Code)))
                {
                    var luckyDraw = await Queryable().Include(c => c.LuckyDrawRecords).ThenInclude(c => c.MessageReceiver).Where(c => c.IsFinish == false && c.StartTime <= now && c.EndTime >= now).Where(c => msg.Contains(c.Code)).ToListAsync();
                    foreach (var x in luckyDraw)
                    {
                        var recordCount = x.LuckyDrawRecords.Where(c => c.MessageReceiver.Identification == userId).Count();
                        if (recordCount >= x.MaxCount)
                        {
                            _logger.LogInformation($"抽奖用完,{userId},{groupId}");
                            return "抽奖次数已用完";
                        }
                        string receiverId = null;
                        var receiver = await _mdmMessageReceiverService.Queryable().Where(c => c.Identification == userId).FirstOrDefaultAsync();
                        if (receiver == null)
                        {
                            //自动创建接收者
                            receiverId = await _mdmMessageReceiverService.InsertAsync(new MdmMessageReceiver()
                            {
                                Code = "RobotUser",
                                Name = "抽奖用户",
                                MessageType = userId.IsQq() ? MessageType.QQ : MessageType.Weixin,
                                Identification = userId,
                            });
                        }
                        else
                        {
                            receiverId = receiver.Id;
                        }
                        //创建抽奖记录
                        var no = (await _redisStackExchangeService.CacheCountCheck($"LuckyDraw:{x.Code}", TimeSpan.FromDays(365), 1)).FirstOrDefault();
                        await _luckyDrawRecordService.InsertAsync(new BusLuckyDrawRecord()
                        {
                            No = no,
                            LuckyDrawId = x.Id,
                            MessageReceiverId = receiverId,
                            GroupId = groupId,
                        });
                        result += $"抽奖编号:{no},开奖时间:{x.EndTime.ToString("yy-MM-dd HH:mm")},开奖数量:{x.WinCount}";
                    }
                }
                _logger.LogInformation($"抽奖完成,{userId},{groupId}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"抽奖出错:{ex.Message}");
                return "当前抽奖人数较多,请稍后再试~";
            }
        }


        /// <summary>
        /// 开奖
        /// </summary>
        /// <returns></returns>
        public async Task FinishLuckyDraw()
        {
            var list = await Queryable().Where(c => c.IsFinish == false && c.EndTime <= DateTime.Now).ToListAsync();
            //var recordList = new List<LuckyDrawRecord>();
            Random ra = new Random();
            foreach (var x in list)
            {
                if (x.LuckyDrawRecords.Count > 0)
                {
                    var min = x.LuckyDrawRecords.Select(c => c.No).Min();
                    var max = x.LuckyDrawRecords.Select(c => c.No).Max();
                    var winNoList = new List<int>();
                    for (int i = 0; i < x.LuckyDrawRecords.Count; i++)
                    {
                        var winNo = ra.Next(min, max);
                        if (!winNoList.Contains(winNo))
                        {
                            winNoList.Add(winNo);
                        }
                        if (winNoList.Count >= x.WinCount)
                        {
                            break;
                        }
                    }
                    foreach (var item in x.LuckyDrawRecords.Where(c => winNoList.Contains(c.No)))
                    {
                        item.IsWin = true;
                        var sendInput = new PublishNoticeMessageToInput();
                        sendInput.Text = $"恭喜中奖,你的抽奖编号是{item.No}";
                        if (item.MessageReceiver.MessageType == MessageType.QQ)
                        {
                            sendInput.Qq_Receiver = item.MessageReceiver.Identification;
                        }
                        if (item.MessageReceiver.MessageType == MessageType.Weixin)
                        {
                            sendInput.Wx_Wxid = item.MessageReceiver.Identification;
                            sendInput.Wx_Roomid = item.GroupId;
                        }
                        await _noticeMessageService.PublishNoticeMessageTo(sendInput);
                    }
                }
                x.IsFinish = true;
            }
            await BatchUpdateAsync(list);

        }
    }
}
