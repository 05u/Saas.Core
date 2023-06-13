using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 摄像头
    /// </summary>
    [Table("mdm_camera")]
    public class MdmCamera : BaseEntity
    {
        /// <summary>
        /// 摄像头名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属家庭名称
        /// </summary>
        public string HomeName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// Onvif用户名
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Onvif密码
        /// </summary>
        public string Pass { get; set; }

        /// <summary>
        /// 开启时X坐标
        /// </summary>
        public double OnAreaX { get; set; }

        /// <summary>
        /// 开启时Y坐标
        /// </summary>
        public double OnAreaY { get; set; }

        /// <summary>
        /// 关闭时X坐标
        /// </summary>
        public double OffAreaX { get; set; }

        /// <summary>
        /// 关闭时Y坐标
        /// </summary>
        public double OffAreaY { get; set; }
    }
}
