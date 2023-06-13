using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    /// 孕产妇业务
    /// </summary>
    public class PregnantWomanEatMedicineRecordController : BaseApiController
    {

        private readonly ILogger<RemoteCommandController> _logger;
        private readonly MainDbContext _dbContext;
        private readonly BusPregnantWomanEatMedicineRecordService _pregnantWomanEatMedicineRecordService;
        private readonly BusBloodPressureRecordService _bloodPressureRecordService;
        private readonly ICurrentUserService _currentUserService;
        private readonly BusPregnantWomanEventRecordService _pregnantWomanEventRecordService;
        private readonly IMapper _mapper;


        /// <summary>
        /// 
        /// </summary>
        public PregnantWomanEatMedicineRecordController(ILogger<RemoteCommandController> logger,
            MainDbContext myDbContext,
            BusPregnantWomanEatMedicineRecordService pregnantWomanEatMedicineRecordService,
            BusBloodPressureRecordService bloodPressureRecordService,
            ICurrentUserService currentUserService,
            BusPregnantWomanEventRecordService pregnantWomanEventRecordService,
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = myDbContext;
            _pregnantWomanEatMedicineRecordService = pregnantWomanEatMedicineRecordService;
            _bloodPressureRecordService = bloodPressureRecordService;
            _currentUserService = currentUserService;
            _pregnantWomanEventRecordService = pregnantWomanEventRecordService;
            _mapper = mapper;
        }

        /// <summary>
        /// 提交围产期监测数据
        /// </summary>
        /// <param name="pregnantWomanEventType">事件类型(1吃奶粉,2吃母乳,3小便,4大便,5开奶粉,6开尿不湿,7吃维D,8吃维AD)</param>
        /// <param name="value">事件值(可选)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> SubmitWomanEventRecord(PregnantWomanEventType pregnantWomanEventType, string value)
        {
            await _pregnantWomanEventRecordService.InsertAsync(new BusPregnantWomanEventRecord() { PregnantWomanEventType = pregnantWomanEventType, Value = value });
            return "围产期监测数据提交成功!";

        }

        /// <summary>
        /// 查询围产期监测数据
        /// </summary>
        /// <param name="pregnantWomanEventType">事件类型(1吃奶粉,2吃母乳,3小便,4大便)</param>
        /// <param name="value">事件值(可选)</param>
        /// <param name="count">查询条数(可选,默认10条)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<List<PregnantWomanEventRecordDto>> SearchTodayWomanEventRecord(PregnantWomanEventType? pregnantWomanEventType, string value, int? count)
        {
            return await _pregnantWomanEventRecordService.Queryable()
                .WhereIf(pregnantWomanEventType.HasValue, c => c.PregnantWomanEventType == pregnantWomanEventType)
                .WhereIf(value.IsNotBlank(), c => c.Value.Contains(value)).OrderByDescending(c => c.CreateTime)
                .Take(count ?? 10)
                .ProjectTo<PregnantWomanEventRecordDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        /// <summary>
        /// 提交吃药结果
        /// </summary>
        /// <param name="IsEatSuccess">是否吃完(true-吃了,false-没吃)</param>
        /// <param name="FailRemark">没吃原因说明(没吃的话需要填写)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> SubmitEatMedicineRecord(bool? IsEatSuccess, string FailRemark)
        {
            await _pregnantWomanEatMedicineRecordService.Submit(new SubmitInput() { IsEatSuccess = IsEatSuccess, FailRemark = FailRemark });
            return "吃药记录提交成功,感谢您的努力!";

        }

        /// <summary>
        /// 查询今日吃药记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<List<BusPregnantWomanEatMedicineRecord>> SearchTodayRecord()
        {
            var nowDate = DateTime.Now.Date;
            var records = await _pregnantWomanEatMedicineRecordService.Queryable().Where(c => c.StartTime >= nowDate).ToListAsync();
            return records;
        }


        /// <summary>
        /// 提交血压记录
        /// </summary>
        /// <param name="highPressure">高压(mmHg)</param>
        /// <param name="lowPressure">低压(mmHg)</param>
        /// <param name="pulse">脉搏(次/分)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> SubmitBloodPressureRecord(int highPressure, int lowPressure, int pulse)
        {
            if (highPressure == 0 || lowPressure == 0 || pulse == 0)
            {
                throw new BusinessException("所有数值均为必填,请确认后重新提交!");
            }
            BloodPressureResult bloodPressureResult;
            BloodPressureTimeFrame bloodPressureTimeFrame;
            if (DateTime.Now < DateTime.Today.AddHours(12))//早晨时段
            {
                bloodPressureTimeFrame = BloodPressureTimeFrame.Morning;
            }
            else//晚间时段
            {
                bloodPressureTimeFrame = BloodPressureTimeFrame.Evening;
            }
            string msg = "";
            if (highPressure < 135 && lowPressure < 85)//通过
            {
                bloodPressureResult = BloodPressureResult.Pass;
                msg = "本次结果全部正常!";
            }
            else if (highPressure < 140 && lowPressure < 90)//低风险
            {
                bloodPressureResult = BloodPressureResult.LowRisk;
                if (highPressure >= 135)
                {
                    msg += $"本次结果高压值异常,实测值:{highPressure},参考值:<135(国标)<140(鼓医),";
                }
                if (lowPressure >= 85)
                {
                    msg += $"本次结果低压值异常,实测值:{lowPressure},参考值:<85(国标)<90(鼓医),";
                }
                msg += "低风险预警,请保持关注!";

            }
            else//高风险
            {
                bloodPressureResult = BloodPressureResult.HighRisk;
                if (highPressure >= 140)
                {
                    msg += $"本次结果高压值异常,实测值:{highPressure},参考值:<135(国标)<140(鼓医),";
                }
                if (lowPressure >= 90)
                {
                    msg += $"本次结果低压值异常,实测值:{lowPressure},参考值:<85(国标)<90(鼓医),";
                }
                msg += "高风险预警,请立即就医!";
            }
            var record = new BusBloodPressureRecord()
            {
                HighPressure = highPressure,
                LowPressure = lowPressure,
                Pulse = pulse,
                BloodPressureResult = bloodPressureResult,
                BloodPressureTimeFrame = bloodPressureTimeFrame,
            };
            await _bloodPressureRecordService.InsertAsync(record);

            return $"[{record.BloodPressureResult.GetDescription()}]血压监测记录提交成功,{msg}";
        }


        /// <summary>
        /// 查询近7日血压记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<List<BusBloodPressureRecord>> SearchBloodPressureRecord(int day = 7)
        {
            var stratTime = DateTime.Now.Date.AddDays(-day);
            var records = await _bloodPressureRecordService.Queryable().Where(c => c.CreateTime >= stratTime).ToListAsync();
            return records;
        }



    }
}





