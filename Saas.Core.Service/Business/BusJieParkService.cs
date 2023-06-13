using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 捷停车
    /// </summary>
    public class BusJieParkService : Repository<BusJiePark>
    {
        private IHttpClientFactory _httpClientFactory;
        private readonly BusNoticeMessageService _noticeMessageService;

        /// <summary>
        /// ctor
        /// </summary>
        public BusJieParkService(MainDbContext context, ICurrentUserService currentUserService, IHttpClientFactory httpClientFactory, BusNoticeMessageService noticeMessageService) : base(context, currentUserService)
        {
            _httpClientFactory = httpClientFactory;
            _noticeMessageService = noticeMessageService;
        }


        /// <summary>
        /// 停车场领券
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetParkCoupon(bool ignoreWorkday = false)
        {
            if (ignoreWorkday == true || await IsWorkday() == true)
            {
                var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);

                var taskList = await Queryable().ToListAsync();

                foreach (var item in taskList)
                {
                    //停车场信息
                    var parkInfo = await GetParkInfo(item.ParkCode);
                    if (parkInfo.FavourList != null && parkInfo.FavourList.Count > 0)
                    {
                        var param = new GetParkCouponDto
                        {
                            PlanNo = parkInfo.FavourList.FirstOrDefault()?.PlanNo,
                            UserId = item.UserId,
                            Mobile = item.Mobile,
                        };
                        var content = JsonContent.Create(param);
                        var response = await client.PostAsync($"https://sytgate.jslife.com.cn/base-gateway/coupons/receive", content);
                        var result = (await response.Content.ReadAsStringAsync()).FromJSON<GetParkCouponResultDto>();
                        if (item.MessageGroupId.IsNotBlank())
                        {
                            await _noticeMessageService.PublishNoticeMessageToGroup(null, $"{parkInfo.ParkName}{Environment.NewLine}领券结果:{result?.Message ?? "无返回或解析失败!"}{Environment.NewLine}实时空位:{parkInfo?.EmptySpaces}个", false, item.MessageGroupId);
                        }
                        //随机延迟,避免风控
                        Random ra = new();
                        Thread.Sleep(ra.Next(1000, 2000));
                    }
                }
                return $"{taskList.Count}条领券任务执行完毕";
            }
            else
            {
                return $"非工作日不自动领券";
            }
        }

        /// <summary>
        /// 获取停车场余位
        /// </summary>
        /// <returns></returns>
        public async Task<int?> GetParkFreeSpace(string parkCode)
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);

            var param = new GetParkInfoInputDto
            {
                ParkCode = parkCode,
            };
            var content = JsonContent.Create(param);
            var response = await client.PostAsync($"https://sytgate.jslife.com.cn/core-gateway/cwzg/v4/park_attention_info", content);
            var result = (await response.Content.ReadAsStringAsync()).FromJSON<ParkInfoBase<GetParkInfoOutputDto>>();
            return result?.Data?.EmptySpaces;
        }

        /// <summary>
        /// 获取停车场信息
        /// </summary>
        /// <returns></returns>
        public async Task<GetParkInfoOutputDto> GetParkInfo(string parkCode)
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);

            var param = new GetParkInfoInputDto
            {
                ParkCode = parkCode,
            };
            var content = JsonContent.Create(param);
            var response = await client.PostAsync($"https://sytgate.jslife.com.cn/core-gateway/cwzg/v4/park_attention_info", content);
            var result = (await response.Content.ReadAsStringAsync()).FromJSON<ParkInfoBase<GetParkInfoOutputDto>>();
            return result?.Data;
        }

        /// <summary>
        /// 查询今日是否为工作日
        /// </summary>
        /// <returns></returns>
        public async Task<bool?> IsWorkday()
        {
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            var now = DateTime.Now;
            var response = await client.GetAsync($"https://api.apihubs.cn/holiday/get");
            var result = (await response.Content.ReadAsStringAsync()).FromJSON<ParkInfoBase<HolidayList>>();
            var today = result?.Data?.List?.FirstOrDefault(c => c.Date.ToString() == now.ToString("yyyyMMdd"));
            if (today?.Workday == 1)//工作日
            {
                return true;
            }
            if (today?.Workday == 2)//非工作日
            {
                return false;
            }
            return null;//获取失败
        }
    }
}
