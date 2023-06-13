using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Dtos
{
    public class GetParkCouponDto
    {
        public string PlanNo { get; set; }
        public string UserId { get; set; }

        public string Mobile { get; set; }

        public string ReqSource { get; set; } = "JTC_A";

        public string Charset { get; set; } = "UTF-8";

        public string SignType { get; set; } = "MD5";

        public string Version { get; set; } = "V1.4";

    }

    public class GetParkCouponResultDto
    {
        public string ResultCode { get; set; }
        public string Message { get; set; }
        public string Obj { get; set; }
        public string ReqId { get; set; }
        public string Success { get; set; }

    }



    /// <summary>
    /// 获取停车场信息入参
    /// </summary>
    public class GetParkInfoInputDto
    {
        public string UserId { get; set; } = "";

        public double Latitude { get; set; } = 0;

        public double Longitude { get; set; } = 0;
        public string ParkCode { get; set; }
        public string Platform { get; set; } = "";

    }

    /// <summary>
    /// 捷停车基础返回
    /// </summary>
    public class ParkInfoBase<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
    }

    /// <summary>
    /// 捷停车 停车场信息
    /// </summary>
    public class GetParkInfoOutputDto
    {
        public string ParkName { get; set; }
        public string ParkCode { get; set; }

        public string Address { get; set; }
        public string Canton { get; set; }

        /// <summary>
        /// 空车位
        /// </summary>
        public int EmptySpaces { get; set; }

        /// <summary>
        /// 总车位
        /// </summary>
        public int TotalSpaces { get; set; }

        public string ParkSource { get; set; }


        public string ParkId { get; set; }

        /// <summary>
        /// 优惠列表
        /// </summary>
        public List<Favour> FavourList { get; set; }

    }

    /// <summary>
    /// 捷停车 优惠信息
    /// </summary>
    public class Favour
    {

        /// <summary>
        /// 总库存
        /// </summary>
        public int? AllTotalStock { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public string FavourAmount { get; set; }

        public string FavourType { get; set; }

        public string FavourNo { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }

        public string PlanNo { get; set; }

        public int? FavourCount { get; set; }
        public int? TotalStock { get; set; }
        public string ParkCode { get; set; }


        public int? Stock { get; set; }

    }

    /// <summary>
    /// FRP查询出参
    /// </summary>
    public class FrpOutput
    {
        public List<FrpProxie> Proxies { get; set; }
    }


    public class FrpProxie
    {
        public string Name { get; set; }

        public FrpConf Conf { get; set; }

        public int Today_traffic_in { get; set; }
        public int Today_traffic_out { get; set; }

        public int Cur_conns { get; set; }
        public string Last_start_time { get; set; }

        public string Last_close_time { get; set; }

        /// <summary>
        /// 状态 在线online 离线offline
        /// </summary>
        public string Status { get; set; }


    }

    public class FrpConf
    {
        /// <summary>
        /// 远程端口
        /// </summary>
        public string Remote_port { get; set; }
    }

    public class HolidayList
    {
        public List<Holiday> List { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
    }

    public class Holiday
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Date { get; set; }

        /// <summary>
        /// 1工作日 2非工作日
        /// </summary>
        public int Workday { get; set; }
    }

}
