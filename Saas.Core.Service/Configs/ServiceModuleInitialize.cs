using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saas.Core.Data.Context;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saas.Core.Infrastructure.Utilities.Constants;

namespace Saas.Core.Service.Configs
{
    /// <summary>
    /// 依赖注入模块初始化
    /// </summary>
    public static class ServiceModuleInitialize
    {
        /// <summary>
        /// 添加数据库初始配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddHealthChecks().AddDbContextCheck<MainDbContext>();
            var dbType = configuration.GetValue<string>("ConnectionStrings:DbType");
            if (dbType.IsEqual(DatabaseConstValue.MySqlDbType))
            {
                //services.AddDbContext<MainDbContext>(options => options.UseMySql(configuration.GetConnectionString("MySql"), MySqlServerVersion.LatestSupportedServerVersion));
                // Replace with your server version and type.
                // Use 'MariaDbServerVersion' for MariaDB.
                // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
                // For common usages, see pull request #1233.
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 24));
                //var serverVersion = new MariaDbServerVersion(new Version(10, 5, 0));
                // Replace 'YourDbContext' with the name of your own DbContext derived class.
                services.AddDbContext<MainDbContext>(options =>
                {
                    options.UseMySql(configuration.GetConnectionString("MySql"), serverVersion, options => options.EnableRetryOnFailure()).EnableDetailedErrors();
                    // The following three options help with debugging, but should
                    // be changed or removed for production.
                    //.LogTo(Console.WriteLine, LogLevel.Information)
                    //.EnableSensitiveDataLogging()
                });
            }
            else if (dbType.IsEqual(DatabaseConstValue.SqlServerDbType))
            {
                services.AddDbContext<MainDbContext>(options =>
                {
                    //启用显示敏感数据
                    options.EnableSensitiveDataLogging(true);
                    options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
                });
            }
            //else if (dbType.IsEqual(DatabaseConstValue.PostgresSqlDbType))
            //{
            //    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            //    services.AddDbContext<MainDbContext>(

            //        options => { options.UseNpgsql(configuration.GetConnectionString("PostgresSql")); }
            //        );
            //}
            else
            {
                throw new BusinessException($"没有找到对应类型的数据库配置{dbType}");
            }
        }
    }
}
