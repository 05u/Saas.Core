using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 幸运抽奖记录
    /// </summary>
    public class BusLuckyDrawRecordService : Repository<BusLuckyDrawRecord>
    {
        private readonly ILogger<BusLuckyDrawRecordService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        public BusLuckyDrawRecordService(MainDbContext context, ILogger<BusLuckyDrawRecordService> logger, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _logger = logger;
        }
    }
}
