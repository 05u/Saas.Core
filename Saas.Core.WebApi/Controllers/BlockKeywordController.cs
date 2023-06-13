using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 封锁关键词
    /// </summary>
    public class BlockKeywordController : BaseApiController
    {
        private readonly BusBlockKeywordService _service;

        /// <summary>
        /// ctor
        /// </summary>
        public BlockKeywordController(BusBlockKeywordService BlockKeywordService)
        {
            _service = BlockKeywordService;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<BusBlockKeyword>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
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
        public async Task<string> Create([FromBody] BusBlockKeyword dto)
        {
            if (dto.Value.IsBlank())
            {
                throw new BusinessException("关键词值必填");
            }
            if (await _service.ExistsAsync(x => x.Value == dto.Value))
            {
                throw new BusinessException("关键词值重复");
            }
            var Id = await _service.InsertAsync(dto);
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<BusBlockKeyword> Get([FromRoute] string id)
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
        public async Task<bool> Edit([FromBody] BusBlockKeyword dto)
        {
            if (dto.Value.IsBlank())
            {
                throw new BusinessException("关键词值必填");
            }
            if (await _service.ExistsAsync(x => x.Value == dto.Value && x.Id != dto.Id))
            {
                throw new BusinessException("关键词值重复");
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
            await _service.DeleteAsync(ids);
            return true;
        }

    }
}
