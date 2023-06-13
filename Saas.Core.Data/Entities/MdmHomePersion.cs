using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 家庭人员
    /// </summary>
    [Table("mdm_home_persion")]
    public class MdmHomePersion : BaseEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机MAC地址
        /// </summary>
        public string Mac { get; set; }

        /// <summary>
        /// 未检测到在家的次数
        /// </summary>
        public int NotCheckedNumber { get; set; }

        /// <summary>
        /// 判定阈值(超过此次数不在家,判定为真不在家)
        /// </summary>
        public int JudgmentThreshold { get; set; }

        /// <summary>
        /// 是否在家
        /// </summary>
        public bool IsAtHome { get; set; }
    }
}
