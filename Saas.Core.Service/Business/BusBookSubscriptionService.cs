using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Dtos;
using System.Text.RegularExpressions;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 图书订阅
    /// </summary>
    public class BusBookSubscriptionService : Repository<BusBookSubscription>
    {
        private IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BusBookSubscriptionService> _logger;
        private readonly BusNoticeMessageService _noticeMessageService;

        /// <summary>
        /// ctor
        /// </summary>
        public BusBookSubscriptionService(MainDbContext context, IHttpClientFactory httpClientFactory, ILogger<BusBookSubscriptionService> logger, BusNoticeMessageService noticeMessageService, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _noticeMessageService = noticeMessageService;
        }

        /// <summary>
        /// 获取库存列表
        /// </summary>
        /// <param name="name"></param>
        public async Task<List<StockInfo>> GetStockList(string name)
        {
            var list = new List<StockInfo>();
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);

            //先获取key
            var getKeyResponse = await client.GetAsync($"http://opac.jslib.org.cn/F?RN={Math.Round(new Random().NextDouble() * 1000000000)}");//模拟真实请求参数
            var keyResult = await getKeyResponse.Content.ReadAsStringAsync();
            var baseUrl = Regex.Match(keyResult, @"(?<=form1 action="").*?(?="")").ToString();

            var response = await client.GetAsync($"{baseUrl}?func=find-b&find_code=WRD&request={name}&local_base=CNBOK");
            var result = await response.Content.ReadAsStringAsync();

            var locationList = Regex.Matches(result, @"(?<= >).*?(?=<\/A>)");
            var findNoList = Regex.Matches(result, @"(?<=<td class=bookid>).*?(?=<td class=holding>)");
            var allCountList = Regex.Matches(result, @"(?<=<td class=holding>     ).*?(?=\/)");
            var outCountList = Regex.Matches(result, @"(?<=\/     ).*?(?=<\/td>)");
            int i = 0;
            int count = locationList.Count;
            if (count == findNoList.Count && count == allCountList.Count && count == outCountList.Count)
            {
                foreach (var item in locationList)
                {
                    _ = int.TryParse(allCountList[i].ToString(), out int allCount);
                    _ = int.TryParse(outCountList[i].ToString(), out int outCount);
                    list.Add(new StockInfo()
                    {
                        Location = item.ToString(),
                        FindNo = findNoList[i].ToString(),
                        AllCount = allCount,
                        OutCount = outCount,
                    });
                    i++;
                }
            }
            else
            {
                _logger.LogError($"南京图书馆解析数量不对等,{count},{findNoList.Count},{allCountList.Count},{outCountList.Count},原始内容:{result}");
                throw new BusinessException("南京图书馆解析失败,请查看日志!");
            }
            return list;
        }

        /// <summary>
        /// 确认库存(定时任务)
        /// </summary>
        /// <returns></returns>
        public async Task CheckStock()
        {
            var nowTime = DateTime.Now;
            var startTime = DateTime.Now.Date.AddHours(9);
            var endTime = DateTime.Now.Date.AddHours(18);

            if (startTime <= nowTime && nowTime <= endTime)
            {
                var task = Queryable().Where(c => !c.IsInStock).ToList();
                var name = task.Select(c => c.Name).Distinct().ToList();
                foreach (var x in name)
                {
                    var allBook = await GetStockList(x);
                    var canTakeBook = allBook.Where(c => !c.Location.Contains("保存本") && !c.Location.Contains("闭架库") && c.InCount > 0).ToList();

                    _logger.LogInformation($"图书库存结果:{canTakeBook.ToJSON}");

                    string canTakeBookText = "";

                    if (canTakeBook.Any())
                    {
                        //拼所有库位
                        foreach (var y in canTakeBook)
                        {
                            canTakeBookText += $"馆藏位置:{y.Location}{Environment.NewLine}索书号:{y.FindNo}{Environment.NewLine}馆藏总数:{y.AllCount}{Environment.NewLine}可借数量:{y.InCount}{Environment.NewLine}{Environment.NewLine}";

                        }

                        //本书订阅者列表
                        var subscription = task.Where(c => c.Name == x).ToList();
                        var msgList = new List<PublishNoticeMessageToInput>();

                        foreach (var z in subscription)
                        {
                            msgList.Add(new PublishNoticeMessageToInput()
                            {
                                Text = $"《{x}》有库存了,可前往南京图书馆借取!{Environment.NewLine}{Environment.NewLine}{canTakeBookText}",
                                Qq_Receiver = z.MessageType == MessageType.QQ ? z.Identification : null,
                                Wx_Wxid = z.MessageType == MessageType.Weixin ? z.Identification : null,
                                IsSaveRecord = false,
                            });

                            z.IsInStock = true;
                        }
                        //发送通知
                        await _noticeMessageService.PublishNoticeMessageListTo(msgList);


                    }


                }
                await BatchUpdateAsync(task);
            }


        }

        /// <summary>
        /// 创建图书订阅
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateBookSubscription(string name, MessageType messageType, string identification)
        {
            if (name.IsBlank() || identification.IsBlank())
            {
                throw new BusinessException("存在必填项未填");
            }
            var data = await Queryable().Where(c => c.Name == name && c.MessageType == messageType && c.Identification == identification).FirstOrDefaultAsync();
            if (data != null)
            {
                if (data.IsInStock == false)
                {
                    return "此书已在实时监控中,无需重复添加,有库存了会第一时间通知~";
                }
                else
                {
                    data.IsInStock = true;
                    await UpdateAsync(data);
                    return "已重新添加订阅,有库存了会第一时间通知~";
                }
            }
            else
            {
                data = new BusBookSubscription()
                {
                    Name = name,
                    MessageType = messageType,
                    Identification = identification,
                };
                await InsertAsync(data);
                return "已添加订阅,有库存了会第一时间通知~";

            }
        }
    }
}
