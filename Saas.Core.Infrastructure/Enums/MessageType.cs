using System.ComponentModel;

namespace Saas.Core.Infrastructure.Enums
{
    /// <summary>
    /// 发送消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 消息网关
        /// </summary>
        [Description("消息网关")]
        MessageGateway = 0,

        /// <summary>
        /// 电脑操作(开机,解锁,锁定)
        /// </summary>
        [Description("电脑操作(开机,解锁,锁定)")]
        ComputerOperation = 1,

        /// <summary>
        /// 小爱TTS
        /// </summary>
        [Description("小爱TTS")]
        XiaoaiTTS = 2,

        /// <summary>
        /// QQ
        /// </summary>
        [Description("QQ")]
        QQ = 3,

        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        Weixin = 4,

        /// <summary>
        /// 微信群
        /// </summary>
        [Description("微信群")]
        WeixinGroup = 5,

        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
        Exception = 999,

    }
}
