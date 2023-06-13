using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 接口监控
    /// </summary>
    public class InterfaceMonitorController : BaseApiController
    {
        private readonly BusInterfaceMonitorService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        public InterfaceMonitorController(BusInterfaceMonitorService interfaceMonitorService, IMapper mapper)
        {
            _service = interfaceMonitorService;
            _mapper = mapper;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<InterfaceMonitorDto>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
        {
            var list = await _service.Queryable().ProjectTo<InterfaceMonitorDto>(_mapper.ConfigurationProvider).PagingResultAsync(filter.PageIndex, filter.PageSize, filter.SortField, filter.SortType);
            return list;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Create([FromBody] InterfaceMonitorDto dto)
        {
            if (dto.Url.IsBlank())
            {
                throw new BusinessException("接口地址必填");
            }
            if (await _service.ExistsAsync(x => x.Url == dto.Url))
            {
                throw new BusinessException("接口地址重复");
            }
            if (dto.InterfaceType != InterfaceType.GET)
            {
                throw new BusinessException("目前只支持GET类型的接口");
            }
            var Id = await _service.InsertAsync(_mapper.Map<InterfaceMonitorDto, BusInterfaceMonitor>(dto));
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<BusInterfaceMonitor> Get([FromRoute] string id)
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
        public async Task<bool> Edit([FromBody] InterfaceMonitorDto dto)
        {
            if (await _service.ExistsAsync(x => x.Url == dto.Url && x.Id != dto.Id))
            {
                throw new BusinessException("接口地址重复");
            }
            await _service.UpdateAsync(_mapper.Map<InterfaceMonitorDto, BusInterfaceMonitor>(dto));
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
        /// 手动触发一次接口检测
        /// </summary>
        [HttpGet]
        public async Task<string> CheckInterface()
        {
            await _service.CheckInterface();
            return null;
        }

    }
}
