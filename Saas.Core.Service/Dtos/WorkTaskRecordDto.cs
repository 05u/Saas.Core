using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using OfficeOpenXml.Table;
using Saas.Core.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 工作任务记录
    /// </summary>
    [ExcelImporter(IsLabelingError = true)]
    [ExcelExporter(Name = "工作任务记录", TableStyle = TableStyles.Light10, AutoFitAllColumn = true)]
    public class WorkTaskRecordDto
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
        /// 微信Id
        /// </summary>
        [ImporterHeader(Name = "微信Id")]
        [ExporterHeader(DisplayName = "微信Id", IsAutoFit = true)]
        [IEIgnore(IsImportIgnore = true, IsExportIgnore = false)]
        public string Wxid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        [ImporterHeader(Name = "姓名")]
        [ExporterHeader(DisplayName = "姓名", IsAutoFit = true)]
        public string Name { get; set; }

        /// <summary>
        /// 上班日期
        /// </summary>
        [Required(ErrorMessage = "上班日期不能为空")]
        [ImporterHeader(Name = "上班日期")]
        [ExporterHeader(DisplayName = "上班日期", IsAutoFit = true)]
        public DateTime WorkDate { get; set; }

        /// <summary>
        /// 班次类型
        /// </summary>
        [Required(ErrorMessage = "班次类型不能为空")]
        [ImporterHeader(Name = "班次类型")]
        [ExporterHeader(DisplayName = "班次类型", IsAutoFit = true)]
        public WorkClassType WorkClassType { get; set; }
    }
}
