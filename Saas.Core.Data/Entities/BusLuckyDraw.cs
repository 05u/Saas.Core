using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 幸运抽奖
    /// </summary>
    [Table("bus_lucky_draw")]
    public class BusLuckyDraw : BaseEntity
    {
        /// <summary>
        /// 抽奖活动代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 每人最大抽奖次数
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// 中奖数量
        /// </summary>
        public int WinCount { get; set; }

        /// <summary>
        /// 是否开奖
        /// </summary>
        public bool IsFinish { get; set; }
        #region 导航属性

        /// <summary>
        /// 抽奖记录
        /// </summary>
        public virtual IList<BusLuckyDrawRecord> LuckyDrawRecords { get; set; }
        #endregion
    }
}
