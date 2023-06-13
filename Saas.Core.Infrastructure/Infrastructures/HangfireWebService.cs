using Hangfire;
using Hangfire.Dashboard;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// 
    /// </summary>
    public static class HangfireWebService
    {
        /// <summary>
        /// 定时任务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddHangfireWebService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("JobSqlServer")));
            services.AddHangfireServer(x =>
            {
                x.ServerName = Environment.MachineName;
                x.WorkerCount = 1;
            });

            ////把定时任务和周期性任务区分开，运行在不同队列
            //services.AddHangfireServer(action =>
            //{
            //    action.ServerName = $"{Environment.MachineName}:default";
            //    action.Queues = new[] { "default" };
            //    action.WorkerCount = 1;
            //});

            //services.AddHangfireServer(action =>
            //{
            //    action.ServerName = $"{Environment.MachineName}:serial";
            //    action.Queues = new[] { "serial" };
            //    action.WorkerCount = Environment.ProcessorCount * 5;
            //});


            return services;
        }
        /// <summary>
        /// 使用HangfireForSqlServer
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHangfireWebService(this IApplicationBuilder app)
        {
            //var option = new DashboardOptions
            //{
            //    Authorization = new[] { new HangfireAuthorizationFilter() }
            //};
            //app.UseHangfireDashboard("/task");
            app.UseHangfireDashboard("/task", new DashboardOptions()
            {
                Authorization = new[] { new CustomAuthorizeFilter() }
            });
            //有效期30天
            GlobalStateHandlers.Handlers.Add(new SucceededStateExpireHandler(24 * 60 * 30));
            return app;
        }


        /// <summary>
        /// 面板授权验证
        /// </summary>
        public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public bool Authorize([NotNull] DashboardContext context)
            {

                //return true;

                // var req = context.GetHttpContext().Request;
                // //return httpcontext.User.Identity.IsAuthenticated;
                //// return true;
                // try
                // {

                //     string token = req.GetToken();
                //     if (!JWTHelper.CheckToken(token, JWTHelper.JWTSecret))
                //     {
                //         return false;
                //     }

                //     var payload = JWTHelper.GetPayload<JWTPayload>(token);
                //     if (payload.Expire < DateTime.Now)
                //     {
                //         return false;
                //     }
                //     return true;
                // }
                // catch (Exception ex)
                // {
                //     return false;
                // }

                var httpContext = context.GetHttpContext();

                var header = httpContext.Request.Headers["Authorization"];

                if (string.IsNullOrWhiteSpace(header))
                {
                    SetChallengeResponse(httpContext);
                    return false;
                }

                var authValues = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);

                if (!"Basic".Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase))
                {
                    SetChallengeResponse(httpContext);
                    return false;
                }

                var parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
                var parts = parameter.Split(':');

                if (parts.Length < 2)
                {
                    SetChallengeResponse(httpContext);
                    return false;
                }

                var username = parts[0];
                var password = parts[1];

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    SetChallengeResponse(httpContext);
                    return false;
                }

                if (username == "xzm" && password == "0124578")
                {
                    return true;
                }

                SetChallengeResponse(httpContext);
                return false;

            }

            private void SetChallengeResponse(HttpContext httpContext)
            {
                httpContext.Response.StatusCode = 401;
                httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
                httpContext.Response.WriteAsync("Authentication is required.");
            }
        }





        /// <summary>
        /// 已完成的job设置过期，防止数据无限增长
        /// </summary>
        public class SucceededStateExpireHandler : IStateHandler
        {
            public TimeSpan JobExpirationTimeout;

            public SucceededStateExpireHandler(int jobExpirationTimeout)
            {
                JobExpirationTimeout = TimeSpan.FromMinutes(jobExpirationTimeout);
            }

            public string StateName => SucceededState.StateName;

            public void Apply(ApplyStateContext context, IWriteOnlyTransaction transaction)
            {
                context.JobExpirationTimeout = JobExpirationTimeout;
            }

            public void Unapply(ApplyStateContext context, IWriteOnlyTransaction transaction)
            {
            }
        }

    }
}
