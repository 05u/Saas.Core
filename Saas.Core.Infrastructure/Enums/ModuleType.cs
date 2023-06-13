using System.ComponentModel;

namespace Saas.Core.Infrastructure.Enums
{
    /// <summary>
    /// 
    /// </summary>
    [Description("上传模块类别")]
    public enum ModuleType
    {
        /// <summary>
        /// 小爱设备
        /// </summary>
        [Description("小爱设备")]
        MdmXiaoaiSpeaker = 1,

        /// <summary>
        /// 工作任务记录
        /// </summary>
        [Description("工作任务记录")]
        WorkTaskRecord = 2,

        /// <summary>
        /// 慧明安全标准化导入
        /// </summary>
        [Description("慧明安全标准化导入")]
        SafetyStandard = 3,
    }
}
