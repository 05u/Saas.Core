using Microsoft.EntityFrameworkCore;
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
    /// ChatGPT上下文管理
    /// </summary>
    public class BusChatGptContextService : Repository<BusChatGptContext>
    {
        private readonly ILogger<BusChatGptContextService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        public BusChatGptContextService(MainDbContext context, ILogger<BusChatGptContextService> logger, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _logger = logger;
        }

        /// <summary>
        /// 重置可用次数
        /// </summary>
        /// <returns></returns>
        public async Task ResetAvailableCount()
        {
            //不足20次的补齐
            var list = await Queryable().Where(c => c.AvailableCount < 20).ToListAsync();
            foreach (var item in list)
            {
                item.AvailableCount = 20;
            }
            await BatchUpdateAsync(list);
        }
    }
}
