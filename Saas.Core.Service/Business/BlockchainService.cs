using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 区块链服务
    /// </summary>
    public class BlockchainService
    {
        private readonly ILogger<BlockchainService> _logger;
        private IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IRedisStackExchangeService _iRedisStackExchangeService;
        private readonly BusNoticeMessageService _noticeMessageService;
        private readonly string key = "ELLFW7EA1BHHRY1SQ444IB9WOMKZSWTWTSEFDZMY";

        /// <summary>
        /// ctor
        /// </summary>
        public BlockchainService(ILogger<BlockchainService> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, IRedisStackExchangeService iRedisStackExchangeService, BusNoticeMessageService noticeMessageService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _iRedisStackExchangeService = iRedisStackExchangeService;
            _noticeMessageService = noticeMessageService;
        }

        /// <summary>
        /// 获取所有支持的交易所列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<MfcMarketsOutput>> GetMarkets()
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            client.DefaultRequestHeaders.Add("X-API-KEY", key);
            var response = await client.GetAsync($"https://data.mifengcha.com/api/v3/markets");
            var result = (await response.Content.ReadAsStringAsync()).FromJSON<List<MfcMarketsOutput>>();
            return result;
        }

        /// <summary>
        /// 获取指定交易所信息
        /// </summary>
        /// <returns></returns>
        public async Task<MfcMarketsOutput> GetMarketBySlug(string slug)
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            client.DefaultRequestHeaders.Add("X-API-KEY", key);
            var response = await client.GetAsync($"https://data.mifengcha.com/api/v3/markets/{slug}");
            var result = (await response.Content.ReadAsStringAsync()).FromJSON<MfcMarketsOutput>();
            return result;
        }

        /// <summary>
        /// 获取所有支持的币种列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<MfcSymbolsOutput>> GetSymbols()
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            client.DefaultRequestHeaders.Add("X-API-KEY", key);
            var response = await client.GetAsync($"https://data.mifengcha.com/api/v3/symbols");
            var result = (await response.Content.ReadAsStringAsync()).FromJSON<List<MfcSymbolsOutput>>();
            return result;
        }

        /// <summary>
        /// 获取单个币种信息
        /// </summary>
        /// <returns></returns>
        public async Task<MfcSymbolsOutput> GetSymbols(string slug)
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            client.DefaultRequestHeaders.Add("X-API-KEY", key);
            var response = await client.GetAsync($"https://data.mifengcha.com/api/v3/symbols/{slug}");
            var result = (await response.Content.ReadAsStringAsync()).FromJSON<MfcSymbolsOutput>();
            return result;
        }

        /// <summary>
        /// 获取币种价格
        /// </summary>
        /// <returns></returns>
        public async Task<List<MfcPriceOutput>> GetPrice(string slug)
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            client.DefaultRequestHeaders.Add("X-API-KEY", key);
            var response = await client.GetAsync($"https://data.mifengcha.com/api/v3/price/?slug={slug}");
            var result = (await response.Content.ReadAsStringAsync()).FromJSON<List<MfcPriceOutput>>();
            return result;
        }

    }
}
