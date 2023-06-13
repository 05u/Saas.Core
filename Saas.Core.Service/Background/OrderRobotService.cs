using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Business;
using System.Reactive.Linq;

namespace Saas.Core.Service.Background
{
    /// <summary>
    /// 订单机器人服务
    /// </summary>
    public class OrderRobotService : BackgroundService
    {
        private readonly ILogger<OrderRobotService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration Configuration;

        /// <summary>
        /// ctor
        /// </summary>
        public OrderRobotService(ILogger<OrderRobotService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            Configuration = configuration;
        }

        /// <summary>
        /// 启动订单机器人服务
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool isActive = Convert.ToBoolean(Configuration["OrderBot:IsActive"]);
            if (isActive)
            {
                try
                {

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var noticeMessageService = scope.ServiceProvider.GetService<BusNoticeMessageService>();
                        var redisStackExchangeService = scope.ServiceProvider.GetService<IRedisStackExchangeService>();

                        var exit = new ManualResetEvent(false);

                        var bot = new MiraiBot
                        {
                            Address = Configuration.GetSection("OrderBot:Address").Value,
                            VerifyKey = Configuration.GetSection("OrderBot:VerifyKey").Value,
                            QQ = Configuration.GetSection("OrderBot:QQ").Value,
                        };

                        await bot.LaunchAsync();

                        //订阅好友消息
                        bot.MessageReceived
                            .OfType<FriendMessageReceiver>()
                            .Subscribe(async r =>
                            {
                                if (r.Sender.Id != bot.QQ)
                                {
                                    var msg = r.MessageChain.GetPlainMessage().Trim();
                                    if (msg.IsContainsAny(new string[] { "C#", "c#", ".net", ".NET", ".Net", "winform", "asp", "毕设", "毕业设计", "作业" }))
                                    {
                                        _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:OrderRobotReceive", TimeSpan.FromDays(365), 1);
                                        await noticeMessageService.PublishNoticeMessageToGroup("个人组", $"发现可接订单!{Environment.NewLine}发单人:{r.Sender.NickName}({r.Sender.Id}){Environment.NewLine}订单详情:{Environment.NewLine}{msg}", false);
                                    }
                                }
                            });

                        //订阅群消息
                        bot.MessageReceived
                            .OfType<GroupMessageReceiver>()
                            .Subscribe(async r =>
                            {
                                if (r.Sender.Id != bot.QQ)
                                {
                                    var msg = r.MessageChain.GetPlainMessage().Trim();
                                    if (msg.IsContainsAny(new string[] { "C#", "c#", ".net", ".NET", ".Net", "winform", "asp", "毕设", "毕业设计", "作业" }))
                                    {
                                        _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:OrderRobotReceive", TimeSpan.FromDays(365), 1);
                                        await noticeMessageService.PublishNoticeMessageToGroup("个人组", $"发现可接订单!{Environment.NewLine}发单人:{r.Sender.Name}({r.Sender.Id}){Environment.NewLine}发单群:{r.GroupName}({r.GroupId}){Environment.NewLine}订单详情:{Environment.NewLine}{msg}", false);
                                    }
                                }

                            });


                        //订阅临时消息
                        bot.MessageReceived
                            .OfType<TempMessageReceiver>()
                            .Subscribe(async r =>
                            {
                                if (r.Sender.Id != bot.QQ)
                                {
                                    var msg = r.MessageChain.GetPlainMessage().Trim();
                                    if (msg.IsContainsAny(new string[] { "C#", "c#", ".net", ".NET", ".Net", "winform", "asp", "毕设", "毕业设计", "作业" }))
                                    {
                                        _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:OrderRobotReceive", TimeSpan.FromDays(365), 1);
                                        await noticeMessageService.PublishNoticeMessageToGroup("个人组", $"发现可接订单!{Environment.NewLine}发单人:{r.Sender.Name}({r.Sender.Id}){Environment.NewLine}订单详情:{Environment.NewLine}{msg}", false);
                                    }
                                }

                            });


                        _logger.LogInformation($"订单机器人[{bot.QQ}] 启动...");
                        exit.WaitOne();

                    }


                }
                catch (Exception e)
                {
                    _logger.LogError($"订单机器人发生异常:{e.Message}");
                }
            }
        }
    }
}
