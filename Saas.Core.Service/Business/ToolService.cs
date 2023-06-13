using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;
using System.Net.Http.Json;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 工具服务
    /// </summary>
    public class ToolService
    {
        private readonly ILogger<ToolService> _logger;
        private IHttpClientFactory _httpClientFactory;
        private readonly BusNoticeMessageService _noticeMessageService;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// ctor
        /// </summary>
        public ToolService(ILogger<ToolService> logger, IHttpClientFactory httpClientFactory, BusNoticeMessageService noticeMessageService, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _noticeMessageService = noticeMessageService;
            _configuration = configuration;
        }


        /// <summary>
        /// ChatGPT语言模型接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ChatGPTOutput> ChatGPT(ChatGPTInput input)
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            client.DefaultRequestHeaders.Add("Authorization", _configuration.GetSection("OpenAiToken").Value);

            var content = JsonContent.Create(input);
            var response = await client.PostAsync(_configuration.GetSection("UrlConfig:ChatGPTApi").Value, content);
            var result = (await response.Content.ReadAsStringAsync()).FromJSON<ChatGPTOutput>();
            return result;
        }

        /// <summary>
        /// 检查内网穿透
        /// </summary>
        /// <returns></returns>
        public async Task CheckIntranetPenetration()
        {
            var now = DateTime.Now;
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            client.DefaultRequestHeaders.Add("Authorization", _configuration.GetSection("FrpToken").Value);
            var response = await client.GetAsync(_configuration.GetSection("UrlConfig:FrpApi").Value);
            var result = (await response.Content.ReadAsStringAsync()).FromJSON<FrpOutput>();
            var warningList = result.Proxies.Where(c => c.Status == "online").Where(c => (now - DateTime.Parse(now.Year + "-" + c.Last_start_time)).Minutes == 59).ToList();
            var text = $"检测到下列内网穿透服务仍在运行,如不用请及时关闭!{Environment.NewLine}";
            foreach (var item in warningList)
            {
                text += $"----------{Environment.NewLine}";
                text += $"名称:{item.Name}{Environment.NewLine}端口号:{item.Conf?.Remote_port}{Environment.NewLine}启用时间:{item.Last_start_time}{Environment.NewLine}";
            }
            if (warningList.Count > 0)
            {
                await _noticeMessageService.PublishNoticeMessageToGroup("慧明", text, false);
            }

        }
    }
}
