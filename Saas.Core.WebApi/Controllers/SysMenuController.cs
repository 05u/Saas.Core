using Microsoft.AspNetCore.Mvc;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class SysMenuController : BaseApiController
    {
        private readonly SysMenuService _service;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// ctor
        /// </summary>
        public SysMenuController(SysMenuService SysMenuService, ICurrentUserService currentUserService)
        {
            _service = SysMenuService;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<SysMenu>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
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
        public async Task<string> Create([FromBody] SysMenu dto)
        {
            if (dto.Name.IsBlank())
            {
                throw new BusinessException("菜单名称必填");
            }
            //if (await _service.ExistsAsync(x => x.Name == dto.Name))
            //{
            //    throw new BusinessException("菜单名称重复");
            //}
            var Id = await _service.InsertAsync(dto);
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<SysMenu> Get([FromRoute] string id)
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
        public async Task<bool> Edit([FromBody] SysMenu dto)
        {
            if (dto.Name.IsBlank())
            {
                throw new BusinessException("菜单名称必填");
            }
            //if (await _service.ExistsAsync(x => x.Name == dto.Name && x.Id != dto.Id))
            //{
            //    throw new BusinessException("菜单名称重复");
            //}
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
        /// 获取菜单下拉框树状结构
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<SelectTreeResponseDto>> GetSelectTree()
        {
            var menuList = await _service.GetMenuAsync(null, _currentUserService.IsAdmin());
            var treeList = menuList
                .OrderBy(x => x.ParentId)
                .OrderBy(x => x.Sort)
                .Select(x => new SelectTreeResponseDto
                {
                    Id = x.Id,
                    PId = x.ParentId,
                    Value = x.Id,
                    Label = x.Name
                });
            return treeList;
        }

        /// <summary>
        /// 获取菜单树状结构
        /// </summary>
        [HttpGet]
        public async Task<List<TreeResponseDto>> GetTree()
        {
            var menuList = await _service.GetMenuTreeAsync(_currentUserService.IsAdmin());
            return menuList;
        }

    }
}
