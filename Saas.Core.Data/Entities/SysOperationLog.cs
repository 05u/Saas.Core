using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 操作日志表
    /// </summary>
    [Table("sys_operation_log")]
    public class SysOperationLog : BaseEntity
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType OperationType { get; set; }

        /// <summary>
        /// 请求人IP
        /// </summary>
        public string RequestIp { get; set; }

        /// <summary>
        /// 请求人地理位置
        /// </summary>
        public string RequestAddress { get; set; }
    }
}
