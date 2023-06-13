using System.ComponentModel;

namespace Saas.Core.Infrastructure.Enums
{
    /// <summary>
    /// 远程指令类型
    /// </summary>
    public enum ActionType
    {
        [Description("无指令")]
        None = 0,

        [Description("重启")]
        RebootAction = 1,

        [Description("关机")]
        ShutdownAction = 2,

        [Description("锁定")]
        Lock = 3,
    }
}
