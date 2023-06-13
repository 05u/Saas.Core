using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Utilities;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 消息群组
    /// </summary>
    public class MdmMessageGroupService : Repository<MdmMessageGroup>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public MdmMessageGroupService(MainDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }
    }
}
