using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 消息接收者
    /// </summary>
    [Table("mdm_message_receiver")]
    public class MdmMessageReceiver : BaseEntity
    {

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// 第三方平台标识(如QQ号,wxid等)
        /// </summary>
        public string Identification { get; set; }

        #region 导航属性
        /// <summary>
        /// 消息群组与接收者关联表
        /// </summary>
        public virtual IList<MdmMessageGroupReceiver> MessageGroupReceivers { get; set; }
        #endregion





    }


}
