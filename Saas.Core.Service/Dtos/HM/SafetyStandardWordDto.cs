using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Service.Dtos.HM
{
    /// <summary>
    /// 安全标准化word中的表格
    /// </summary>
    public class SafetyStandardWordDto
    {

        /// <summary>
        /// 一级要素标编号
        /// </summary>
        public string Factor1No { get; set; }

        /// <summary>
        /// 二级要素标编号
        /// </summary>
        public string Factor2No { get; set; }

        /// <summary>
        /// 三级要素标编号
        /// </summary>
        public string Factor3No { get; set; }

        /// <summary>
        /// 四级要素标编号
        /// </summary>
        public string Factor4No { get; set; }

        /// <summary>
        /// 一级要素标题
        /// </summary>
        public string Factor1 { get; set; }

        /// <summary>
        /// 二级要素标题
        /// </summary>
        public string Factor2 { get; set; }

        /// <summary>
        /// 三级要素标题
        /// </summary>
        public string Factor3 { get; set; }

        /// <summary>
        /// 检查项名称 (基本规范要求)
        /// </summary>
        public string LibraryName { get; set; }

        /// <summary>
        /// 企业创建标准要求
        /// </summary>
        public string CreateStandard { get; set; }

        /// <summary>
        /// 检查项分值 
        /// </summary>
        public decimal LibraryScore { get; set; }

        /// <summary>
        /// 检查方法 (评分方式)
        /// </summary>
        public string CheckMethod { get; set; }

        /// <summary>
        /// 企业内部分工
        /// </summary>
        public string Insider { get; set; }

        /// <summary>
        /// 检查情况1
        /// </summary>
        public string CheckSituation1 { get; set; }

        /// <summary>
        /// 检查情况2
        /// </summary>
        public string CheckSituation2 { get; set; }
    }

    public class Factor4Info
    {
        public string Factor3No { get; set; }

        public int Factor4LastNo { get; set; }

        /// <summary>
        /// 检查项名称 (基本规范要求)
        /// </summary>
        public string LibraryName { get; set; }
    }
}
