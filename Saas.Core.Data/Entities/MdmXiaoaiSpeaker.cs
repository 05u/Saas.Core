using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Core.Data.Entities
{
    /// <summary>
    /// 小爱音箱
    /// </summary>
    [Table("mdm_xiaoai_speaker")]
    public class MdmXiaoaiSpeaker : BaseEntity
    {

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        #region 小米平台字段
        /// <summary>
        /// 
        /// </summary>
        public string DeviceID { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MiotDID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Hardware { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Mac { get; set; }
        #endregion


    }


}
