using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 客户端表
    /// </summary>
    [Table("sys_client")]
    public class SysClient : BaseEntity
    {

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 客户端appKey
        /// </summary>
        public string ClientKey { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }


}
