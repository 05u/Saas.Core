using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Utilities;
using System.Net;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 接口监控
    /// </summary>
    public class BusInterfaceMonitorService : Repository<BusInterfaceMonitor>
    {
        private IHttpClientFactory _httpClientFactory;
        private readonly BusNoticeMessageService _noticeMessageService;
        private readonly ILogger<BusInterfaceMonitorService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        public BusInterfaceMonitorService(MainDbContext context,
            IHttpClientFactory httpClientFactory,
            BusNoticeMessageService noticeMessageService,
            ILogger<BusInterfaceMonitorService> logger, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _httpClientFactory = httpClientFactory;
            _noticeMessageService = noticeMessageService;
            _logger = logger;
        }

        /// <summary>
        /// 检查接口可用性
        /// </summary>
        /// <returns></returns>
        public async Task CheckInterface()
        {
            var nowTime = DateTime.Now;
            var list = await Queryable().Where(c => c.NextTime == null || c.NextTime <= nowTime).ToListAsync();
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            client.Timeout = TimeSpan.FromSeconds(15);//设置超时
            foreach (var item in list)
            {
                if (item.InterfaceType == InterfaceType.GET)
                {
                    HttpResponseMessage response = null;
                    string errorMsg = null;
                    try
                    {
                        response = await client.GetAsync(item.Url);
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message;
                    }
                    if (response == null)
                    {
                        _logger.LogWarning($"接口监控异常:{errorMsg},接口地址:{item.Url}");
                        if (item.IsMonitoringAlarm == false)
                        {
                            var text = $"检测到接口异常,详情:{errorMsg},请及时处理!{Environment.NewLine}{item.Url}";
                            await _noticeMessageService.PublishNoticeMessageToGroup(null, text, true, item.MdmMessageGroupId);
                        }
                        item.IsMonitoringAlarm = true;
                    }
                    else if (response?.StatusCode != HttpStatusCode.OK)
                    {
                        _logger.LogWarning($"接口监控异常:{response.ToJSON()}");
                        if (item.IsMonitoringAlarm == false)
                        {
                            var text = $"检测到接口异常,详情:{(int)response.StatusCode},{response.StatusCode},请及时处理!{Environment.NewLine}{item.Url}";
                            await _noticeMessageService.PublishNoticeMessageToGroup(null, text, true, item.MdmMessageGroupId);
                        }
                        item.IsMonitoringAlarm = true;
                    }
                    else
                    {
                        _logger.LogInformation($"接口监控正常:{response.ToJSON()}");
                        if (item.IsMonitoringAlarm == true)
                        {
                            var text = $"检测到接口恢复正常,详情:{(int)response.StatusCode},{response.StatusCode}!{Environment.NewLine}{item.Url}";
                            await _noticeMessageService.PublishNoticeMessageToGroup(null, text, true, item.MdmMessageGroupId);
                        }
                        item.IsMonitoringAlarm = false;
                    }
                    item.NextTime = nowTime.AddMinutes(item.Cycle);
                }
            }
            await BatchUpdateAsync(list);
        }
    }
}
