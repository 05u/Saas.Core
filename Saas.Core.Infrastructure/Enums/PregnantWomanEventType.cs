using System.ComponentModel;

namespace Saas.Core.Infrastructure.Enums
{
    /// <summary>
    /// 孕产妇事件类型
    /// </summary>
    public enum PregnantWomanEventType
    {
        /// <summary>
        /// 吃奶粉
        /// </summary>
        [Description("吃奶粉")]
        EatMilkPowder = 1,

        /// <summary>
        /// 吃母乳
        /// </summary>
        [Description("吃母乳")]
        EatMotherMilk = 2,

        /// <summary>
        /// 小便
        /// </summary>
        [Description("小便")]
        ToUrinate = 3,

        /// <summary>
        /// 大便
        /// </summary>
        [Description("大便")]
        ToMoveBowels = 4,

        /// <summary>
        /// 开奶粉
        /// </summary>
        [Description("开奶粉")]
        OpenMilkPowder = 5,

        /// <summary>
        /// 开尿不湿
        /// </summary>
        [Description("开尿不湿")]
        Opendiapers = 6,

        /// <summary>
        /// 吃维D
        /// </summary>
        [Description("吃维D")]
        EatVD = 7,

        /// <summary>
        /// 吃维AD
        /// </summary>
        [Description("吃维AD")]
        EatVAD = 8,
    }
}
