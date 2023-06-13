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
    /// 家庭人员
    /// </summary>
    public class MdmHomePersionService : Repository<MdmHomePersion>
    {
        private readonly ILogger<MdmHomePersionService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        public MdmHomePersionService(MainDbContext context, ILogger<MdmHomePersionService> logger, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _logger = logger;
        }
    }
}
