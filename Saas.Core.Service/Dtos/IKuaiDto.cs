using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 爱快通用返回
    /// </summary>
    public class IKuaiResult<T>
    {
        public int Result { get; set; }
        public string ErrMsg { get; set; }
        public T Data { get; set; }

    }

    /// <summary>
    /// 爱快通用入参
    /// </summary>
    public class CallInput<T>
    {
        [JsonProperty("func_name")]

        public string Func_name { get; set; }

        [JsonProperty("action")]

        public string Action { get; set; }

        [JsonProperty("param")]

        public T Param { get; set; }

    }

    /// <summary>
    /// 获取网关参数
    /// </summary>
    public class PPPoEGatewayParam
    {
        public string TYPE { get; set; }

        public string ORDER_BY { get; set; }

        public string ORDER { get; set; }

        [JsonProperty("vlan_internet")]

        public int Vlan_internet { get; set; }

        [JsonProperty("interface")]

        public string Interface { get; set; }

        [JsonProperty("limit")]

        public string Limit { get; set; }

    }

    /// <summary>
    /// 终端监控 查询入参
    /// </summary>
    public class MonitorLanip
    {
        public string TYPE { get; set; }

        public string ORDER_BY { get; set; }

        [JsonProperty("orderType")]

        public string OrderType { get; set; }

        [JsonProperty("limit")]

        public string Limit { get; set; }

        public string ORDER { get; set; }

    }

    /// <summary>
    /// 终端监控 出参
    /// </summary>
    public class MonitorLanipOutput
    {
        public List<MonitorLanipData> Data { get; set; }

        public int Total { get; set; }

    }

    public class MonitorLanipData
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string Ip_addr { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Mac地址
        /// </summary>
        public string Mac { get; set; }
    }

    /// <summary>
    /// 重新拨号参数
    /// </summary>
    public class PPPoERedial
    {
        [JsonProperty("id")]

        public string Id { get; set; }

    }

    /// <summary>
    /// 爱快登录入参
    /// </summary>
    public class LoginInput
    {
        public string Username { get; set; }
        public string Passwd { get; set; }

        public string Pass { get; set; }

        public bool Remember_password { get; set; }

    }

    public class PPPoEGateway
    {
        public List<Vlan_data> Vlan_data { get; set; }

        public int vlan_total { get; set; }
    }

    public class Vlan_data
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Vlan_name { get; set; }

        /// <summary>
        /// 网关
        /// </summary>
        public string Pppoe_gateway { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string Pppoe_ip_addr { get; set; }
    }
}
