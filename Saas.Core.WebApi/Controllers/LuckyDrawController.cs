using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 幸运抽奖
    /// </summary>
    public class LuckyDrawController : BaseApiController
    {
        private readonly BusLuckyDrawService _service;
        private readonly BusLuckyDrawRecordService _luckyDrawRecordService;

        /// <summary>
        /// ctor
        /// </summary>
        public LuckyDrawController(BusLuckyDrawService LuckyDrawService, BusLuckyDrawRecordService luckyDrawRecordService)
        {
            _service = LuckyDrawService;
            _luckyDrawRecordService = luckyDrawRecordService;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<BusLuckyDraw>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
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
        public async Task<string> Create([FromBody] BusLuckyDraw dto)
        {
            if (dto.MaxCount < 1)
            {
                throw new BusinessException("抽奖次数至少一次");
            }
            if (dto.WinCount < 1)
            {
                throw new BusinessException("中奖数量至少一次");
            }
            if (dto.StartTime >= dto.EndTime)
            {
                throw new BusinessException("结束时间必须晚于开始时间");
            }
            if (dto.Code.IsBlank())
            {
                throw new BusinessException("抽奖代码必填");
            }
            if (await _service.ExistsAsync(x => x.Code == dto.Code))
            {
                throw new BusinessException("抽奖代码重复");
            }
            var Id = await _service.InsertAsync(dto);
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<BusLuckyDraw> Get([FromRoute] string id)
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
        public async Task<bool> Edit([FromBody] BusLuckyDraw dto)
        {
            if (dto.MaxCount < 1)
            {
                throw new BusinessException("抽奖次数至少一次");
            }
            if (dto.WinCount < 1)
            {
                throw new BusinessException("中奖数量至少一次");
            }
            if (dto.StartTime >= dto.EndTime)
            {
                throw new BusinessException("结束时间必须晚于开始时间");
            }
            if (dto.Code.IsBlank())
            {
                throw new BusinessException("抽奖代码必填");
            }
            if (await _service.ExistsAsync(x => x.Code == dto.Code && x.Id != dto.Id))
            {
                throw new BusinessException("抽奖代码重复");
            }
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
            var recordIds = _luckyDrawRecordService.Queryable().Where(c => ids.Contains(c.LuckyDrawId)).Select(c => c.Id).ToList();
            await _luckyDrawRecordService.DeleteAsync(recordIds);
            await _service.DeleteAsync(ids);
            return true;
        }

    }
}
