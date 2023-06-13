using System.ComponentModel;

namespace Saas.Core.Infrastructure.Enums
{
    /// <summary>
    /// 班次类型
    /// </summary>
    public enum WorkClassType
    {
        /// <summary>
        /// 大夜班
        /// </summary>
        [Description("大夜班")]
        Dayeban = 1,

        /// <summary>
        /// 白班
        /// </summary>
        [Description("白班")]
        Baiban = 2,

        /// <summary>
        /// 小夜班
        /// </summary>
        [Description("小夜班")]
        Xiaoyeban = 4,
    }
}
