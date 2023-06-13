using System.ComponentModel;

namespace Saas.Core.Infrastructure.Enums
{
    /// <summary>
    /// 接口类型
    /// </summary>
    public enum InterfaceType
    {
        /// <summary>
        /// GET
        /// </summary>
        [Description("GET")]
        GET = 0,

        /// <summary>
        /// POST
        /// </summary>
        [Description("POST")]
        POST = 1,
    }
}
