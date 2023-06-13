using System.ComponentModel.DataAnnotations;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 提交吃药记录入参
    /// </summary>
    public class SubmitInput
    {
        /// <summary>
        /// 是否吃完(0=没吃,1-吃了)
        /// </summary>
        [Required]
        public bool? IsEatSuccess { get; set; }

        /// <summary>
        /// 没吃原因说明(没吃的话需要填写)
        /// </summary>
        public string FailRemark { get; set; }
    }
}
