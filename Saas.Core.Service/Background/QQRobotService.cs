using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions;
using Mirai.Net.Utils.Scaffolds;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Business;
using System.Reactive.Linq;

namespace Saas.Core.Service.Background
{
    /// <summary>
    /// QQ机器人服务
    /// </summary>
    public class QQRobotService : BackgroundService
    {
        private readonly ILogger<QQRobotService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration Configuration;

        /// <summary>
        /// ctor
        /// </summary>
        public QQRobotService(ILogger<QQRobotService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            Configuration = configuration;
        }

        /// <summary>
        /// 启动QQ机器人服务
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool isActive = Convert.ToBoolean(Configuration["MiraiBot:IsActive"]);
            if (isActive)
            {
                try
                {

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var robotService = scope.ServiceProvider.GetService<RobotService>();
                        var eatMedicineService = scope.ServiceProvider.GetService<BusPregnantWomanEatMedicineRecordService>();
                        var redisStackExchangeService = scope.ServiceProvider.GetService<IRedisStackExchangeService>();

                        var exit = new ManualResetEvent(false);

                        var bot = new MiraiBot
                        {
                            Address = Configuration.GetSection("MiraiBot:Address").Value,
                            VerifyKey = Configuration.GetSection("MiraiBot:VerifyKey").Value,
                            QQ = Configuration.GetSection("MiraiBot:QQ").Value,
                        };

                        await bot.LaunchAsync();

                        //订阅好友消息
                        bot.MessageReceived
                            .OfType<FriendMessageReceiver>()
                            .Subscribe(async r =>
                            {
                                _logger.LogInformation($"收到QQ好友消息:{r.Sender.Id}");
                                if (r.Sender.Id != bot.QQ)
                                {
                                    _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:QQRobotReceive", TimeSpan.FromDays(365), 1);
                                    var msg = r.MessageChain.GetPlainMessage().Trim();
                                    var repMsg = await robotService.GeneralMessageProcess(msg, r.Sender.Id);
                                    if (repMsg.IsNotBlank())
                                    {
                                        await r.SendMessageAsync(repMsg);
                                        _logger.LogInformation($"回复好友消息:{repMsg}");
                                        _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:QQRobotSend", TimeSpan.FromDays(365), 1);
                                    }
                                }
                            });

                        //订阅群消息
                        bot.MessageReceived
                            .OfType<GroupMessageReceiver>()
                            .Subscribe(async r =>
                            {
                                _logger.LogInformation($"收到QQ群消息:{r.Sender.Id}");
                                if (r.Sender.Id != bot.QQ)
                                {
                                    var reveiver = r.ToJSON();
                                    if (reveiver.Contains($"\"target\":\"{bot.QQ}\""))
                                    {
                                        _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:QQRobotReceive", TimeSpan.FromDays(365), 1);
                                        Random ra = new Random();
                                        Thread.Sleep(ra.Next(500, 2000));
                                        //随机延迟,避免风控
                                        var msg = r.MessageChain.GetPlainMessage().Trim();
                                        var repMsg = await robotService.GeneralMessageProcess(msg, r.Sender.Id);
                                        if (repMsg.IsNotBlank())
                                        {
                                            await r.SendMessageAsync(repMsg);
                                            _logger.LogInformation($"回复群消息:{repMsg}");
                                            _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:QQRobotSend", TimeSpan.FromDays(365), 1);
                                        }
                                    }
                                }

                            });


                        //订阅临时消息
                        bot.MessageReceived
                            .OfType<TempMessageReceiver>()
                            .Subscribe(async r =>
                            {
                                _logger.LogInformation($"收到QQ临时消息:{r.Sender.Id}");
                                if (r.Sender.Id != bot.QQ)
                                {
                                    _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:QQRobotReceive", TimeSpan.FromDays(365), 1);
                                    Random ra = new Random();
                                    Thread.Sleep(ra.Next(1000, 5000));
                                    //随机延迟,避免风控
                                    var msg = r.MessageChain.GetPlainMessage().Trim();
                                    var repMsg = await robotService.GeneralMessageProcess(msg, r.Sender.Id);
                                    if (repMsg.IsNotBlank())
                                    {
                                        await r.SendMessageAsync(repMsg);
                                        _logger.LogInformation($"回复临时消息:{repMsg}");
                                        _ = await redisStackExchangeService.CacheCountCheck("CountAnalysis:QQRobotSend", TimeSpan.FromDays(365), 1);
                                    }
                                }

                            });


                        _logger.LogInformation($"QQ机器人[{bot.QQ}] 启动...");
                        exit.WaitOne();

                    }


                }
                catch (Exception e)
                {
                    _logger.LogError($"发生异常:{e.Message}");
                }
            }
        }
    }
}
