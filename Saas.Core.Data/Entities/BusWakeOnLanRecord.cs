using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 网络唤醒记录
    /// </summary>
    [Table("bus_wake_on_lan_record")]
    public class BusWakeOnLanRecord : BaseEntity
    {
        /// <summary>
        /// 唤醒目标MAC
        /// </summary>
        public string Mac { get; set; }


        /// <summary>
        /// 唤醒说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 请求人IP
        /// </summary>
        public string SenderIP { get; set; }
    }
}
