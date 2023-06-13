using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// ChatGPT上下文管理
    /// </summary>
    public class ChatGptContextController : BaseApiController
    {
        private readonly BusChatGptContextService _service;

        /// <summary>
        /// ctor
        /// </summary>
        public ChatGptContextController(BusChatGptContextService ChatGptContextService)
        {
            _service = ChatGptContextService;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<BusChatGptContext>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
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
        public async Task<string> Create([FromBody] BusChatGptContext dto)
        {
            if (dto.Identification.IsBlank())
            {
                throw new BusinessException("必填");
            }
            if (await _service.ExistsAsync(x => x.Identification == dto.Identification))
            {
                throw new BusinessException("重复");
            }
            var Id = await _service.InsertAsync(dto);
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<BusChatGptContext> Get([FromRoute] string id)
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
        public async Task<bool> Edit([FromBody] BusChatGptContext dto)
        {
            if (dto.Identification.IsBlank())
            {
                throw new BusinessException("必填");
            }
            if (await _service.ExistsAsync(x => x.Identification == dto.Identification && x.Id != dto.Id))
            {
                throw new BusinessException("重复");
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


        /// <summary>
        /// 上下文可用次数充值
        /// </summary>
        /// <param name="identification">用户标识</param>
        /// <param name="count">充值次数</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> AddCount(string identification, int count)
        {
            var dto = await _service.Queryable().Where(c => c.Identification == identification).FirstOrDefaultAsync();
            if (dto == null)
            {
                return "未查询到该用户";
            }
            dto.AvailableCount = dto.AvailableCount + count;
            await _service.UpdateAsync(dto);
            return $"充值成功,本次增加{count}次,总可用{dto.AvailableCount}次";
        }

    }
}
