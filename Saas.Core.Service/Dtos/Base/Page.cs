namespace Saas.Core.Service.Dtos
{
    public class Page
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Page()
        {
            PageIndex = 1;
            PageSize = 10;
        }
        public Page(int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }
    }
}
