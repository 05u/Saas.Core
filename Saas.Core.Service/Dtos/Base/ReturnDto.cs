namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// Service到Controller通用返回包装
    /// </summary>
    public class ReturnDto
    {

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
