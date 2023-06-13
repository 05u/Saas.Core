using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 消息群组
    /// </summary>
    [Table("mdm_message_group")]
    public class MdmMessageGroup : BaseEntity
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
        /// 允许快捷加入
        /// </summary>
        public bool AllowQuickJoin { get; set; }


        #region 导航属性

        /// <summary>
        /// 消息群组与接收者关联表
        /// </summary>
        public virtual IList<MdmMessageGroupReceiver> MessageGroupReceivers { get; set; }
        #endregion

    }


}
