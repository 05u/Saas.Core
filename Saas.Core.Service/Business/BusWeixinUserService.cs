using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Utilities;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 微信用户
    /// </summary>
    public class BusWeixinUserService : Repository<BusWeixinUser>
    {
        private readonly ILogger<BusWeixinUserService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        public BusWeixinUserService(MainDbContext context, ILogger<BusWeixinUserService> logger, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _logger = logger;

        }
    }
}
