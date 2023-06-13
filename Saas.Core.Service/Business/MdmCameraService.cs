using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Utilities;
using System.Net;
using XiaoFeng.Onvif;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 摄像头
    /// </summary>
    public class MdmCameraService : Repository<MdmCamera>
    {
        private readonly ILogger<MdmCameraService> _logger;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// ctor
        /// </summary>
        public MdmCameraService(MainDbContext context, ILogger<MdmCameraService> logger, ICurrentUserService currentUserService, IConfiguration configuration) : base(context, currentUserService)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Onvif测试
        /// </summary>
        /// <returns></returns>
        public async Task OnvifTest()
        {

            var ip = "192.168.8.111";
            var port = 8088;
            var user = "admin";
            var pass = "admin";
            var iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            //设备扫描
            var resu = await DeviceService.DiscoveryOnvif(3);
            //设备时间
            var onvifUTCDateTime = await DeviceService.GetSystemDateAndTime(iPEndPoint);
            //设备信息
            var info = await DeviceService.GetDeviceInformation(iPEndPoint, user, pass, onvifUTCDateTime);
            //设备能力
            var abilities = await DeviceService.GetCapabilities(iPEndPoint);
            //token信息
            var tokens = await MediaService.GetProfiles(iPEndPoint, user, pass, onvifUTCDateTime);
            //流地址
            var streamUri = await MediaService.GetStreamUri(iPEndPoint, user, pass, onvifUTCDateTime, tokens[0]);
            //快照地址
            var img = await MediaService.GetSnapshotUri(iPEndPoint, user, pass, onvifUTCDateTime, tokens[0]);
            //云台状态
            await PTZService.GetStatus(iPEndPoint, user, pass, onvifUTCDateTime, tokens[0]);
            //设置home
            await PTZService.SetHomePosition(iPEndPoint, user, pass, onvifUTCDateTime, tokens[0]);
            //绝对移动
            await PTZService.AbsoluteMove(iPEndPoint, user, pass, onvifUTCDateTime, tokens[0], 0, 0);
            //继续移动
            await PTZService.ContinuousMove(iPEndPoint, user, pass, onvifUTCDateTime, tokens[0], 0.6, 0.2, 1);
            //相对移动
            await PTZService.RelativeMove(iPEndPoint, user, pass, onvifUTCDateTime, tokens[0], 0.8, 0.5, 0.5);
            //回归home
            await PTZService.GotoHomePosition(iPEndPoint, user, pass, onvifUTCDateTime, tokens[0], 0.3, 1, 1);
        }

        /// <summary>
        /// 设置摄像头位置
        /// </summary>
        /// <param name="ip">Onvif设备IP地址</param>
        /// <param name="port">Onfif设备通信端口号,不传默认80</param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public async Task SetCameraArea(string ip, int? port, string user, string pass, double x, double y)
        {
            var iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port ?? 80);
            var onvifUTCDateTime = await DeviceService.GetSystemDateAndTime(iPEndPoint);
            var tokens = await MediaService.GetProfiles(iPEndPoint, user, pass, onvifUTCDateTime);
            await PTZService.AbsoluteMove(iPEndPoint, user, pass, onvifUTCDateTime, tokens[0], x, y);

        }

        /// <summary>
        /// 设置摄像头开关状态
        /// </summary>
        /// <param name="status">true:开 false:关</param>
        /// <param name="name">所属家庭名称(非必须)+摄像头名称(全部则操作所有)</param>
        /// <returns></returns>
        public async Task<string> SetCameraStatus(bool status, string name)
        {
            var list = await Queryable()
                .WhereIf(name != "全部", c => name.Contains(c.Name) && (c.HomeName == null || name.Contains(c.HomeName)))
                .ToListAsync();
            var result = "";
            var encryptionKey = _configuration.GetRequiredSection("EncryptionKey")?.Value;
            if (status)
            {
                foreach (var item in list)
                {
                    try
                    {
                        if (item.Pass.IsNotBlank() && encryptionKey.IsNotBlank())
                        {
                            item.Pass = AESEncryption.DecryptAES(item.Pass, encryptionKey);
                        }
                        await SetCameraArea(item.Ip, item.Port, item.User, item.Pass, item.OnAreaX, item.OnAreaY);
                    }
                    catch (Exception ex)
                    {
                        result += ex.Message;
                    }
                }
            }
            else
            {
                foreach (var item in list)
                {
                    try
                    {
                        if (item.Pass.IsNotBlank() && encryptionKey.IsNotBlank())
                        {
                            item.Pass = AESEncryption.DecryptAES(item.Pass, encryptionKey);
                        }
                        await SetCameraArea(item.Ip, item.Port, item.User, item.Pass, item.OffAreaX, item.OffAreaY);
                    }
                    catch (Exception ex)
                    {
                        result += ex.Message;
                    }
                }
            }
            return result;
        }
    }
}
