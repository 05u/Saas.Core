using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using OfficeOpenXml.Table;
using System.ComponentModel.DataAnnotations;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    [ExcelImporter(IsLabelingError = true)]
    [ExcelExporter(Name = "小爱设备", TableStyle = TableStyles.Light10, AutoFitAllColumn = true)]
    public class MdmXiaoaiSpeakerDto
    {

        #region 框架通用
        /// <summary>
        /// 主键
        /// </summary>
        [IEIgnore(IsImportIgnore = true, IsExportIgnore = true)]
        public string Id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [IEIgnore(IsImportIgnore = true, IsExportIgnore = true)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [IEIgnore(IsImportIgnore = true, IsExportIgnore = true)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [IEIgnore(IsImportIgnore = true, IsExportIgnore = true)]
        public string CreateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [IEIgnore(IsImportIgnore = true, IsExportIgnore = true)]
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [IEIgnore(IsImportIgnore = true, IsExportIgnore = true)]
        public string ModifyBy { get; set; }
        #endregion

        /// <summary>
        /// 代码
        /// </summary>
        [Required(ErrorMessage = "代码不能为空")]
        [ImporterHeader(Name = "代码")]
        [ExporterHeader(DisplayName = "代码", IsAutoFit = true)]
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [ImporterHeader(Name = "名称")]
        [ExporterHeader(DisplayName = "名称", IsAutoFit = true)]
        public string Name { get; set; }

        #region 小米平台字段
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "DeviceID不能为空")]
        [ImporterHeader(Name = "小米平台字段-DeviceID")]
        [ExporterHeader(DisplayName = "小米平台字段-DeviceID", IsAutoFit = true)]
        public string DeviceID { get; set; }
        /// <summary>
        ///  
        /// </summary>
        [ImporterHeader(Name = "小米平台字段-SerialNumber")]
        [ExporterHeader(DisplayName = "小米平台字段-SerialNumber", IsAutoFit = true)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ImporterHeader(Name = "小米平台字段-Address")]
        [ExporterHeader(DisplayName = "小米平台字段-Address", IsAutoFit = true)]
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ImporterHeader(Name = "小米平台字段-MiotDID")]
        [ExporterHeader(DisplayName = "小米平台字段-MiotDID", IsAutoFit = true)]
        public string MiotDID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ImporterHeader(Name = "小米平台字段-Hardware")]
        [ExporterHeader(DisplayName = "小米平台字段-Hardware", IsAutoFit = true)]
        public string Hardware { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ImporterHeader(Name = "小米平台字段-Mac")]
        [ExporterHeader(DisplayName = "小米平台字段-Mac", IsAutoFit = true)]
        public string Mac { get; set; }
        #endregion
    }
}
