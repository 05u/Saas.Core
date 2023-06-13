using System.ComponentModel.DataAnnotations;

namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageBaseFilter<T> where T : BaseFilter
    {

        /// <summary>
        ///分页请求模型
        /// </summary>
        public PageBaseFilter()
        {
            // this.Search = default ( T );
        }
        /// <summary>
        ///搜索的条件
        /// </summary>
        public T Search
        {
            get;
            set;
        }
        /// <summary>
        /// 页码
        /// </summary>
        [Required(ErrorMessage = "{0} 必须要传.")]
        [Range(1, int.MaxValue, ErrorMessage = "{0}必须是大于1的整数")]
        public int PageIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 每页记录数
        /// </summary>
        [Required(ErrorMessage = "{0} 必须要传.")]
        [Range(1, int.MaxValue, ErrorMessage = "{0}必须是大于1的整数")]
        public int PageSize
        {
            get;
            set;
        }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; } = "Id";

        /// <summary>
        /// 排序方式
        /// </summary>
        public string SortType { get; set; } = "asc";



    }
}
