using Saas.Core.Infrastructure.Extentions;

namespace Saas.Core.Infrastructure.Dtos
{
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PagedResult()
        {
            Datas = new List<T>();
        }

        /// <summary>
        /// ctor with params
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示数量</param>
        public PagedResult(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Records { set; get; }
        /// <summary>
        /// 当前页的所有项
        /// </summary>
        public IList<T> Datas { set; get; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { set; get; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { set; get; }
        /// <summary>
        /// 页总数
        /// </summary>
        public int TotalPage { get { return Records.CeilingDivide(PageSize); } }
    }
}
