using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 微信用户
    /// </summary>
    [Table("bus_weixin_user")]
    public class BusWeixinUser : BaseEntity
    {
        /// <summary>
        /// 微信Id
        /// </summary>
        public string Wxid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }


    }
}
