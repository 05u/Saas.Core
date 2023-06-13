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
    /// 通知提醒任务
    /// </summary>
    public class NoticeTaskController : BaseApiController
    {
        private readonly BusNoticeTaskService _service;

        /// <summary>
        /// ctor
        /// </summary>
        public NoticeTaskController(BusNoticeTaskService NoticeTaskService)
        {
            _service = NoticeTaskService;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<BusNoticeTask>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
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
        public async Task<string> Create([FromBody] BusNoticeTask dto)
        {
            if (dto.Name.IsBlank())
            {
                throw new BusinessException("通知名称必填");
            }
            var Id = await _service.InsertAsync(dto);
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<BusNoticeTask> Get([FromRoute] string id)
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
        public async Task<bool> Edit([FromBody] BusNoticeTask dto)
        {
            if (dto.Name.IsBlank())
            {
                throw new BusinessException("通知名称必填");
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
        /// 手动触发 发出通知提醒消息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> SendNoticeTask()
        {
            await _service.SendNoticeTask();
            return true;
        }
    }
}
