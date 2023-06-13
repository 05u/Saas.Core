using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 孕妇吃药记录
    /// </summary>
    [Table("bus_pregnant_woman_eat_medicine_record")]
    public class BusPregnantWomanEatMedicineRecord : BaseEntity
    {


        /// <summary>
        /// 开始时间(这个时间之后可以吃)
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间(要在这个时间之前吃完)
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 药品名称
        /// </summary>
        public string MedicineName { get; set; }

        /// <summary>
        /// 服用说明
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 是否吃完
        /// </summary>
        public bool? IsEatSuccess { get; set; }

        /// <summary>
        /// 是否吃完
        /// </summary>
        [NotMapped]
        public string IsEatSuccessText => IsEatSuccess == true ? "已吃完" : (StartTime > DateTime.Now ? "未到开吃时间" : (EndTime < DateTime.Now ? "逾期未吃" : "未吃"));

        /// <summary>
        /// 没吃原因说明
        /// </summary>
        public string FailRemark { get; set; }

        /// <summary>
        /// 提醒次数
        /// </summary>
        public int NoticeSecond { get; set; }

        /// <summary>
        /// 罚款金额
        /// </summary>
        public int? Fines { get; set; }


    }
}
