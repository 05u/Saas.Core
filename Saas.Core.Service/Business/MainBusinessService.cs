using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;

namespace Saas.Core.Service.Business
{
    /// <summary>
    /// 系统业务
    /// </summary>
    public class MainBusinessService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// 文件存放的根路径
        /// </summary>
        private readonly string _baseUploadDir;


        /// <summary>
        /// ctor
        /// </summary>
        public MainBusinessService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _baseUploadDir = Path.Combine(_webHostEnvironment.ContentRootPath, "UpLoads");
        }

        /// <summary>
        /// 保存上传文件通用方法
        /// </summary>
        /// <param name="file"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public string SaveFile(IFormFile file, string moduleType = null)
        {
            try
            {
                var moduleTypeEnum = moduleType.GetEnum<ModuleType>();

                //服务器将要存储文件的路径
                var Folder = Path.Combine(_baseUploadDir, moduleTypeEnum.ToString());

                if (Directory.Exists(Folder) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(Folder);
                }
                StreamReader reader = new StreamReader(file.OpenReadStream());
                String content = reader.ReadToEnd();
                String filename = Path.Combine(Folder, file.FileName);
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    // 复制文件
                    file.CopyTo(fs);
                    // 清空缓冲区数据
                    fs.Flush();
                }
                return filename;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
