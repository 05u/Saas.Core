using EasyCaching.Core.Configurations;
using EasyCaching.Interceptor.AspectCore;
using EasyCaching.Serialization.SystemTextJson.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Utilities;
//using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// Web 基础服务提供者
    /// </summary>
    public static class BaseWebService
    {
        /// <summary>
        /// CorsName
        /// </summary>
        public const string CorsName = "Cors";

        /// <summary>
        /// 获取json配置
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerOptions GetOxygenJsonOptions()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                //增加默认json序列化的编码规则，避免中文乱码
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                //驼峰式名称转换
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            //日期格式化转换器
            jsonOptions.Converters.Add(new DatetimeJsonConverter());

            return jsonOptions;
        }

        /// <summary>
        /// 添加json.net的配置转换规则
        /// </summary>
        /// <param name="mvcBuilder"></param>
        public static void AddJsonConfig(this IMvcBuilder mvcBuilder)
        {
            //mvcBuilder.AddJsonOptions(config =>
            //{
            //    //增加默认json序列化的编码规则，避免中文乱码
            //    config.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            //    //驼峰式名称转换
            //    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //    //日期格式化转换器
            //    config.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());

            //});

            mvcBuilder.AddNewtonsoftJson(config =>
            {
                config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                config.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                config.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                config.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            });
        }

        /// <summary>
        /// 注册当前用户服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddCurrentUserService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }

        ///// <summary>
        ///// 添加Swagger服务
        ///// </summary>
        ///// <param name="services"></param>
        //public static void AddOxygenSwaggerService(this IServiceCollection services)
        //{
        //    services.AddSwaggerGen(option =>
        //    {
        //        option.SwaggerDoc("sparktodo", new OpenApiInfo
        //        {
        //            Version = "v1",
        //            Title = "Oxygen API",
        //            Description = "API for Oxygen",
        //            Contact = new OpenApiContact() { Name = "Oxygen", Email = "service@Oxygen.com" }
        //        });

        //        // 批量注释文件添加
        //        var xmlFiles = new DirectoryInfo(AppContext.BaseDirectory).GetFiles("*.xml");
        //        foreach (var file in xmlFiles)
        //        {
        //            option.IncludeXmlComments(file.FullName, true);
        //        }

        //        //Token认证配置
        //        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        //        {
        //            Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
        //            Name = "Authorization",
        //            In = ParameterLocation.Header,
        //            Type = SecuritySchemeType.ApiKey,
        //        });
        //        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        //        {
        //            {
        //                new OpenApiSecurityScheme
        //                {
        //                    Reference = new OpenApiReference()
        //                    {
        //                        Id = "Bearer",
        //                        Type = ReferenceType.SecurityScheme
        //                    }
        //                }, Array.Empty<string>()
        //            }
        //        });

        //        //分组  

        //        option.SwaggerDoc(nameof(SystemConst.SYSTEM), new OpenApiInfo { Title = nameof(SystemConst.SYSTEM), Version = nameof(SystemConst.SYSTEM) });   //分组显示
        //        option.SwaggerDoc(nameof(SystemConst.MDM), new OpenApiInfo { Title = nameof(SystemConst.MDM), Version = nameof(SystemConst.MDM) });   //分组显示
        //        option.SwaggerDoc(nameof(SystemConst.EDI), new OpenApiInfo { Title = nameof(SystemConst.EDI), Version = nameof(SystemConst.EDI) });   //分组显示
        //        option.SwaggerDoc(nameof(SystemConst.ANDON), new OpenApiInfo { Title = nameof(SystemConst.ANDON), Version = nameof(SystemConst.ANDON) });   //分组显示          
        //        option.SwaggerDoc(nameof(SystemConst.WMS), new OpenApiInfo { Title = nameof(SystemConst.WMS), Version = nameof(SystemConst.WMS) });   //分组显示
        //        option.SwaggerDoc(nameof(SystemConst.MES), new OpenApiInfo { Title = nameof(SystemConst.MES), Version = nameof(SystemConst.MES) });   //分组显示
        //        option.SwaggerDoc(nameof(SystemConst.TPM), new OpenApiInfo { Title = nameof(SystemConst.TPM), Version = nameof(SystemConst.TPM) });   //分组显示

        //        //option.SwaggerDoc("User", new OpenApiInfo { Title = "用户模块", Version = "User" });   //分组显示
        //        //option.SwaggerDoc("Project", new OpenApiInfo { Title = "项目模块", Version = "Project" });   //分组显示
        //        //option.SwaggerDoc("User", new OpenApiInfo { Title = "用户模块", Version = "User" });   //分组显示
        //        //option.SwaggerDoc("Project", new OpenApiInfo { Title = "项目模块", Version = "Project" });   //分组显示
        //        //option.SwaggerDoc("User", new OpenApiInfo { Title = "用户模块", Version = "User" });   //分组显示
        //        //option.SwaggerDoc("Project", new OpenApiInfo { Title = "项目模块", Version = "Project" });   //分组显示


        //    });
        //}

        ///// <summary>
        ///// 使用Swagger以及UI
        ///// </summary>
        ///// <param name="app"></param>
        //public static void UseOxygenSwagger(this IApplicationBuilder app)
        //{
        //    //Enable middleware to serve generated Swagger as a JSON endpoint.
        //    //app.UseSwagger();

        //    app.UseSwagger(c =>
        //    {


        //        c.PreSerializeFilters.Add((doc, _) =>
        //        {
        //            //doc.Servers?.Clear();
        //            doc.Servers?.Add(new OpenApiServer { Url = "http://192.168.56.75:64454", Description = "前端测试api" });
        //            doc.Servers?.Add(new OpenApiServer { Url = "http://124.70.147.142:64454", Description = "IRAP" });
        //            doc.Servers?.Add(new OpenApiServer {   Url="http://localhost:64454", Description="本地测试api-后端" });
        //        });
        //    });

        //    //Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint
        //    app.UseSwaggerUI(option =>
        //    {
        //        //option.SwaggerEndpoint("/swagger/sparktodo/swagger.json", "Oxygen Api Documents");
        //        option.SwaggerEndpoint($"/swagger/{nameof(SystemConst.SYSTEM)}/swagger.json", nameof(SystemConst.SYSTEM));
        //        option.SwaggerEndpoint($"/swagger/{nameof(SystemConst.MDM)}/swagger.json", nameof(SystemConst.MDM));
        //        option.SwaggerEndpoint($"/swagger/{nameof(SystemConst.ANDON)}/swagger.json", nameof(SystemConst.ANDON));
        //        option.SwaggerEndpoint($"/swagger/{nameof(SystemConst.EDI)}/swagger.json", nameof(SystemConst.EDI));
        //        option.SwaggerEndpoint($"/swagger/{nameof(SystemConst.WMS)}/swagger.json", nameof(SystemConst.WMS));
        //        option.SwaggerEndpoint($"/swagger/{nameof(SystemConst.MES)}/swagger.json", nameof(SystemConst.MES));
        //        option.SwaggerEndpoint($"/swagger/{nameof(SystemConst.TPM)}/swagger.json", nameof(SystemConst.TPM));

        //        //如果设置根目录为swagger,将此值置空
        //        option.RoutePrefix = string.Empty;
        //        option.DocumentTitle = "Oxygen API";
        //    });
        //}

        ///// <summary>
        ///// Api参数模型验证
        ///// </summary>
        ///// <param name="services"></param>
        //public static void AddOxygenModelValidate(this IServiceCollection services)
        //{
        //    services.Configure<ApiBehaviorOptions>(options =>
        //    {
        //        options.InvalidModelStateResponseFactory = (context) =>
        //        {
        //            var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList().Join(",");
        //            var json = new ResponseJsonData<string> { Code = (int)HttpStatusCode.BadRequest, Message = errors };
        //            return new JsonResult(json);
        //        };
        //    });
        //}

        ///// <summary>
        ///// 添加IdentityServer 4认证服务
        ///// </summary>
        ///// <param name="services"></param>
        ///// <param name="configuration"></param>
        //public static void AddOxygenAuthenticatiaon(this IServiceCollection services, IConfiguration configuration)
        //{



        //    //IdentityServer 4 用户信息校验服务配置
        //    services.AddAuthentication(options =>
        //    {
        //        options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
        //        options.DefaultChallengeScheme = nameof(OxygenApiAuthenticationHandler);
        //        options.DefaultForbidScheme = nameof(OxygenApiAuthenticationHandler);

        //    })
        //    .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
        //    {
        //        options.Authority = configuration.GetValue<string>("IdentityServerUrl").TrimEnd('/');
        //        options.RequireHttpsMetadata = false;
        //        options.Audience = configuration.GetValue<string>("Api:Id");
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateAudience = false
        //        };
        //    })

        //    .AddScheme<AuthenticationSchemeOptions, OxygenApiAuthenticationHandler>(nameof(OxygenApiAuthenticationHandler), o => { })
        //    ;

        //}

        ///// <summary>
        ///// 认证授权
        ///// </summary>
        ///// <param name="app"></param>
        //public static void UseOxygenAuthentication(this IApplicationBuilder app)
        //{
        //    app.UseAuthentication();
        //    app.UseAuthorization();
        //}



        ///// <summary>
        ///// 认证授权
        ///// </summary>
        ///// <param name="app"></param>
        //public static void UseOxygenMiniProfiler(this IApplicationBuilder app)
        //{
        //    app.UseMiniProfiler();

        //}


        ///// <summary>
        ///// 认证授权
        ///// </summary>
        ///// <param name="app"></param>
        //public static void AddOxygenMiniProfiler(this IServiceCollection services)
        //{
        //    services.AddMiniProfiler(new Action<MiniProfilerOptions>(options =>
        //    {
        //        options.RouteBasePath = "/profiler";
        //        //options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();
        //        //options.ResultsAuthorize = _ => !Program.DisableProfilingResults;
        //        options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;
        //        options.EnableServerTimingHeader = true;
        //        options.IgnoredPaths.Add("/lib");
        //        options.IgnoredPaths.Add("/css");
        //        options.IgnoredPaths.Add("/js");
        //    })).AddEntityFramework();
        //}


        ///// <summary>
        ///// WebSockets服务器
        ///// </summary>
        ///// <param name="app"></param>
        //public static void UseOxygenSignalR(this IApplicationBuilder app)
        //{
        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapHub<TestHub>("/signalR/testhub");
        //    });

        //}


        ///// <summary>
        ///// 认证授权
        ///// </summary>
        ///// <param name="app"></param>
        //public static void AddOxygenSignalR(this IServiceCollection services)
        //{
        //    services.AddSignalR();
        //}


        public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            //configuration
            services.AddEasyCaching(options =>
            {
                //use memory cache that named default
                //options.UseInMemory("default");

                // // use memory cache with your own configuration
                // options.UseInMemory(config => 
                // {
                //     config.DBConfig = new InMemoryCachingOptions
                //     {
                //         // scan time, default value is 60s
                //         ExpirationScanFrequency = 60, 
                //         // total count of cache items, default value is 10000
                //         SizeLimit = 100 
                //     };
                //     // the max random second will be added to cache's expiration, default value is 120
                //     config.MaxRdSecond = 120;
                //     // whether enable logging, default is false
                //     config.EnableLogging = false;
                //     // mutex key's alive time(ms), default is 5000
                //     config.LockMs = 5000;
                //     // when mutex key alive, it will sleep some time, default is 300
                //     config.SleepMs = 300;
                // }, "m2");
                var serializerName = "s1";
                //use redis cache that named redis1
                options.UseRedis(config =>
                {
                    config.DBConfig.Endpoints.Add(new ServerEndPoint(configuration.GetValue<string>("Redis:Url"), Convert.ToInt32(configuration.GetValue<string>("Redis:Port"))));
                    var password = configuration.GetValue<string>("Redis:Password");
                    if (password.IsNotBlank())
                    {
                        config.DBConfig.Password = password;
                    }
                    config.SerializerName = serializerName;
                    config.DBConfig.Database = Convert.ToInt32(configuration.GetValue<string>("Redis:Database"));

                }, "redis1")
                //.WithMessagePack(serializerName)//with messagepack serialization
                .WithSystemTextJson(serializerName)
                .UseRedisLock()//with distributed lock
                ;
            });

            services.ConfigureAspectCoreInterceptor(options =>
            {
                // 可以在这里指定你要用那个provider
                // 或者在Attribute上面指定
                options.CacheProviderName = "redis1";
            });


        }



    }
}
