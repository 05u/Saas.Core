using DotNetCore.CAP.Messages;
using Exceptionless;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Saas.Core.Data.Context;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Base;
using Saas.Core.Service.Configs;
using Saas.Core.WebApi;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Text;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    SystemInfo.SystemStartTime = DateTime.Now;
    var builder = WebApplication.CreateBuilder(args);
    builder.WebHost.UseUrls("http://*:30005");
    //builder.WebHost.UseUrls("http://*:5000;https://*:5001");
    //builder.WebHost.UseKestrel(options =>
    //{
    //    var x509ca = new X509Certificate2(File.ReadAllBytes(@"r:\test.pfx"), "test");
    //    options.ListenAnyIP(5001, config => config.UseHttps(x509ca));
    //});


    // Add services to the container.

    //配置文件
    //var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    //builder.Services.AddSingleton<IConfiguration>(serviceProvider =>
    // {
    //     IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
    //     configurationBuilder.AddJsonFile("appsettings.json");
    //     return configurationBuilder.Build();
    // });
    //IConfiguration configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();
    var configuration = builder.Configuration;

    //注册数据库
    builder.Services.AddDatabase(configuration);

    //显示错误关键信息
    //IdentityModelEventSource.ShowPII = true;

    //添加jwt验证：
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = nameof(ApiAuthenticationHandler);
        options.DefaultForbidScheme = nameof(ApiAuthenticationHandler);

    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,//是否验证Issuer
            ValidateAudience = true,//是否验证Audience
            ValidateLifetime = true,//是否验证失效时间
            ValidateIssuerSigningKey = true,//是否验证SecurityKey
            ValidAudience = "User",//接收者
            ValidIssuer = Dns.GetHostName() ?? "",//签发者
            ClockSkew = TimeSpan.Zero, // // 默认允许 300s  的时间偏移量，设置为0
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenHelper.secretKey))//拿到SecurityKey
        };
    })
    .AddScheme<AuthenticationSchemeOptions, ApiAuthenticationHandler>(nameof(ApiAuthenticationHandler), o => { });

    //如果部署在linux系统上，需要加上下面的配置：
    builder.Services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
    //如果部署在IIS上，需要加上下面的配置：
    //builder.Services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);

    builder.Services.AddControllers(x =>
    {
        //统一数据返回格式处理
        x.Filters.Add<WebApiResponseDataMiddleware>();

        x.Filters.Add<UnitOfWorkFilter>();
    }).AddJsonConfig();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        //开启权限小锁
        options.OperationFilter<AddResponseHeadersFilter>();
        options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

        //在header中添加token，传递到后台
        options.OperationFilter<SecurityRequirementsOperationFilter>();
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "JWT授权(数据将在请求头中进行传递)直接在下面框中输入Bearer {token}(注意两者之间是一个空格)",
            Name = "Authorization",//jwt默认的参数名称
            In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
            Type = SecuritySchemeType.ApiKey
        });

        options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi Test", Version = "1.0.0.0" });
        // 批量注释文件添加
        var xmlFiles = new DirectoryInfo(AppContext.BaseDirectory).GetFiles("*.xml");
        foreach (var file in xmlFiles)
        {
            options.IncludeXmlComments(file.FullName, true);
        }
    });

    //HttpClientFactory
    builder.Services.AddHttpClient(HttpClientConst.CommonClient);

    //注册automapper
    builder.Services.UseAutoMapperExtention();

    //注入缓存Service
    builder.Services.AddMemoryCache();//使用缓存
    builder.Services.AddSingleton<ICacheService, CacheService>();
    builder.Services.AddSingleton<IRedisStackExchangeService, RedisStackExchangeService>();
    builder.Services.AddCaching(configuration);

    //注入任务调度器
    builder.Services.AddHangfireWebService(configuration);

    //获取IP使用
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    //注册当前用户获取服务
    builder.Services.AddCurrentUserService();

    //注册业务Services
    builder.Services.ConfigureServices();

    //初始化任务
    var init = new Init();
    init.InitUploadFilePath();
    init.SetConfig(configuration);


    //注册Exceptionless
    //builder.Services.Configure<ExceptionlessSetting>(config.GetSection("Exceptionless"));
    bool isOpenExceptionLess = Convert.ToBoolean(configuration["Exceptionless:IsActive"]);
    if (isOpenExceptionLess)
    {
        var apikey = Convert.ToString(configuration["Exceptionless:ApiKey"]);
        builder.Services.AddExceptionless(new ExceptionlessClient(x =>
        {
            x.ServerUrl = Convert.ToString(configuration["Exceptionless:ServerUrl"]);
            x.ApiKey = Convert.ToString(configuration["Exceptionless:ApiKey"]);

        }));
    }

    builder.Services.AddCap(x =>
    {
        x.UseEntityFramework<MainDbContext>();
        x.UseRabbitMQ(x =>
        {
            x.HostName = Convert.ToString(configuration["RabbitMQ:HostName"]);
            x.Port = Convert.ToInt32(configuration["RabbitMQ:Port"]);
            x.Password = Convert.ToString(configuration["RabbitMQ:Password"]);
            x.UserName = Convert.ToString(configuration["RabbitMQ:UserName"]);
            x.VirtualHost = configuration["RabbitMQ:VHost"] ?? "/";
            //x.ExchangeName = "";                    
        });
        x.UseDashboard();
        x.FailedRetryCount = 3;
        x.FailedThresholdCallback = failed =>
        {
            logger.Error($@"A message of type {failed.MessageType} failed after executing {x.FailedRetryCount} several times, 
                        requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
            //throw new BusinessException("参数验证错误,请检查!", errors);
        };
    });

    var app = builder.Build();

    //注册认证授权
    app.UseAuthentication();
    app.UseAuthorization();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((doc, _) =>
        {
            doc.Servers?.Clear();
            doc.Servers?.Add(new OpenApiServer { Url = "http://api.service.zhimin.me:30005", Description = "外网" });
            doc.Servers?.Add(new OpenApiServer { Url = "http://192.168.6.141:30005", Description = "内网" });
            doc.Servers?.Add(new OpenApiServer { Url = "http://localhost:30005", Description = "本地" });
        });
    });
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/swagger/v1/swagger.json", "Default");

        //如果设置根目录为swagger,将此值置空
        option.RoutePrefix = string.Empty;
        option.DocumentTitle = "WebApi Test";
        //option.DefaultModelsExpandDepth(-1);
    });
    //}

    //检查当前项目启动后，监听的是否是多个端口，其中如果有协议是Https—我们在访问Http的默认会转发到Https中
    //app.UseHttpsRedirection();


    //全局异常处理
    app.UseMiddleware<WebApiGlobalExceptionMiddleware>();
    app.UseMiddleware<CorsMiddleware>();
    if (isOpenExceptionLess)
    {
        app.UseExceptionless();
    }
    app.MapControllers();

    //任务调度
    app.UseHangfireWebService();
    RecurringJobConfig.ConfigureRecurringJobs();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
