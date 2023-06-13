using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 捷停车自动领券
    /// </summary>
    public class JieParkController : BaseApiController
    {
        private readonly BusJieParkService _service;

        public JieParkController(BusJieParkService service)
        {
            _service = service;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<BusJiePark>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
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
        public async Task<string> Create([FromBody] BusJiePark dto)
        {
            if (dto.ParkCode.IsBlank())
            {
                throw new BusinessException("停车场代码必填");
            }
            if (dto.UserId.IsBlank())
            {
                throw new BusinessException("捷停车用户主键必填");
            }
            if (dto.Mobile.IsBlank())
            {
                throw new BusinessException("捷停车用户手机必填");
            }
            if (!dto.Mobile.IsPhone())
            {
                throw new BusinessException("手机号不符合规范,请检查");
            }
            if (await _service.ExistsAsync(x => x.ParkCode == dto.ParkCode && x.UserId == dto.UserId && x.Mobile == dto.Mobile))
            {
                throw new BusinessException("记录重复");
            }
            var Id = await _service.InsertAsync(dto);
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<BusJiePark> Get([FromRoute] string id)
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
        public async Task<bool> Edit([FromBody] BusJiePark dto)
        {
            if (dto.ParkCode.IsBlank())
            {
                throw new BusinessException("停车场代码必填");
            }
            if (dto.UserId.IsBlank())
            {
                throw new BusinessException("捷停车用户主键必填");
            }
            if (dto.Mobile.IsBlank())
            {
                throw new BusinessException("捷停车用户手机必填");
            }
            if (!dto.Mobile.IsPhone())
            {
                throw new BusinessException("手机号不符合规范,请检查");
            }
            if (await _service.ExistsAsync(x => x.ParkCode == dto.ParkCode && x.UserId == dto.UserId && x.Mobile == dto.Mobile && x.Id != dto.Id))
            {
                throw new BusinessException("记录重复");
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
        /// 手动执行领券任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> GetParkCoupon()
        {
            return await _service.GetParkCoupon(true);
        }

        /// <summary>
        /// 查询今日是否为工作日
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<bool?> IsWorkday()
        {
            return await _service.IsWorkday();
        }
    }
}
