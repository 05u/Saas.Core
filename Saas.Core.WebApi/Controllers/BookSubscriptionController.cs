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
    /// 图书订阅
    /// </summary>
    public class BookSubscriptionController : BaseApiController
    {
        private readonly BusBookSubscriptionService _service;

        /// <summary>
        /// ctor
        /// </summary>
        public BookSubscriptionController(BusBookSubscriptionService bookSubscriptionService)
        {
            _service = bookSubscriptionService;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<BusBookSubscription>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
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
        public async Task<string> Create([FromBody] BusBookSubscription dto)
        {
            return await _service.CreateBookSubscription(dto.Name, dto.MessageType, dto.Identification);
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<BusBookSubscription> Get([FromRoute] string id)
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
        public async Task<bool> Edit([FromBody] BusBookSubscription dto)
        {
            if (dto.Name.IsBlank() || dto.Identification.IsBlank())
            {
                throw new BusinessException("存在必填项未填");
            }
            if (await _service.ExistsAsync(x => x.Name == dto.Name && x.MessageType == dto.MessageType && x.Identification == dto.Identification && x.Id != dto.Id))
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

        /// <summary>
        /// 查询图书
        /// </summary>
        /// <param name="name">书名</param>
        /// <param name="onlyInCount">只显示有库存</param>
        /// <param name="onlyCanOut">只显示可借出</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<List<StockInfo>> Search(string name, bool? onlyInCount, bool? onlyCanOut)
        {
            var dto = await _service.GetStockList(name);
            if (onlyInCount == true)
            {
                dto = dto.Where(c => c.InCount > 0).ToList();
            }
            if (onlyCanOut == true)
            {
                dto = dto.Where(c => !c.Location.Contains("保存本") && !c.Location.Contains("闭架库")).ToList();
            }
            return dto;
        }

    }
}
