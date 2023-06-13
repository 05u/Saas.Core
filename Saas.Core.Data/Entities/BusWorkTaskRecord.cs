using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 工作任务记录
    /// </summary>
    [Table("bus_work_task_record")]
    public class BusWorkTaskRecord : BaseEntity
    {
        /// <summary>
        /// 微信Id
        /// </summary>
        public string Wxid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上班日期
        /// </summary>
        public DateTime WorkDate { get; set; }

        /// <summary>
        /// 班次类型
        /// </summary>
        public WorkClassType WorkClassType { get; set; }

        /// <summary>
        /// 计划提醒时间
        /// </summary>
        public DateTime PlanNoticeTime { get; set; }

        /// <summary>
        /// 是否已经提醒
        /// </summary>
        public bool IsNotice { get; set; }
    }
}
