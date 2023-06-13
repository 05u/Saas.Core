using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Base
{
    /// <summary>
    /// 消息队列消费
    /// </summary>
    public class RabbitMqService : ICapSubscribe
    {
        private IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration Configuration;
        private readonly MdmMessageGroupService _mdmMessageGroupService;


        /// <summary>
        /// ctor
        /// </summary>
        public RabbitMqService(IHttpClientFactory httpClientFactory, IConfiguration configuration, MdmMessageGroupService mdmMessageGroupService)
        {
            _httpClientFactory = httpClientFactory;
            Configuration = configuration;
            _mdmMessageGroupService = mdmMessageGroupService;
        }

        /// <summary>
        /// 测试订阅
        /// </summary>
        [CapSubscribe(SystemConst.NoticeMessage)]
        public void TestReceive(List<IPInfo> data)
        {

        }
    }
}
