using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Utilities;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 消息接收者
    /// </summary>
    public class MdmMessageReceiverService : Repository<MdmMessageReceiver>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public MdmMessageReceiverService(MainDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }
    }
}
