using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Utilities;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class SysClientService : Repository<SysClient>
    {
        private readonly ILogger<SysClientService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        public SysClientService(MainDbContext context, ILogger<SysClientService> logger, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _logger = logger;

        }
    }
}
