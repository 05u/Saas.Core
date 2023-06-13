using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 消息群组与接收者关联表
    /// </summary>
    [Table("mdm_message_group_receiver")]
    public class MdmMessageGroupReceiver : BaseEntity
    {



        /// <summary>
        /// 消息群组Id
        /// </summary>
        public string MessageGroupId { get; set; }

        /// <summary>
        /// 消息接收者Id
        /// </summary>
        public string MessageReceiverId { get; set; }


        #region 导航属性
        /// <summary>
        /// 消息群组
        /// </summary>
        public virtual MdmMessageGroup MessageGroup { get; set; }

        /// <summary>
        /// 消息接收者
        /// </summary>
        public virtual MdmMessageReceiver MessageReceiver { get; set; }
        #endregion




    }


}
