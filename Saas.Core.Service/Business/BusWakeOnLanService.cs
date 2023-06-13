using Microsoft.AspNetCore.Http;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Data.Respository;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using System.Net.Sockets;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 网络唤醒服务
    /// </summary>
    public class BusWakeOnLanService : Repository<BusWakeOnLanRecord>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        public BusWakeOnLanService(MainDbContext context, IHttpContextAccessor httpContextAccessor, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 唤醒指定主机
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public async Task WOL(string mac, string remark)
        {

            try
            {
                if (mac.IsBlank())
                {
                    throw new BusinessException("MAC地址必填,请检查!");
                }
                WakeUpCore(FormatMac(mac));
                await InsertAsync(new BusWakeOnLanRecord()
                {
                    Mac = mac,
                    Remark = remark,
                    SenderIP = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress.ToString()
                });
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }


        }

        /// <summary>
        /// 发送魔术包
        /// </summary>
        /// <param name="mac"></param>
        private static void WakeUpCore(byte[] mac)
        {
            //发送方法是通过UDP
            using UdpClient client = new UdpClient();
            //Broadcast内容为：255,255,255,255.广播形式，所以不需要IP
            client.Connect(System.Net.IPAddress.Broadcast, 50000);
            //下方为发送内容的编制，6遍“FF”+17遍mac的byte类型字节。
            byte[] packet = new byte[17 * 6];
            for (int i = 0; i < 6; i++) packet[i] = 0xFF;
            for (int i = 1; i <= 16; i++) for (int j = 0; j < 6; j++) packet[i * 6 + j] = mac[j];
            //唤醒动作
            int result = client.Send(packet, packet.Length);
        }

        /// <summary>
        /// 转换MAC地址
        /// </summary>
        /// <param name="macInput"></param>
        /// <returns></returns>
        private static byte[] FormatMac(string macInput)
        {
            byte[] mac = new byte[6];
            //消除MAC地址中的“-”符号
            //string[] sArray = str.Split('-');
            string str = macInput.Replace("-", string.Empty).Replace(":", string.Empty);
            if (str.Length != 12)
            {
                throw new BusinessException("填写的MAC地址错误,请检查!");
            }
            //mac地址从string转换成byte
            for (var i = 0; i < 6; i++)
            {
                //var byteValue = Convert.ToByte(sArray[i], 16); 
                var byteValue = Convert.ToByte(str.Substring(i * 2, 2), 16);
                mac[i] = byteValue;
            }
            return mac;
        }
    }
}
