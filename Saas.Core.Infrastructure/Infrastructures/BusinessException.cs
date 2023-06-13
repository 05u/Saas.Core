namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// 业务提示信息异常
    /// </summary>
    public class BusinessException : Exception
    {



        public string ExceptionMessage { get; set; } = "程序发生业务异常:";


        public List<InnerBusinessException> InnerBusinessExceptionList { get; set; }

        public BusinessException(string message) : base(message)
        {
            ExceptionMessage += (Environment.NewLine + message);
        }

        public BusinessException(string message, Exception ex) : base(message, ex)
        {
            ExceptionMessage += (Environment.NewLine + message);
        }
        public BusinessException(string message, Exception ex, List<InnerBusinessException> list) : base(message, ex)
        {
            ExceptionMessage += (Environment.NewLine + message);
            InnerBusinessExceptionList = list;
            if (list?.Any() ?? false)
            {
                ExceptionMessage += (Environment.NewLine + "更多信息明细:");

            }
        }
        public BusinessException(string message, List<InnerBusinessException> list) : base(message)
        {
            ExceptionMessage += (Environment.NewLine + message);
            InnerBusinessExceptionList = list;

            if (list?.Any() ?? false)
            {
                ExceptionMessage += (Environment.NewLine + "更多信息明细:");

            }


        }
        public BusinessException(string message, List<string> list) : base(message)
        {
            ExceptionMessage += (Environment.NewLine + message);
            InnerBusinessExceptionList = list.ConvertAll(c => new
              InnerBusinessException
            { ErrorMessage = c });

            if (list?.Any() ?? false)
            {
                ExceptionMessage += (Environment.NewLine + "更多信息明细:");

            }


        }



        public BusinessException(InnerBusinessException innerException) : this(new List<InnerBusinessException> { innerException })
        {

        }
        public BusinessException(List<InnerBusinessException> list) : base()
        {
            InnerBusinessExceptionList = list;
            if (list?.Any() ?? false)
            {
                ExceptionMessage += (Environment.NewLine + "更多信息明细:");

            }
        }
    }



    public class InnerBusinessException
    {

        /// <summary>
        /// 
        /// </summary>
        public int ErrorCode { get; set; } = -1;


        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }




    }


}
