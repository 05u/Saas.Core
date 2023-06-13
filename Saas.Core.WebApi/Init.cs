using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Utilities;

namespace Saas.Core.WebApi
{
    /// <summary>
    /// 初始化任务类
    /// </summary>
    public class Init
    {
        /// <summary>
        /// 生成
        /// </summary>
        public void InitUploadFilePath()
        {

            typeof(ModuleType).SelectList().ForEach(x =>
            {
                var directoryPath = Path.Combine("UpLoads", x.Code.ToString());
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

            });
            //typeof(DownLoadType).SelectList().ForEach(x =>
            //{
            //    var directoryPath = Path.Combine("Downloads", x.Code.ToString());
            //    if (!Directory.Exists(directoryPath))
            //    {
            //        Directory.CreateDirectory(directoryPath);
            //    }

            //});
            //typeof(SysTempType).SelectList().ForEach(x =>
            //{
            //    var directoryPath = Path.Combine("SysTemps", x.Code.ToString());
            //    if (!Directory.Exists(directoryPath))
            //    {
            //        Directory.CreateDirectory(directoryPath);
            //    }
            //});

        }

        /// <summary>
        /// 设置静态类属性
        /// </summary>
        public void SetConfig(IConfiguration configuration)
        {
            TokenHelper.secretKey = configuration.GetSection("SecretKey").Value;
            MqttHelper.address = configuration.GetSection("Mqtt:Address").Value;
            MqttHelper.username = configuration.GetSection("Mqtt:Username").Value;
            MqttHelper.password = configuration.GetSection("Mqtt:Password").Value;
        }
    }
}
