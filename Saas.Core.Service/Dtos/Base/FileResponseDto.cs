
namespace Saas.Core.Service.Dtos
{
    /// <summary>
    /// 文件上传返回对象
    /// </summary>
    public class FileResponseDto
    {
        /// <summary>
        /// 是否完成了文件上传
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// 文件的绝对路径
        /// </summary>
        public string FileUrl { get; set; }
    }
}
