using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 菜单查询入参
    /// </summary>
    public class SysMenuFilter : BaseFilter
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单编码
        /// </summary>
        public string Code { get; set; }
    }
}
