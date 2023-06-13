namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 库存信息
    /// </summary>
    public class StockInfo
    {
        /// <summary>
        /// 馆藏位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 索书号
        /// </summary>
        public string FindNo { get; set; }

        /// <summary>
        /// 馆藏总数
        /// </summary>
        public int AllCount { get; set; }

        /// <summary>
        /// 已借出数
        /// </summary>
        public int OutCount { get; set; }

        /// <summary>
        /// 可借数量
        /// </summary>
        public int InCount => AllCount - OutCount;
    }
}
