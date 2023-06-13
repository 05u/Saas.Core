using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 工作任务记录服务
    /// </summary>
    public class BusWorkTaskRecordService : Repository<BusWorkTaskRecord>
    {

        private readonly BusNoticeMessageService _noticeMessageService;

        /// <summary>
        /// 
        /// </summary>
        public BusWorkTaskRecordService(MainDbContext context, BusNoticeMessageService noticeMessageService, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _noticeMessageService = noticeMessageService;
        }

        /// <summary>
        /// 发送工作任务通知
        /// </summary>
        public async Task SendWorkTaskNotice()
        {

            var noticeList = await Queryable().Where(c => !c.IsNotice && DateTime.Now >= c.PlanNoticeTime).ToListAsync();
            var msgList = new List<PublishNoticeMessageToInput>();
            noticeList.ForEach(x =>
            {
                var overdueTag = x.PlanNoticeTime < DateTime.Now ? " [已过期]" : "";
                msgList.Add(new PublishNoticeMessageToInput()
                {
                    Text = $"{x.Name},您有{x.WorkDate.Year}年{x.WorkDate.Month}月{x.WorkDate.Day}日的{x.WorkClassType.GetDescription()}即将开始,请做好准备!{overdueTag}",
                    Wx_Wxid = x.Wxid,
                    IsSaveRecord = false,
                });
                x.IsNotice = true;

            });
            if (noticeList.Count() != 0)
            {
                await BatchUpdateAsync(noticeList);
                await _noticeMessageService.PublishNoticeMessageListTo(msgList);
            }
        }


    }
}
