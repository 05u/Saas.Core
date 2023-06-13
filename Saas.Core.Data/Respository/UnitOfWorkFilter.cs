using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Attributes;

namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// 工作单元拦截器
    /// </summary>
    public class UnitOfWorkFilter : ActionFilterAttribute
    {
        private readonly IUnitWork _unitWork;


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public UnitOfWorkFilter(IServiceProvider serviceProvider)
        {
            _unitWork = serviceProvider.GetService<IUnitWork>();
        }

        /// <summary>
        /// 进控制器前
        /// </summary>
        /// <param name="context"></param>

        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var needUnitWork = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                    .Any(a => a.GetType().Equals(typeof(UnitWorkAttribute)));
                if (needUnitWork)
                {
                    await _unitWork.BeginTransactionAsync();
                }
            }

            base.OnActionExecuting(context);
        }
        /// <summary>
        /// 进控制器后
        /// </summary>
        /// <param name="context"></param>
        public async override void OnActionExecuted(ActionExecutedContext context)
        {

            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var needUnitWork = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                    .Any(a => a.GetType().Equals(typeof(UnitWorkAttribute)));
                if (needUnitWork)
                {
                    if (context.Exception == null)
                    {
                        await _unitWork.CommitAsync();

                    }
                    else
                    {
                        await _unitWork.RollbackAsync();
                    }

                }

            }

            base.OnActionExecuted(context);
        }


    }
}
