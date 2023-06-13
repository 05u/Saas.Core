/*******************************************************************************
* Copyright (C) Oxygen
* 
* Author: Oxygen
* Create Date: 2019/11/21 15:50:39
* Description: <Description>
* 
* Revision History:
* Date         Author               Description
*
*********************************************************************************/

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Saas.Core.Infrastructure.Utilities;

namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// web api统一数据返回格式处理
    /// </summary>
    public class WebApiResponseDataMiddleware : ActionFilterAttribute
    {
        /// <summary>
        /// OnResultExecuting
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var rawRes = descriptor.ControllerTypeInfo.GetCustomAttributes(true).OfType<RawResponseFilter>().Any()
                  || descriptor.MethodInfo.GetCustomAttributes(true).OfType<RawResponseFilter>().Any();
                if (rawRes)
                {
                    base.OnResultExecuting(context);
                    return;
                }
            }

            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                context.Result = new ObjectResult(ToResponse(objectResult.StatusCode ?? StatusCodes.Status200OK, objectResult.Value));
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(ToNotFound());
            }
            else if (context.Result is ContentResult)
            {
                //原样返回，不做处理
            }
            else if (context.Result is StatusCodeResult)
            {
                var result = context.Result as StatusCodeResult;
                context.Result = new ObjectResult(ToResponse(result.StatusCode, string.Empty));
            }
            else
            {
                base.OnResultExecuting(context);
            }
        }

        /// <summary>
        /// 未找到
        /// </summary>
        /// <returns></returns>
        private ResponseModel<object> ToNotFound()
        {
            var responseData = new ResponseModel<object>();
            //responseData.Code = StatusCodes.Status404NotFound;
            responseData.Message = "未找到资源";
            return responseData;
        }

        /// <summary>
        /// 返回指定格式的数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value"></param>
        /// <param name="message">提示消息</param>
        /// <returns></returns>
        private ResponseModel<object> ToResponse(int code, object value, string message = "请求成功")
        {
            var responseData = new ResponseModel<object>();
            //responseData.Code = code;
            responseData.Data = value;
            responseData.Message = message;
            responseData.Success = true;
            return responseData;
        }
    }
}
