
using Exceptionless;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Saas.Core.Infrastructure.Utilities;
using System.Net;
using System.Text.Json;

namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// web api 全局异常日志处理管道
    /// </summary>
    public class WebApiGlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<WebApiGlobalExceptionMiddleware> logger;


        private readonly IConfiguration configuration;
        private IHttpClientFactory httpClientFactory;

        /// <summary>
        /// ctor
        /// </summary>
        public WebApiGlobalExceptionMiddleware(RequestDelegate next,
            ILogger<WebApiGlobalExceptionMiddleware> logger,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            this.next = next;
            this.logger = logger;
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// invoke
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await WriteExceptionAsync(context, ex).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 处理异常数据，以及返回数据格式
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task WriteExceptionAsync(HttpContext context, Exception exception)
        {
            var baseException = exception.GetBaseException();

            //记录所有错误到Exceptionless
            bool isOpenExceptionLess = Convert.ToBoolean(configuration["Exceptionless:IsActive"]);
            if (isOpenExceptionLess)
            {
                baseException.ToExceptionless().Submit();

                //非业务类异常发送消息提醒管理员处理
                if (!(baseException is BusinessException))
                {
#if DEBUG
                    Console.WriteLine($"{DateTime.Now} WebApi发生非业务类异常!");
#else
                    var client = httpClientFactory.CreateClient(HttpClientConst.CommonClient);
                    await client.GetAsync($"http://localhost:5000/api/MessageGateway/GetPublishNoticeMessageTo?text={"WebApi发生非业务类异常,请及时处理!"}&qq_Receiver={configuration["AdminInfo:QQ"]}&wx_Wxid={configuration["AdminInfo:Weixin"]}");
#endif
                }
            }


            //记录日志
            logger.LogError(exception, $"异常消息:{baseException.Message}");

            //返回友好的提示
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.OK;
            var jsonString = string.Empty;
            //状态码
            var code = 0;
            if (exception is UnauthorizedAccessException)
            {
                code = (int)HttpStatusCode.Unauthorized;
                var json = new ResponseModel<string>
                {
                    //Code = code,
                    Message = exception.GetBaseException().Message,
                    Data = string.Empty
                };
                jsonString = JsonSerializer.Serialize(json, BaseWebService.GetOxygenJsonOptions());
            }

            else if (exception is BusinessException)
            {
                //code = (int)HttpStatusCode.BadRequest;
                //code = (int)exception.ErrorCode;
                var businessException = (exception as BusinessException);
                var json = new ResponseModel<List<InnerBusinessException>>
                {
                    //Code = (int)businessException.ErrorCode,
                    Message = businessException?.ExceptionMessage,
                    Data = businessException?.InnerBusinessExceptionList
                };
                jsonString = JsonSerializer.Serialize(json, BaseWebService.GetOxygenJsonOptions());
            }

            else if (exception is Exception)
            {
                code = (int)HttpStatusCode.InternalServerError;
                var json = new ResponseModel<string>
                {
                    //Code = code,
                    Message = exception.GetBaseException().Message,
                    Data = string.Empty
                };
                jsonString = JsonSerializer.Serialize(json, BaseWebService.GetOxygenJsonOptions());

            }




            await response.WriteAsync(jsonString).ConfigureAwait(false);
        }
    }
}
