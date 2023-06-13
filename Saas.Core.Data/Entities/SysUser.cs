using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 系统用户表
    /// </summary>
    [Table("sys_user")]
    public class SysUser : BaseEntity
    {

        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string AvaterPath { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 身份
        /// </summary>
        public string Sub { get; set; }


    }


}
