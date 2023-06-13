using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 远程指令记录表
    /// </summary>
    [Table("bus_remote_command")]
    public class BusRemoteCommand : BaseEntity
    {

        /// <summary>
        /// 是否重启
        /// </summary>
        public bool? RebootAction { get; set; }
        /// <summary>
        /// 是否关机
        /// </summary>
        public bool? ShutdownAction { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 客户端Mac地址
        /// </summary>
        public string ClientMac { get; set; }

        /// <summary>
        /// 客户端是否在线
        /// </summary>
        [NotMapped]
        public bool IsOnline => (DateTime.Now - (DateTime)LastHeartTime).TotalSeconds <= (HeartbeatCycle < 60 ? 60 : HeartbeatCycle) ? true : false;

        /// <summary>
        /// 上次心跳时间
        /// </summary>
        public DateTime? LastHeartTime { get; set; }

        /// <summary>
        /// 上次重启时间
        /// </summary>
        public DateTime? LastRebootTime { get; set; }

        /// <summary>
        /// 上次关机时间
        /// </summary>
        public DateTime? LastShutdownTime { get; set; }

        /// <summary>
        /// 心跳周期 单位:秒
        /// </summary>
        public int HeartbeatCycle { get; set; }

        /// <summary>
        /// 是否已触发监控报警
        /// </summary>
        public bool IsMonitoringAlarm { get; set; }

        /// <summary>
        /// 远程指令类型
        /// </summary>
        public ActionType ActionType { get; set; }
    }
}
