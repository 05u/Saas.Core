using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 血压监测记录
    /// </summary>
    [Table("bus_blood_pressure_record")]
    public class BusBloodPressureRecord : BaseEntity
    {
        /// <summary>
        /// 高压 mmHg
        /// </summary>
        public int HighPressure { get; set; }

        /// <summary>
        /// 低压 mmHg
        /// </summary>
        public int LowPressure { get; set; }

        /// <summary>
        /// 脉搏 次/分
        /// </summary>
        public int Pulse { get; set; }

        /// <summary>
        /// 血压测试结果
        /// </summary>
        public BloodPressureResult BloodPressureResult { get; set; }

        /// <summary>
        /// 血压测试时段
        /// </summary>
        public BloodPressureTimeFrame BloodPressureTimeFrame { get; set; }
    }

    /// <summary>
    /// 血压测试结果
    /// </summary>
    public enum BloodPressureResult
    {
        /// <summary>
        /// 通过 低于135/85
        /// </summary>
        [Description("通过")]
        Pass = 1,

        /// <summary>
        /// 低风险 135/85-140/90
        /// </summary>
        [Description("低风险")]
        LowRisk = 2,

        /// <summary>
        /// 高风险 高于140/90
        /// </summary>
        [Description("高风险")]
        HighRisk = 3,
    }

    /// <summary>
    /// 血压测试时段
    /// </summary>
    public enum BloodPressureTimeFrame
    {
        /// <summary>
        /// 早晨
        /// </summary>
        Morning = 1,

        /// <summary>
        /// 晚上
        /// </summary>
        Evening = 2,
    }
}
