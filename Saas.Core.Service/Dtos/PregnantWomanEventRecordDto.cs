using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 孕妇事件记录Dto
    /// </summary>
    public class PregnantWomanEventRecordDto : BaseEntity
    {
        /// <summary>
        /// 孕妇事件类型
        /// </summary>
        public PregnantWomanEventType PregnantWomanEventType { get; set; }

        /// <summary>
        /// 孕妇事件类型
        /// </summary>
        public string PregnantWomanEventTypeText => PregnantWomanEventType.GetDescription();

        /// <summary>
        /// 记录值
        /// </summary>
        public string Value { get; set; }
    }
}
