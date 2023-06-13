using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 孕妇吃药记录服务
    /// </summary>
    public class BusPregnantWomanEatMedicineRecordService : Repository<BusPregnantWomanEatMedicineRecord>
    {
        private IHttpClientFactory _httpClientFactory;
        private readonly BusNoticeMessageService _noticeMessageService;

        /// <summary>
        /// 
        /// </summary>
        public BusPregnantWomanEatMedicineRecordService(MainDbContext context, IHttpClientFactory httpClientFactory, BusNoticeMessageService noticeMessageService, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _httpClientFactory = httpClientFactory;
            _noticeMessageService = noticeMessageService;
        }

        /// <summary>
        /// 孕妇吃药记录生成
        /// </summary>
        /// <returns></returns>
        public async Task PregnantWomanEatMedicineRecordCreat()
        {
            var nowDate = DateTime.Now.Date;
            var zaoStartTime = nowDate.AddHours(8.5);
            var zaoEndTime = nowDate.AddHours(9.5);
            var zhongStartTime = nowDate.AddHours(12);
            var zhongEndTime = nowDate.AddHours(14);
            var wanStartTime = nowDate.AddHours(21);
            var wanEndTime = nowDate.AddHours(22.5);

            ////早上
            //await InsertAsync(new PregnantWomanEatMedicineRecord
            //{
            //    StartTime = zaoStartTime,
            //    EndTime = zaoEndTime,
            //    MedicineName = "琥珀酸亚铁片",
            //    Remark = "无",
            //});


            ////中午
            //await InsertAsync(new PregnantWomanEatMedicineRecord
            //{
            //    StartTime = zhongStartTime,
            //    EndTime = zhongEndTime,
            //    MedicineName = "琥珀酸亚铁片",
            //    Remark = "无",
            //});

            //晚上
            await InsertAsync(new BusPregnantWomanEatMedicineRecord
            {
                StartTime = wanStartTime,
                EndTime = wanEndTime,
                MedicineName = "钙片",
                Remark = "无",
            });
        }

        /// <summary>
        /// 孕妇吃药上报
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Submit(SubmitInput input)
        {
            if (input.IsEatSuccess == null)
            {
                throw new BusinessException("是否吃完状态是必填项,请确认后再次提交!");
            }
            var nowTime = DateTime.Now;
            var record = Queryable().Where(c => c.StartTime <= nowTime && nowTime <= c.EndTime).FirstOrDefault();
            if (record == null)
            {
                throw new BusinessException("当前时段未查询到吃药需求,请确认后再次提交!");
            }
            if (input.IsEatSuccess == false && input.FailRemark.IsBlank())
            {
                throw new BusinessException("无法提交,因为没吃药情况下,没吃原因是必填的!");
            }
            record.IsEatSuccess = (bool)input.IsEatSuccess;
            record.FailRemark = input.FailRemark;
            await UpdateAsync(record);
            return true;
        }

        /// <summary>
        /// 孕妇吃药上报--成功
        /// </summary>
        /// <returns></returns>
        public async Task<string> SubmitSuccess()
        {
            var nowTime = DateTime.Now;
            var record = Queryable().Where(c => c.StartTime <= nowTime && nowTime <= c.EndTime).FirstOrDefault();
            if (record == null)
            {
                return "当前时段未查询到吃药需求,请确认后再次提交!";
            }
            record.IsEatSuccess = true;
            await UpdateAsync(record);
            return "恭喜你吃药成功,每天按时吃药有助于积累信用哦~";
        }


        /// <summary>
        /// 发送吃药通知
        /// </summary>
        /// <returns></returns>
        public async Task SendEatMedicineNotice()
        {
            var nowTime = DateTime.Now;
            var record = Queryable().Where(c => c.StartTime <= nowTime && nowTime <= c.EndTime).FirstOrDefault();

            if (record != null && record.IsEatSuccess == null)
            {
                record.NoticeSecond += 1;
                await UpdateAsync(record);
                var text = $"吃药提醒来啦~[第{record.NoticeSecond}次]{Environment.NewLine}药品名称: {record.MedicineName}{Environment.NewLine}服用说明: {record.Remark}{Environment.NewLine}吃完请对我说=>我已吃完";

                await _noticeMessageService.PublishNoticeMessageToGroup("双人组", text, false);

            }

            var overdueRecord = Queryable().Where(c => c.EndTime < nowTime && c.Fines == null && c.IsEatSuccess != true).FirstOrDefault();//过期了,没罚款,药没吃
            if (overdueRecord != null)
            {
                //查一下上一次罚款金额
                var lastFines = Queryable().Where(c => c.Fines != null).OrderByDescending(c => c.Fines).FirstOrDefault().Fines;
                overdueRecord.Fines = lastFines + 1;
                await UpdateAsync(overdueRecord);

                var text = $"您的药品逾期未吃~{Environment.NewLine}药品名称: {overdueRecord.MedicineName}{Environment.NewLine}服用说明: {overdueRecord.Remark}{Environment.NewLine}罚款金额: {overdueRecord.Fines}元";

                await _noticeMessageService.PublishNoticeMessageToGroup("双人组", text, false);

            }
        }
    }
}
