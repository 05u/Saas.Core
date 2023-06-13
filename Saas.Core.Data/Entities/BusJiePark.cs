using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 捷停车
    /// </summary>
    [Table("bus_jie_park")]
    public class BusJiePark : BaseEntity
    {
        /// <summary>
        /// 停车场代码
        /// </summary>
        public string ParkCode { get; set; }

        /// <summary>
        /// 捷停车用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 捷停车手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 接受通知消息的群组Id
        /// </summary>
        public string MessageGroupId { get; set; }


        #region 导航属性
        /// <summary>
        /// 
        /// </summary>
        public virtual MdmMessageGroup MessageGroup { get; set; }
        #endregion
    }
}
