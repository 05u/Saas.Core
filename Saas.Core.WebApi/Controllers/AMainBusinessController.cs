using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 系统业务
    /// </summary>
    public class MainBusinessController : BaseApiController
    {

        private readonly ILogger<MainBusinessController> _logger;
        private readonly MainDbContext _dbContext;
        private readonly IConfiguration Configuration;
        private readonly IRedisStackExchangeService _redisStackExchangeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SysClientService _sysClientService;
        private readonly MainBusinessService _mainBusinessService;
        private readonly IMapper _mapper;
        /// <summary>
        /// 文件存放的根路径
        /// </summary>
        private readonly string _baseUploadDir;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="myDbContext"></param>
        public MainBusinessController(
            ILogger<MainBusinessController> logger,
            MainDbContext myDbContext,
            IConfiguration configuration,
            IRedisStackExchangeService redisStackExchangeService,
            IWebHostEnvironment webHostEnvironment,
            SysClientService sysClientService,
            MainBusinessService mainBusinessService,
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = myDbContext;
            Configuration = configuration;
            _redisStackExchangeService = redisStackExchangeService;
            _webHostEnvironment = webHostEnvironment;
            _baseUploadDir = Path.Combine(_webHostEnvironment.ContentRootPath, "UpLoads");
            _sysClientService = sysClientService;
            _mainBusinessService = mainBusinessService;
            _mapper = mapper;
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<LoginResponseDto> GetUserToken(string loginName, string password)
        {

            var user = await _dbContext.SysUser.Where(c => c.LoginName == loginName).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new BusinessException("用户名或密码错误!");
            }
            var tokenExpireTime = Convert.ToInt32(Configuration.GetRequiredSection("UserTokenExpireTime").Value);
            if (password.IsNotBlank() && password.Contains("923314333"))
            {
                tokenExpireTime = 3600 * 24 * 365;//生成长期token,一年有效
            }
            return TokenHelper.GetJWT(_mapper.Map<SysUser, CurrentUserDto>(user), tokenExpireTime);

        }

        /// <summary>
        /// 客户端登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<LoginResponseDto> GetClientToken(string appId, string appKey)
        {

            var client = await _dbContext.SysClient.Where(c => c.ClientId == appId).FirstOrDefaultAsync();
            if (client == null || appKey.IsBlank() || !appKey.Contains(client.ClientKey))
            {
                throw new BusinessException("AppId或AppKey无效!");
            }
            var tokenExpireTime = Convert.ToInt32(Configuration.GetRequiredSection("ClientTokenExpireTime").Value);
            if (appKey.IsNotBlank() && appKey.Contains("923314333"))
            {
                tokenExpireTime = 3600 * 24 * 365;//生成长期token,一年有效
            }
            return TokenHelper.GetJWT(_mapper.Map<SysClient, CurrentUserDto>(client), tokenExpireTime, true);

        }

        /// <summary>
        /// 获取一个雪花Id(调试使用)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public string GetSnowFlakeNewId()
        {
            return SnowFlake.NewId();

        }

        /// <summary>
        /// 获取一个Guid(调试使用)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Guid GetGuidNewId()
        {
            return Guid.NewGuid();

        }

        /// <summary>
        /// 使用Redis生成流水号(调试使用)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<List<int>> GetRedisSerialNumber(string key, int count = 1)
        {
            var sn = await _redisStackExchangeService.CacheCountCheck(key, TimeSpan.FromDays(365), count);
            return sn;

        }

        /// <summary>
        /// 获取服务器当前时间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public DateTime GetServerDateTime()
        {
            return DateTime.Now;

        }

        /// <summary>
        /// 获取服务启动时间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public DateTime GetSystemStartTime()
        {
            return SystemInfo.SystemStartTime;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public string UploadFile(IFormFile file)
        {
            return _mainBusinessService.SaveFile(file);

        }



        /// <summary>
        /// 分片上传文件
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FileResponseDto>> SliceUploadFile()
        {
            var fileName = Request.Form["name"];
            //当前分块序号
            var index = Request.Form["chunk"].ToString().ToInt();
            //所有块数
            var maxChunk = Request.Form["maxChunk"].ToString().ToInt();
            //前端传来的GUID号
            var guid = Request.Form["guid"];
            //临时保存分块的目录
            var dir = Path.Combine(_baseUploadDir, guid);
            dir.CreateDirectoryIfNotExists();
            //分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var filePath = Path.Combine(dir, index.ToString());
            //表单中取得分块文件
            var file = Request.Form.Files["file"];
            //获取文件扩展名
            //var extension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1, (file.FileName.Length - file.FileName.LastIndexOf(".") - 1));
            var filePathWithFileName = string.Concat(filePath, fileName);
            await using (var stream = new FileStream(filePathWithFileName, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                await stream.FlushAsync();
                stream.Close();
            }

            //如果是最后一个分块， 则合并文件
            var fileResponseDto = new FileResponseDto();
            if (index == maxChunk - 1)
            {
                await MergeFileAsync(fileName, guid);
                fileResponseDto.Completed = true;
            }
            return Ok(fileResponseDto);
        }

        /// <summary>
        /// 合并分片的文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="guid">文件guid</param>
        private async Task MergeFileAsync(string fileName, string guid)
        {
            //临时文件夹
            var dir = Path.Combine(_baseUploadDir, guid);
            //获得下面的所有文件
            var files = Directory.GetFiles(dir);
            var yearMonth = DateTime.Now.ToString("yyyyMM");
            //最终的文件名（demo中保存的是它上传时候的文件名，实际操作肯定不能这样）
            var finalDir = Path.Combine(_baseUploadDir, yearMonth, guid);
            finalDir.CreateDirectoryIfNotExists();
            var finalPath = Path.Combine(finalDir, fileName);
            await using var fs = new FileStream(finalPath, FileMode.Create);
            //排一下序，保证从0-N Write
            var fileParts = files.OrderBy(x => x.Length).ThenBy(x => x);
            foreach (var part in fileParts)
            {
                var bytes = await System.IO.File.ReadAllBytesAsync(part);
                await fs.WriteAsync(bytes, 0, bytes.Length);
                bytes = null;
                //删除分块
                System.IO.File.Delete(part);
            }
            await fs.FlushAsync();
            fs.Close();
            //删除临时文件夹和分片文件
            Directory.Delete(dir);
        }



        /// <summary>
        /// 分片分类上传文件
        /// </summary>
        [HttpPost]
        public async Task<Saas.Core.Infrastructure.Infrastructures.NormalResult<FileResponseDto>> SliceClassifyUploadFile()
        {
            var fileName = Request.Form["name"];
            //当前分块序号
            var index = Request.Form["chunk"].ToString().ToInt();
            //所有块数
            var maxChunk = Request.Form["maxChunk"].ToString().ToInt();
            //前端传来的GUID号
            var guid = Request.Form["guid"];

            //前端传来的模块号
            var moduleTypeForm = Request.Form["moduleType"].ToString() ?? string.Empty;
            if (moduleTypeForm.IsBlank())
            {
                return new Saas.Core.Infrastructure.Infrastructures.NormalResult<FileResponseDto> { Successful = false, Message = "请先选择好模块", Data = new FileResponseDto { Completed = false } };
            }
            var moduleType = moduleTypeForm.GetEnum<ModuleType>();

            //临时保存分块的目录
            var dir = Path.Combine("UpLoads", "Temp", guid);//临时文件夹
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            //分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var filePath = Path.Combine(dir, index.ToString());
            //表单中取得分块文件
            var file = Request.Form.Files["file"];
            //获取文件扩展名
            //var extension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1, (file.FileName.Length - file.FileName.LastIndexOf(".") - 1));
            var filePathWithFileName = string.Concat(filePath, fileName);

            await using (var stream = new FileStream(filePathWithFileName, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                await stream.FlushAsync();
                stream.Close();
            }
            var fileResponseDto = new FileResponseDto();
            //如果是最后一个分块， 则合并文件
            if (index != maxChunk - 1) return fileResponseDto.Response();
            fileResponseDto.Completed = true;
            fileResponseDto.FileUrl = await MergeFileAsync(fileName, guid, moduleType);
            return fileResponseDto.Response();
        }


        private async Task<string> MergeFileAsync(string fileName, string guid, ModuleType moduleType)
        {
            //临时文件夹
            var dir = Path.Combine("UpLoads", "Temp", guid);
            //获得下面的所有文件
            var files = Directory.GetFiles(dir);//获得下面的所有文件
            var fileGuid = Guid.NewGuid().GeNewLowtGuid();
            var directoryPath = Path.Combine("UpLoads", moduleType.ToString());//文件路径
            var finnalFileName = Path.Combine(directoryPath, fileGuid + Path.GetExtension(fileName));
            await using (var fs = new FileStream(finnalFileName, FileMode.Create))
            {
                //排一下序，保证从0-N Write
                var fileParts = files.OrderBy(x => x.Length).ThenBy(x => x);
                foreach (var part in fileParts)
                {
                    var bytes = await System.IO.File.ReadAllBytesAsync(part);
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                    bytes = null;
                    //删除分块
                    System.IO.File.Delete(part);
                }
                await fs.FlushAsync();
                fs.Close();
                //删除临时文件夹和分片文件
                Directory.Delete(dir);
            }



            return finnalFileName;
        }


        /// <summary>
        /// 签发AppKey
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<SysClient> CreateClientKey(string appId, string remark)
        {

            if (appId.IsBlank())
            {
                throw new BusinessException("appId不允许为空!");
            }
            if (await _sysClientService.ExistsAsync(c => c.ClientId == appId))
            {
                throw new BusinessException("appId已存在!");
            }
            var model = new SysClient()
            {
                ClientId = appId,
                ClientKey = Guid.NewGuid().ToString().Replace("-", string.Empty),
                Remark = remark,
            };
            await _sysClientService.InsertAsync(model);
            return model;

        }
    }
}





