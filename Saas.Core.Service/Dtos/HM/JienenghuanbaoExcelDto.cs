using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using OfficeOpenXml.Table;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Saas.Core.Service.Dtos.HM
{
    /// <summary>
    /// 
    /// </summary>
    [ExcelImporter(IsLabelingError = true)]
    [ExcelExporter(Name = "节能环保指标项", TableStyle = TableStyles.Light10, AutoFitAllColumn = true)]
    public class JienenghuanbaoExcelDto
    {

        /// <summary>
        /// 代码
        /// </summary>
        [Required(ErrorMessage = "代码不能为空")]
        [ImporterHeader(Name = "代码")]
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [ImporterHeader(Name = "指标名称")]
        public string Name { get; set; }

        /// <summary>
        /// 计量单位 
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [ImporterHeader(Name = "计量单位")]
        public string Unit { get; set; }

        /// <summary>
        /// 指标类型 公式计算=1,用户直接填写=2,用户填写月度值求和=3,前置业务维护=4
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [ImporterHeader(Name = "指标类型")]
        public TargetType TargetType { get; set; }


        /// <summary>
        /// 是否可比价
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [ImporterHeader(Name = "是否可比价")]
        public bool IsPriceComparison { get; set; }
    }

    /// <summary>
    /// 指标类型
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        /// 公式计算
        /// </summary>
        [Description("公式计算")]
        ExpCalculate = 1,

        /// <summary>
        /// 用户直接填写
        /// </summary>

        [Description("用户直接填写")]
        UserEdit = 2,

        /// <summary>
        /// 用户填写月度值求和
        /// </summary>
        [Description("用户填写月度值求和")]
        UserEditMonth = 3,

        /// <summary>
        /// 前置业务维护
        /// </summary>
        [Description("前置业务维护")]
        Preposition = 4,
    }
}
