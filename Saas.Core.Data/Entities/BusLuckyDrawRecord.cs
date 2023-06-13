using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 幸运抽奖记录
    /// </summary>
    [Table("bus_lucky_draw_record")]
    public class BusLuckyDrawRecord : BaseEntity
    {
        /// <summary>
        /// 抽奖编号
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 抽奖项目Id
        /// </summary>
        public string LuckyDrawId { get; set; }

        /// <summary>
        /// 联系方式Id
        /// </summary>
        public string MessageReceiverId { get; set; }

        /// <summary>
        /// 群Id(如果发自群)
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 是否中奖
        /// </summary>
        public bool IsWin { get; set; }
        #region 导航属性
        /// <summary>
        /// 抽奖项目
        /// </summary>
        public virtual BusLuckyDraw LuckyDraw { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public virtual MdmMessageReceiver MessageReceiver { get; set; }
        #endregion

    }
}
