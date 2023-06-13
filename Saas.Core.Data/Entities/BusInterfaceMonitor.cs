using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 接口监控
    /// </summary>
    [Table("bus_interface_monitor")]
    public class BusInterfaceMonitor : BaseEntity
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 接口类型
        /// </summary>
        public InterfaceType InterfaceType { get; set; }

        /// <summary>
        /// 监控周期(分钟)
        /// </summary>
        public int Cycle { get; set; }

        /// <summary>
        /// 下次监控日期
        /// </summary>
        public DateTime? NextTime { get; set; }

        /// <summary>
        /// 消息群组Id
        /// </summary>
        public string MdmMessageGroupId { get; set; }

        /// <summary>
        /// 是否已触发监控报警
        /// </summary>
        public bool IsMonitoringAlarm { get; set; }


        #region 导航属性
        /// <summary>
        /// 消息群组
        /// </summary>
        public virtual MdmMessageGroup MdmMessageGroup { get; set; }
        #endregion
    }
}
