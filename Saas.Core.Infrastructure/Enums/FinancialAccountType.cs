using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Infrastructure.Enums
{
    /// <summary>
    /// 账户类型
    /// </summary>
    [Description("账户类型")]
    public enum FinancialAccountType
    {
        /// <summary>
        /// 活期账户
        /// </summary>
        [Description("活期账户")]
        Current = 1,

        /// <summary>
        /// 定期账户
        /// </summary>
        [Description("定期账户")]
        Fixed = 2,

        /// <summary>
        /// 外债账户
        /// </summary>
        [Description("外债账户")]
        Debt = 3,

        /// <summary>
        /// 现金账户
        /// </summary>
        [Description("现金账户")]
        Cash =4,
    }
}
