using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Utilities;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 孕妇事件记录
    /// </summary>
    public class BusPregnantWomanEventRecordService : Repository<BusPregnantWomanEventRecord>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public BusPregnantWomanEventRecordService(MainDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }
    }
}
