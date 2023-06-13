using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Utilities;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class SysOperationLogService : Repository<SysOperationLog>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public SysOperationLogService(MainDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }
    }
}
