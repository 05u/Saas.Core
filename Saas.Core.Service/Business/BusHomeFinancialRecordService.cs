using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Utilities;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 家庭财务台账
    /// </summary>
    public class BusHomeFinancialRecordService : Repository<BusHomeFinancialRecord>
    {
        private readonly ILogger<BusHomeFinancialRecordService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        public BusHomeFinancialRecordService(MainDbContext context, ILogger<BusHomeFinancialRecordService> logger, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _logger = logger;
        }
    }
}
