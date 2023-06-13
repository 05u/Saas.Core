using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 图书订阅
    /// </summary>
    [Table("bus_book_subscription")]
    public class BusBookSubscription : BaseEntity
    {
        /// <summary>
        /// 图书名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 订阅人类型
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// 订阅人 第三方平台标识(如QQ号,wxid等)
        /// </summary>
        public string Identification { get; set; }

        /// <summary>
        /// 是否有库存
        /// </summary>

        public bool IsInStock { get; set; }
    }
}
