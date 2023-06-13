using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 交易所
    /// </summary>
    public class MfcMarketsOutput
    {
        /// <summary>
        /// 交易所名称(ID)
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// 交易所全称
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// 交易所官网链接
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// (旧)通过人工干预统计的交易量(USD)
        /// </summary>
        public string Volume { get; set; }

        /// <summary>
        /// 交易所上报交易量(USD)
        /// </summary>
        public string ReportedVolume { get; set; }

        /// <summary>
        /// 状态: [enable, disable]. disable为停止更新数据
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 是否接入K线数据
        /// </summary>
        public string Kline { get; set; }

        /// <summary>
        /// 是否支持现货
        /// </summary>
        public string Spot { get; set; }

        /// <summary>
        /// 是否支持期货
        /// </summary>
        public string Futures { get; set; }
    }

    /// <summary>
    /// 币种
    /// </summary>
    public class MfcSymbolsOutput
    {
        /// <summary>
        /// 币种名称（ID）
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// 币种符号
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 币种全称
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// 图标链接
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 通过人工干预统计的交易量(USD)
        /// </summary>
        public string VolumeUsd { get; set; }

        /// <summary>
        /// 状态: [enable, disable]. disable为停止更新数据
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 币种市值
        /// </summary>
        public string MarketCapUsd { get; set; }

        /// <summary>
        /// 流通量
        /// </summary>
        public string AvailableSupply { get; set; }

        /// <summary>
        /// 发行总量
        /// </summary>
        public string TotalSupply { get; set; }
    }

    /// <summary>
    /// 币种价格
    /// </summary>
    public class MfcPriceOutput
    {
        /// <summary>
        /// 币种名称
        /// </summary>
        public string s { get; set; }

        ///// <summary>
        ///// 币种符号
        ///// </summary>
        //public string S { get; set; }

        /// <summary>
        /// 价格(USD)
        /// </summary>
        public string u { get; set; }

        /// <summary>
        /// 价格(BTC)
        /// </summary>
        public string b { get; set; }

        /// <summary>
        /// 交易量(USD)
        /// </summary>
        public string v { get; set; }

        /// <summary>
        /// 时间戳(毫秒)
        /// </summary>
        public string T { get; set; }

        /// <summary>
        /// 24小时涨跌幅
        /// </summary>
        public string c { get; set; }

        /// <summary>
        /// 24小时最高价
        /// </summary>
        public string h { get; set; }

        /// <summary>
        /// 24小时最低价
        /// </summary>
        public string l { get; set; }
    }
}
