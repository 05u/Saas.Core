using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;

using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 家庭网络
    /// </summary>
    public class HomeNetworkController : BaseApiController
    {

        private readonly ILogger<XiaoaiController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SysClientService _sysClientService;
        private IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration Configuration;
        private readonly SysOperationLogService _operationLogService;
        private readonly BusNoticeMessageService _noticeMessageService;
        private readonly HomeNetworkService _homeNetworkService;

        /// <summary>
        /// 
        /// </summary>
        public HomeNetworkController(ILogger<XiaoaiController> logger, IHttpContextAccessor httpContextAccessor, SysClientService sysClientService, IHttpClientFactory httpClientFactory, IConfiguration configuration, SysOperationLogService operationLogService, BusNoticeMessageService noticeMessageService, HomeNetworkService homeNetworkService)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _sysClientService = sysClientService;
            _httpClientFactory = httpClientFactory;
            Configuration = configuration;
            _operationLogService = operationLogService;
            _noticeMessageService = noticeMessageService;
            _homeNetworkService = homeNetworkService;
        }

        /// <summary>
        /// 查询路由器网关列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<List<Vlan_data>> GetPPPoEGatewayList()
        {
            return await _homeNetworkService.GetPPPoEGatewayList();
        }

        /// <summary>
        /// 路由器重新拨号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> RouterPPPoERedial()
        {
            var result = await _homeNetworkService.RouterPPPoERedial();
            var gatewayCount = result.Where(c => c.Pppoe_gateway.IsNotBlank()).Select(c => c.Pppoe_gateway).Distinct().Count();
            return new
            {
                Message = gatewayCount > 1 ? $"成功,网关数量{gatewayCount}" : $"失败,网关数量{gatewayCount}",
                GatewayList = result,
            };
        }

        /// <summary>
        /// 获取网络加速服务器列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ContentResult> GetServerList(string key = null)
        {
            Request.Headers.TryGetValue("User-Agent", out var userAgent);
            if (key.IsNotBlank()) { userAgent = key; }

            var dbClient = await _sysClientService.Queryable().Where(c => userAgent.Contains(c.ClientKey)).FirstOrDefaultAsync();
            if (dbClient == null)
            {
                throw new BusinessException("很抱歉，您没有操作权限!");
            }

            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            var requestIp = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress.ToString();
            var areaResponse = await client.GetAsync($"http://whois.pconline.com.cn/ipJson.jsp?ip={requestIp}&json=true");
            var area = await areaResponse.Content.ReadAsStringAsync();
            _logger.LogInformation($"IP地址:{requestIp},接口返回:{area}");
            if (!area.IsContainsAny(new string[] { "局域网", "江苏", "安徽" }))
            {
                throw new BusinessException("很抱歉，您没有操作权限!");
            }

            //记录本次请求日志
            await _operationLogService.InsertAsync(new SysOperationLog() { OperationType = OperationType.GetServerList, RequestIp = requestIp, RequestAddress = area });
            //推送提醒给管理员
            await _noticeMessageService.PublishNoticeMessageToGroup("个人组", $"{dbClient.ClientId}于{DateTime.Now}更新了订阅,备注:{dbClient.Remark ?? "无"}", false);

            var serversResponse = await client.GetAsync(Configuration.GetSection("UrlConfig:DingyueServers").Value);
            var servers = await serversResponse.Content.ReadAsStringAsync();
            return new ContentResult() { Content = servers };

        }

        /// <summary>
        /// 获取网络加速白名单配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ContentResult> GetWhiteList()
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            var response = await client.GetAsync(Configuration.GetSection("UrlConfig:DingyueWhitelist").Value);
            var servers = await response.Content.ReadAsStringAsync();
            return new ContentResult() { Content = servers };

        }


        /// <summary>
        /// 查询在家的家庭成员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<string>> GetPensionAtHome()
        {
            return await _homeNetworkService.GetPensionAtHome();
        }

    }
}





