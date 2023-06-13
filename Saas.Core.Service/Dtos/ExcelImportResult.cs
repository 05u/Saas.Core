namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class ExcelImportResult
    {
        /// <summary>
        /// 是否导入成功
        /// </summary>
        public bool ImportSuccess { get; set; } = true;

        /// <summary>
        /// 导入成功的数量
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorExcelPath { get; set; }
    }
}
