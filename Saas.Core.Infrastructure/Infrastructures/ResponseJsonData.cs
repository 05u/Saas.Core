namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// 返回给前端的通用Json格式
    /// </summary>
    public class ResponseJsonData<T> where T : class
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public T Data { get; set; }
    }
}
