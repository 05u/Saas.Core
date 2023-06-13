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
    /// 家庭人员
    /// </summary>
    public class HomePersionController : BaseApiController
    {
        private readonly MdmHomePersionService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        public HomePersionController(MdmHomePersionService HomePersionService, IMapper mapper)
        {
            _service = HomePersionService;
            _mapper = mapper;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<HomePersionDto>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
        {
            var list = await _service.Queryable().ProjectTo<HomePersionDto>(_mapper.ConfigurationProvider).PagingResultAsync(filter.PageIndex, filter.PageSize, filter.SortField, filter.SortType);
            return list;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Create([FromBody] HomePersionDto dto)
        {
            if (dto.Name.IsBlank())
            {
                throw new BusinessException("人员姓名必填");
            }
            if (dto.Mac.IsBlank())
            {
                throw new BusinessException("手机MAC地址必填");
            }
            if (await _service.ExistsAsync(x => x.Name == dto.Name))
            {
                throw new BusinessException("人员姓名重复");
            }
            if (await _service.ExistsAsync(x => x.Mac == dto.Mac))
            {
                throw new BusinessException("手机MAC地址重复");
            }
            if (dto.JudgmentThreshold == null)
            {
                dto.JudgmentThreshold = 10;
            }
            var Id = await _service.InsertAsync(_mapper.Map<HomePersionDto, MdmHomePersion>(dto));
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<MdmHomePersion> Get([FromRoute] string id)
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
        public async Task<bool> Edit([FromBody] HomePersionDto dto)
        {
            if (dto.Name.IsBlank())
            {
                throw new BusinessException("人员姓名必填");
            }
            if (dto.Mac.IsBlank())
            {
                throw new BusinessException("手机MAC地址必填");
            }
            if (await _service.ExistsAsync(x => x.Name == dto.Name && x.Id != dto.Id))
            {
                throw new BusinessException("人员姓名重复");
            }
            if (await _service.ExistsAsync(x => x.Mac == dto.Mac && x.Id != dto.Id))
            {
                throw new BusinessException("手机MAC地址重复");
            }
            await _service.UpdateAsync(_mapper.Map<HomePersionDto, MdmHomePersion>(dto));
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
