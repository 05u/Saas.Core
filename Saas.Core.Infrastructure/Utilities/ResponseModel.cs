namespace Saas.Core.Infrastructure.Utilities
{
    public class ResponseModel : ResponseModel<object>
    {
        public static ResponseModel CreateSuccess()
        {
            return new ResponseModel { Success = CodeDes.SUCCESS };
        }
        public new static ResponseModel CreateError(string message)
        {
            return new ResponseModel
            {
                Success = CodeDes.ERROR,
                Message = message
            };
        }
    }

    /// <summary>
    /// 标准json交互响应格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 数据体
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 附加消息
        /// </summary>
        public string Message { get; set; }

        public bool IsSuccess()
        {
            return Success == CodeDes.SUCCESS;
        }

        public bool IsError()
        {
            return Success == CodeDes.ERROR;
        }

        /// <summary>
        /// 创建一个<see cref="ResponseModel{T}.data"/>为null的ResponseModel
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">附加消息</param>
        /// <returns></returns>
        public static ResponseModel<T> CreateEmptyDataResponse(bool code, string message)
        {
            return new ResponseModel<T>
            {
                Success = code,
                Message = message
            };
        }


        /// <summary>
        /// 创建一个code为1的<see cref="ResponseModel{T}"/>对象
        /// </summary>
        /// <param name="data">数据体</param>
        /// <returns></returns>
        public static ResponseModel<T> CreateSuccess(T data)
        {
            return new ResponseModel<T>
            {
                Success = CodeDes.SUCCESS,
                Data = data,
                Message = "请求成功",
            };
        }

        /// <summary>
        /// 创建一个code为1的<see cref="ResponseModel{T}"/>对象
        /// </summary>
        /// <param name="message">附加消息</param>
        /// <returns></returns>
        public static ResponseModel<T> CreateError(string message)
        {
            return new ResponseModel<T>
            {
                Success = CodeDes.ERROR,
                Message = message
            };
        }


        /// <summary>
        /// 创建一个<see cref="ResponseModel{T}"/>对象
        /// </summary>
        /// <param name="data">数据体</param>
        /// <param name="code">状态码</param>
        /// <param name="message">附加消息</param>
        /// <returns></returns>
        public static ResponseModel<T> Create(T data, bool code, string message)
        {
            return new ResponseModel<T>
            {
                Success = code,
                Data = data,
                Message = message
            };
        }


        public static ResponseModel CreatePageResult(object data, int count)
        {
            return new ResponseModel
            {
                Success = CodeDes.SUCCESS,
                Data = new
                {
                    data,
                    count
                }
            };
        }
    }

    /// <summary>
    /// <see cref="ResponseModel{T}"/> code描述，100000以内保留，业务子系统使用100000以上
    /// </summary>
    public static class CodeDes
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        public const bool SUCCESS = true;

        /// <summary>
        /// 处理失败
        /// </summary>
        public const bool ERROR = false;
    }
    /// <summary>
    /// 分页返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageData<T>
    {
        /// <summary>
        /// 查询的数据
        /// </summary>
        public IEnumerable<T> DataList { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
