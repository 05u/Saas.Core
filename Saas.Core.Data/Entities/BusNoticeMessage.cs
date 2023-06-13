using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 通知消息记录表
    /// </summary>
    [Table("bus_notice_message")]
    public class BusNoticeMessage : BaseEntity
    {


        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgText { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? SendTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [NotMapped]
        public string MessageTypeText => MessageType.GetDescription();

        /// <summary>
        /// 是否发送成功
        /// </summary>
        public bool? IsSendSuccess { get; set; }

        /// <summary>
        /// 发送失败原因
        /// </summary>
        public string FailureReasons { get; set; }


    }
}
