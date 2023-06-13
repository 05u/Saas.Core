using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Enums;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 接口监控Dto
    /// </summary>
    public class InterfaceMonitorDto : BaseEntity
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 接口类型
        /// </summary>
        public InterfaceType InterfaceType { get; set; }

        /// <summary>
        /// 监控周期(分钟)
        /// </summary>
        public int Cycle { get; set; }

        /// <summary>
        /// 下次监控日期
        /// </summary>
        public DateTime? NextTime { get; set; }

        /// <summary>
        /// 消息群组Id
        /// </summary>
        public string MdmMessageGroupId { get; set; }
    }
}
