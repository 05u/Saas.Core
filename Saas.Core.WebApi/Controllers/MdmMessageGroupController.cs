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
    /// 消息群组
    /// </summary>
    public class MdmMessageGroupController : BaseApiController
    {
        private readonly MdmMessageGroupService _service;

        /// <summary>
        /// ctor
        /// </summary>
        public MdmMessageGroupController(MdmMessageGroupService mdmMessageGroupService)
        {
            _service = mdmMessageGroupService;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<MdmMessageGroup>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
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
        public async Task<string> Create([FromBody] MdmMessageGroup dto)
        {
            if (dto.Name.IsBlank())
            {
                throw new BusinessException("群组名称必填");
            }
            if (await _service.ExistsAsync(x => x.Name == dto.Name))
            {
                throw new BusinessException("名称重复");
            }
            var Id = await _service.InsertAsync(dto);
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<MdmMessageGroup> Get([FromRoute] string id)
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
        public async Task<bool> Edit([FromBody] MdmMessageGroup dto)
        {
            if (await _service.ExistsAsync(x => x.Name == dto.Name && x.Id != dto.Id))
            {
                throw new BusinessException("名称重复");
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
