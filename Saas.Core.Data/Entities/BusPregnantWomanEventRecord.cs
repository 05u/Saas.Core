using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{

    /// <summary>
    /// 孕妇事件记录
    /// </summary>
    [Table("bus_pregnant_woman_event_record")]
    public class BusPregnantWomanEventRecord : BaseEntity
    {

        /// <summary>
        /// 孕妇事件类型
        /// </summary>
        public PregnantWomanEventType PregnantWomanEventType { get; set; }

        /// <summary>
        /// 记录值
        /// </summary>
        public string Value { get; set; }
    }
}
