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
    /// 封锁关键词
    /// </summary>
    public class BusBlockKeywordService : Repository<BusBlockKeyword>
    {
        private readonly ILogger<BusBlockKeywordService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        public BusBlockKeywordService(MainDbContext context, ILogger<BusBlockKeywordService> logger, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _logger = logger;
        }
    }
}
