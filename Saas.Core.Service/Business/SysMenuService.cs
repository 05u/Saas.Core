using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class SysMenuService : Repository<SysMenu>
    {
        private readonly ILogger<SysMenuService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        public SysMenuService(MainDbContext context, ILogger<SysMenuService> logger, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _logger = logger;
        }

        /// <summary>
        /// 加载菜单
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="loadSystem">是否加载系统菜单的数据</param>
        /// <returns></returns>
        public async Task<List<SysMenuDto>> GetMenuAsync([FromBody] PageBaseFilter<SysMenuFilter> filter, bool loadSystem)
        {

            Expression<Func<SysMenu, bool>> exp = x => true;
            exp = exp
                .AndIf(!loadSystem, x => !x.IsSystem)
                .AndIf(filter?.Search?.Name.IsNotBlank() == true, x => x.Name.Contains(filter.Search.Name))
                .AndIf(filter?.Search?.Code.IsNotBlank() == true, x => x.Code == filter.Search.Code);


            var menuList = await Queryable().Where(exp).Select(x => new SysMenuDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                Code = x.Code,
                Url = x.Url,
                Icon = x.Icon,
                Sort = x.Sort,
                Remark = x.Remark,
            }).ToListAsync();
            return menuList.ToList();
        }

        /// <summary>
        /// 加载菜单树状结构
        /// </summary>
        /// <param name="loadSystem">是否加载系统菜单</param>
        /// <returns></returns>
        public async Task<List<TreeResponseDto>> GetMenuTreeAsync( bool loadSystem)
        {
            var menuList = await GetMenuAsync(null, loadSystem);
            var menuTree = DoGetChildren(string.Empty, menuList);
            return menuTree;
        }

        /// <summary>
        /// 递归获取菜单树
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <param name="menuList">菜单原始列表</param>
        /// <returns></returns>
        private List<TreeResponseDto> DoGetChildren(string parentId, IEnumerable<SysMenuDto> menuList)
        {
            var children = menuList
                .WhereIf(parentId.IsBlank(), x => x.ParentId.IsBlank())
                .WhereIf(parentId.IsNotBlank(), x => x.ParentId == parentId)
                .OrderBy(x => x.Sort)
                .Select(x => new TreeResponseDto
                {
                    Key = x.Id,
                    Title = x.Name,
                    Value = x.Id,
                    Url = x.Url,
                    Icon = x.Icon
                }).ToList();
            foreach (var tree in children)
            {
                tree.Children = DoGetChildren(tree.Key, menuList);
            }
            return children;
        }
    }
}
