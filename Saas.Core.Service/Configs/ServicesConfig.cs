using Magicodes.ExporterAndImporter.Excel;
using Microsoft.Extensions.DependencyInjection;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Service.Base;

namespace Saas.Core.Service.Configs
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServicesConfig
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="services"></param>
        //public static void ConfigureServices(IServiceCollection services)
        //{
        //    //基础服务
        //    services.AddScoped<IExcelImporter, ExcelImporter>();
        //    services.AddScoped<IExcelExporter, ExcelExporter>();
        //    services.AddScoped<ExcelService>();

        //    //业务服务
        //    services.AddScoped<NoticeMessageService>();
        //    services.AddScoped<PregnantWomanEatMedicineRecordService>();
        //    services.AddScoped<WakeOnLanService>();
        //    services.AddScoped<RobotService>();

        //    //后台服务
        //    services.AddSingleton<IHostedService, QQRobotService>();
        //    services.AddSingleton<IHostedService, WeixinRobotService>();


        //}

        /// <summary>
        /// 添加需要依赖注入的服务
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(this IServiceCollection services)
        {
            //基础服务
            services.AddScoped<IExcelImporter, ExcelImporter>();
            services.AddScoped<IExcelExporter, ExcelExporter>();
            services.AddScoped<ExcelService>();
            services.AddTransient<RabbitMqService>();
            services.AddSingleton<EmailService>();
            services.AddScoped<IUnitWork, UnitWork>();

            //业务服务
            var serviceRegistrations =
                from type in typeof(ServicesConfig).Assembly.GetTypes()
                where type.IsClass && type.Namespace.IsNotBlank() && type.Namespace.Contains("Saas.Core.Service.Business") && type.Name.EndsWith("Service")
                select new { Implementation = type };
            foreach (var t in serviceRegistrations)
            {
                services.AddScoped(t.Implementation);
            }

            //后台服务
            var backgroundServiceRegistrations =
                from type in typeof(ServicesConfig).Assembly.GetTypes()
                where type.IsClass && type.Namespace.IsNotBlank() && type.Namespace.Contains("Saas.Core.Service.Background") && type.Name.EndsWith("Service")
                select new { Service = type.GetInterfaces().First(), Implementation = type };
            foreach (var t in backgroundServiceRegistrations)
            {
                services.AddSingleton(t.Service, t.Implementation);
            }

        }
    }
}
