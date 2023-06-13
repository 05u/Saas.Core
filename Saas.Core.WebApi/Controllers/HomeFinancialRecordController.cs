using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    /// 家庭财务台账
    /// </summary>
    public class HomeFinancialRecordController : BaseApiController
    {
        private readonly BusHomeFinancialRecordService _service;
        private readonly BusNoticeMessageService _noticeMessageService;


        /// <summary>
        /// ctor
        /// </summary>
        public HomeFinancialRecordController(BusHomeFinancialRecordService service, BusNoticeMessageService noticeMessageService)
        {
            _service = service;
            _noticeMessageService = noticeMessageService;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<BusHomeFinancialRecord>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
        {
            var list = await _service.Queryable().PagingResultAsync(filter.PageIndex, filter.PageSize, filter.SortField, filter.SortType);
            return list;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Create([FromBody] BusHomeFinancialRecord dto)
        {
            var Id = await _service.InsertAsync(dto);
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<BusHomeFinancialRecord> Get([FromRoute] string id)
        {
            var dto = await _service.FindAsync(id);
            return dto;
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Edit([FromBody] BusHomeFinancialRecord dto)
        {

            await _service.UpdateAsync(dto);
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Delete([FromBody] IList<string> ids)
        {
            await _service.DeleteAsync(ids);
            return true;
        }

        /// <summary>
        /// 添加一条财务记录
        /// </summary>
        /// <param name="financialAccountType">账户类型(1活期,2定期,3外债,4现金)</param>
        /// <param name="money">变动金额</param>
        /// <param name="remark">说明</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> CreateHomeFinancialRecord(FinancialAccountType financialAccountType, double money, string remark)
        {
            var Id = await _service.InsertAsync(new BusHomeFinancialRecord()
            {
                FinancialAccountType = financialAccountType,
                Money = money,
                Remark = remark,
            });
            return Id;
        }

        /// <summary>
        /// 获取汇总分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> GetSummaryStatistics(DateTime? endTime)
        {
            var records = await _service.Queryable().WhereIf(endTime.HasValue, c => c.CreateTime <= endTime).ToListAsync();

            var text = $"家庭总资产{records.Sum(c => c.Money)}({new NumChineseConvert().NumToChinese(records.Sum(c => c.Money).ToString("0"))}){Environment.NewLine}其中{Environment.NewLine}定期:{records.Where(c => c.FinancialAccountType == FinancialAccountType.Fixed).Sum(c => c.Money)}{Environment.NewLine}活期:{records.Where(c => c.FinancialAccountType == FinancialAccountType.Current).Sum(c => c.Money)}{Environment.NewLine}外债:{records.Where(c => c.FinancialAccountType == FinancialAccountType.Debt).Sum(c => c.Money)}{Environment.NewLine}现金:{records.Where(c => c.FinancialAccountType == FinancialAccountType.Cash).Sum(c => c.Money)}";
            await _noticeMessageService.PublishNoticeMessageToGroup("个人组", text, false);
            return "请查收IM消息";
        }

    }
}
