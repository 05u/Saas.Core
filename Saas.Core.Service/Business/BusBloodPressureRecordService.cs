using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Utilities;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 血压监测记录
    /// </summary>
    public class BusBloodPressureRecordService : Repository<BusBloodPressureRecord>
    {
        private readonly ILogger<BusBloodPressureRecordService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        public BusBloodPressureRecordService(MainDbContext context, ILogger<BusBloodPressureRecordService> logger, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _logger = logger;
        }
    }
}
