using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Saas.Core.Infrastructure.Utilities;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// api 认证返回数据格式处理
    /// </summary>
    public class ApiAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public ApiAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) :
            base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// HandleAuthenticateAsync
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 认证处理
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status200OK;
            var json = new ResponseModel<string>
            {
                Data = string.Empty,
                Message = "很抱歉，请确保已经登录!",
                //Code = StatusCodes.Status401Unauthorized
            };
            await Response.WriteAsync(JsonSerializer.Serialize(json, BaseWebService.GetOxygenJsonOptions()));
        }

        /// <summary>
        /// 无权访问
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status200OK;
            var json = new ResponseModel<string>
            {
                Data = string.Empty,
                Message = "很抱歉，您无权访问该接口!",
                //Code = StatusCodes.Status403Forbidden
            };
            await Response.WriteAsync(JsonSerializer.Serialize(json, BaseWebService.GetOxygenJsonOptions()));
        }
    }
}
