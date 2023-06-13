using Saas.Core.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 通知提醒任务
    /// </summary>
    [Table("bus_notice_task")]
    public class BusNoticeTask : BaseEntity
    {
        /// <summary>
        /// 通知名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        public NoticeTaskType NoticeTaskType { get; set; }

        /// <summary>
        /// 下次通知时间
        /// </summary>
        public DateTime? NextTime { get; set; }

        /// <summary>
        /// 消息接收者Id
        /// </summary>
        public string MessageReceiverId { get; set; }

        #region 导航属性

        /// <summary>
        /// 消息接收者
        /// </summary>
        public virtual MdmMessageReceiver MessageReceiver { get; set; }
        #endregion
    }
}
