using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
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
    /// 通知提醒任务
    /// </summary>
    public class BusNoticeTaskService : Repository<BusNoticeTask>
    {
        private readonly ILogger<BusNoticeTaskService> _logger;
        private readonly BusNoticeMessageService _noticeMessageService;

        /// <summary>
        /// ctor
        /// </summary>
        public BusNoticeTaskService(MainDbContext context, ILogger<BusNoticeTaskService> logger, ICurrentUserService currentUserService, BusNoticeMessageService noticeMessageService) : base(context, currentUserService)
        {
            _logger = logger;
            _noticeMessageService = noticeMessageService;
        }

        /// <summary>
        /// 发出通知提醒消息
        /// </summary>
        /// <returns></returns>
        public async Task SendNoticeTask()
        {
            var list = await Queryable().Where(c => c.NextTime != null && c.NextTime <= DateTime.Now).ToListAsync();
            foreach (var item in list)
            {
                await _noticeMessageService.PublishNoticeMessageByMessageReceiverId(item.MessageReceiverId, $"通知提醒:{Environment.NewLine}{item.Name}");
                switch (item.NoticeTaskType)
                {
                    case Infrastructure.Enums.NoticeTaskType.Once:
                        item.NextTime = null;
                        break;
                    case Infrastructure.Enums.NoticeTaskType.EveryDay:
                        item.NextTime = item.NextTime?.AddDays(1);
                        break;
                    case Infrastructure.Enums.NoticeTaskType.EveryWeek:
                        item.NextTime = item.NextTime?.AddDays(7);
                        break;
                    case Infrastructure.Enums.NoticeTaskType.EveryMonth:
                        item.NextTime = item.NextTime?.AddMonths(1);
                        break;
                    case Infrastructure.Enums.NoticeTaskType.EveryQuarter:
                        item.NextTime = item.NextTime?.AddMonths(3);
                        break;
                    case Infrastructure.Enums.NoticeTaskType.EveryHalfYear:
                        item.NextTime = item.NextTime?.AddMonths(6);
                        break;
                    case Infrastructure.Enums.NoticeTaskType.EveryYear:
                        item.NextTime = item.NextTime?.AddYears(1);
                        break;
                    default:
                        break;
                }
            };
            await BatchUpdateAsync(list);
        }
    }
}
