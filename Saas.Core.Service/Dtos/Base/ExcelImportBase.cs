using System.ComponentModel.DataAnnotations;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 导入基类
    /// </summary>
    public class ExcelImportBase
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [Required]
        public string FilePath { get; set; }

        /// <summary>
        /// 是否覆盖数据
        /// </summary>
        public bool Override { get; set; }
    }
}
