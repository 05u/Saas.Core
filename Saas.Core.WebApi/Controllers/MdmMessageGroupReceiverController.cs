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
    /// 消息群组与接收者关联
    /// </summary>
    public class MdmMessageGroupReceiverController : BaseApiController
    {
        private readonly MdmMessageGroupReceiverService _service;

        /// <summary>
        /// ctor
        /// </summary>
        public MdmMessageGroupReceiverController(MdmMessageGroupReceiverService mdmMessageGroupService)
        {
            _service = mdmMessageGroupService;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<MdmMessageGroupReceiver>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
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
        public async Task<string> Create([FromBody] MdmMessageGroupReceiver dto)
        {
            if (dto.MessageGroupId.IsBlank() || dto.MessageReceiverId.IsBlank())
            {
                throw new BusinessException("群组Id和接收者Id必填");
            }
            if (await _service.ExistsAsync(x => x.MessageGroupId == dto.MessageGroupId && x.MessageReceiverId == dto.MessageReceiverId))
            {
                throw new BusinessException("已存在关联关系,无法重复添加");
            }
            var Id = await _service.InsertAsync(dto);
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<MdmMessageGroupReceiver> Get([FromRoute] string id)
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
        public async Task<bool> Edit([FromBody] MdmMessageGroupReceiver dto)
        {
            if (await _service.ExistsAsync(x => x.MessageGroupId == dto.MessageGroupId && x.MessageReceiverId == dto.MessageReceiverId))
            {
                throw new BusinessException("已存在关联关系,无法重复添加");
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
