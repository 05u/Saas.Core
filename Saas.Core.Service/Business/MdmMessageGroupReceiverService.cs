using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Utilities;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 消息群组与接收者关联
    /// </summary>
    public class MdmMessageGroupReceiverService : Repository<MdmMessageGroupReceiver>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public MdmMessageGroupReceiverService(MainDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }
    }
}
