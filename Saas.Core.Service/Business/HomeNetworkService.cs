using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 家庭网络
    /// </summary>
    public class HomeNetworkService
    {
        private readonly ILogger<HomeNetworkService> _logger;
        private IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IRedisStackExchangeService _iRedisStackExchangeService;
        private readonly MdmHomePersionService _homePersionService;
        private readonly BusNoticeMessageService _noticeMessageService;


        /// <summary>
        /// ctor
        /// </summary>
        public HomeNetworkService(ILogger<HomeNetworkService> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, IRedisStackExchangeService iRedisStackExchangeService, MdmHomePersionService homePersionService, BusNoticeMessageService noticeMessageService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _iRedisStackExchangeService = iRedisStackExchangeService;
            _homePersionService = homePersionService;
            _noticeMessageService = noticeMessageService;
        }

        /// <summary>
        /// 获得爱快Cookie
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetIKuaiCookie()
        {
            var cookie = "";
            try
            {
                var uri = new Uri("http://192.168.6.1/Action/login");
                var cookieContainer = new CookieContainer();
                var handler = new HttpClientHandler();
                handler.CookieContainer = cookieContainer;
                var client = new HttpClient(handler);
                var param = new LoginInput()
                {
                    Username = "xzm",
                    Passwd = "02a0bf6b5fb9cd331529e210fa0b25c4",
                    Pass = "c2FsdF8xMXd6ZDM0MTU3MTEu",
                    Remember_password = true,
                };
                var content = new StringContent(param.ToJSONCamelCase());
                var response = await client.PostAsync(uri, content);
                var result = (await response.Content.ReadAsStringAsync()).FromJSON<IKuaiResult<object>>();

                if (result.Result == 10000)
                {
                    List<Cookie> cookies = cookieContainer.GetCookies(uri).Cast<Cookie>().ToList();
                    StringBuilder sb_cookie = new StringBuilder();
                    foreach (var item in cookies)
                    {
                        sb_cookie.Append(item.Name);
                        sb_cookie.Append("=");
                        sb_cookie.Append(item.Value);
                        sb_cookie.Append(";");

                    }
                    cookie = sb_cookie.ToString();
                    await _iRedisStackExchangeService.StringSetAsync(SystemConst.IkuaiCookie, cookie);

                }
                return cookie;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// 查询路由器网关列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Vlan_data>> GetPPPoEGatewayList(bool isAutoUpdateCookie = true)
        {

            var param = new CallInput<PPPoEGatewayParam>()
            {
                Func_name = "wan",
                Action = "show",
                Param = new PPPoEGatewayParam()
                {
                    TYPE = "vlan_data,vlan_total",
                    ORDER_BY = "vlan_name",
                    ORDER = "asc",
                    Vlan_internet = 2,
                    Interface = "wan1",
                    Limit = "0,20",
                },
            };
            var result = await GetIkuaiCall<PPPoEGatewayParam, PPPoEGateway>(param);
            if (result.Result == 30000)
            {
                return result?.Data?.Vlan_data;
            }
            else
            {
                if (isAutoUpdateCookie)
                {
                    await GetIKuaiCookie();
                    return (await GetIkuaiCall<PPPoEGatewayParam, PPPoEGateway>(param))?.Data?.Vlan_data;
                }
                return null;
            }
        }

        /// <summary>
        /// 发出爱快API请求
        /// </summary>
        /// <returns></returns>
        public async Task<IKuaiResult<R>> GetIkuaiCall<T, R>(CallInput<T> param)
        {
            var xxx = param.ToJSON();
            var cookie = await _iRedisStackExchangeService.StringGetAsync(SystemConst.IkuaiCookie);
            if (cookie.IsBlank())
            {
                cookie = await GetIKuaiCookie();
            }
            var uri = new Uri("http://192.168.6.1/Action/call");
            var handler = new HttpClientHandler();
            handler.UseCookies = false;
            var client = new HttpClient(handler);
            var content = new StringContent(param.ToJSON());
            content.Headers.Add("Cookie", cookie);
            var response = await client.PostAsync(uri, content);
            var xxxx = await response.Content.ReadAsStringAsync();
            var result = (await response.Content.ReadAsStringAsync()).FromJSON<IKuaiResult<R>>();
            return result;
        }

        /// <summary>
        /// 路由器重新拨号
        /// </summary>
        /// <returns></returns>
        public async Task<List<Vlan_data>> RouterPPPoERedial(bool isAutoUpdateCookie = true)
        {
            var param = new CallInput<PPPoERedial>()
            {
                Func_name = "wan",
                Action = "vlan_down",
                Param = new PPPoERedial()
                {
                    Id = "1,2,3,4",
                },
            };
            var downResult = await GetIkuaiCall<PPPoERedial, object>(param);
            param.Action = "vlan_up";
            var upResult = await GetIkuaiCall<PPPoERedial, object>(param);

            if (downResult.Result == 30000 && upResult.Result == 30000)
            {
                List<Vlan_data> result = null;
                for (int i = 0; i < 10; i++)
                {
                    result = await GetPPPoEGatewayList(false);
                    if (result.All(c => c.Pppoe_gateway.IsNotBlank()))
                    {
                        return result;
                    }
                    Thread.Sleep(1000);
                }
                return result;
            }
            else
            {
                if (isAutoUpdateCookie)
                {
                    await GetIKuaiCookie();
                    param.Action = "vlan_down";
                    downResult = await GetIkuaiCall<PPPoERedial, object>(param);
                    param.Action = "vlan_up";
                    upResult = await GetIkuaiCall<PPPoERedial, object>(param);
                    if (downResult.Result == 30000 && upResult.Result == 30000)
                    {
                        List<Vlan_data> result = null;
                        for (int i = 0; i < 10; i++)
                        {
                            result = await GetPPPoEGatewayList(false);
                            if (result.All(c => c.Pppoe_gateway.IsNotBlank()))
                            {
                                return result;
                            }
                            Thread.Sleep(1000);
                        }
                        return result;
                    }
                }
                _logger.LogError("爱快重新拨号异常(停用结果):" + downResult.ToJSON());
                _logger.LogError("爱快重新拨号异常(启用结果):" + upResult.ToJSON());
                throw new BusinessException("爱快重新拨号异常,请查看日志");
            }
        }


        /// <summary>
        /// 查询在家的家庭成员
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetPensionAtHome(bool isAutoUpdateCookie = true)
        {
            var result = new List<string>();
            var now = DateTime.Now;
            if (7 <= now.Hour && now.Hour <= 22)//只在每天的特定时间内检测 7:00-22:59
            {
                var param = new CallInput<MonitorLanip>()
                {
                    Func_name = "monitor_lanip",
                    Action = "show",
                    Param = new MonitorLanip()
                    {
                        TYPE = "data,total",
                        ORDER_BY = "ip_addr_int",
                        OrderType = "IP",
                        Limit = "0,100",
                        ORDER = "",
                    },
                };
                var httpResult = await GetIkuaiCall<MonitorLanip, MonitorLanipOutput>(param);
                var output = new MonitorLanipOutput();
                if (httpResult.Result == 30000)
                {
                    output = httpResult?.Data;
                }
                else
                {
                    if (isAutoUpdateCookie)
                    {
                        await GetIKuaiCookie();
                        output = (await GetIkuaiCall<MonitorLanip, MonitorLanipOutput>(param))?.Data;
                    }
                }
                if (output?.Data == null || output.Data.Count == 0)
                {
                    throw new BusinessException("未获取到任何在线设备或鉴权失败");
                }
                var macList = output.Data.Select(c => c.Mac).ToList();
                var pensonList = await _homePersionService.Queryable().ToListAsync();
                var atHomeList = new List<string>();
                var notAtHomeList = new List<string>();
                pensonList.ForEach(x =>
                {
                    if (macList.Contains(x.Mac))
                    {
                        //在家
                        result.Add(x.Name);
                        //_logger.LogInformation($"检测到家庭成员[{x.Name}]在家");
                        if (!x.IsAtHome)
                        {
                            x.IsAtHome = true;
                            //推送通知
                            atHomeList.Add(x.Name);
                            _logger.LogInformation($"检测到家庭成员[{x.Name}]在家,距离上次离家已有{x.NotCheckedNumber}分钟");
                        }
                        x.NotCheckedNumber = 0;
                    }
                    else
                    {
                        //离家
                        x.NotCheckedNumber++;
                        //_logger.LogInformation($"检测到家庭成员[{x.Name}]不在家(第{x.NotCheckedNumber}次)");
                        if (x.IsAtHome && x.NotCheckedNumber > x.JudgmentThreshold)
                        {
                            x.IsAtHome = false;
                            //推送通知
                            notAtHomeList.Add(x.Name);
                            _logger.LogInformation($"检测到家庭成员[{x.Name}]不在家(最终判定)");
                        }
                    }
                });

                //推送通知
                var text = "";
                if (atHomeList.Count > 0)
                {
                    text += $"检测到下列家庭成员在家:{Environment.NewLine}";
                    atHomeList.ForEach(x =>
                    {
                        text += $"{x}{Environment.NewLine}";
                    });
                }
                if (notAtHomeList.Count > 0)
                {
                    text += $"检测到下列家庭成员离家:{Environment.NewLine}";
                    notAtHomeList.ForEach(x =>
                    {
                        text += $"{x}{Environment.NewLine}";
                    });
                }
                await _homePersionService.BatchUpdateAsync(pensonList);
                if (text.IsNotBlank())
                {
                    await _noticeMessageService.PublishNoticeMessageToGroup("家庭组", text, false);
                }
            }
            return result;
        }
    }
}
