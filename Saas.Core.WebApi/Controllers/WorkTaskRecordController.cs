using AutoMapper;
using AutoMapper.QueryableExtensions;
using Magicodes.ExporterAndImporter.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Service.Base;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 工作任务记录
    /// </summary>
    public class WorkTaskRecordController : BaseApiController
    {

        private readonly ILogger<WorkTaskRecordController> _logger;
        private readonly BusWorkTaskRecordService _workTaskRecordService;
        private readonly ExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly MainBusinessService _mainBusinessService;
        private readonly BusWeixinUserService _weixinUserService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        public WorkTaskRecordController(
            ILogger<WorkTaskRecordController> logger,
            BusWorkTaskRecordService workTaskRecordService,
            ExcelService excelService,
            IMapper mapper,
            MainBusinessService mainBusinessService,
            BusWeixinUserService weixinUserService,
            IConfiguration configuration)
        {
            _logger = logger;
            _workTaskRecordService = workTaskRecordService;
            _excelService = excelService;
            _mapper = mapper;
            _mainBusinessService = mainBusinessService;
            _weixinUserService = weixinUserService;
            _configuration = configuration;
        }


        /// <summary>
        /// 获取工作任务导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<FileStreamResult> ExcelTemplate()
        {
            return await _excelService.GetTemplate<WorkTaskRecordDto>(ModuleType.WorkTaskRecord);
        }

        /// <summary>
        /// 工作任务导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ExcelImportResult> ExcelTemplateImport(IFormFile file, string authPassword)
        {

            var authPasswordVerify = _configuration.GetRequiredSection("AuthPassword")?.Value;
            if (authPasswordVerify.IsNotBlank() && authPassword != authPasswordVerify)
            {
                throw new BusinessException("密码验证失败!");
            }
            var input = new ExcelImportBase()
            {
                FilePath = _mainBusinessService.SaveFile(file, ModuleType.WorkTaskRecord.ToString()),
                Override = true,
            };

            var weixinUserList = _weixinUserService.Queryable().ToList();

            ExcelImportResult result = new ExcelImportResult();

            var import = await _excelService.Import<WorkTaskRecordDto>(input.FilePath);
            if (!import.HasError)
            {
                var excelData = import.Data.ToList();

                //查询数据库中所有记录
                var dbLists = await _workTaskRecordService.Queryable().ToListAsync();

                List<BusWorkTaskRecord> insertListDto = new List<BusWorkTaskRecord>();
                List<BusWorkTaskRecord> updateListDto = new List<BusWorkTaskRecord>();
                List<DataRowErrorInfo> errorList = new List<DataRowErrorInfo>();


                excelData.ForEach(item =>
                        {
                            DataRowErrorInfo errorInfo = new DataRowErrorInfo();
                            result.Count = excelData.Count;

                            var dtoExist = dbLists.FirstOrDefault(b => b.Name == item.Name && b.WorkDate.Date == item.WorkDate.Date && b.WorkClassType == item.WorkClassType);
                            if (dtoExist != null)
                            {
                                if (!input.Override)
                                {
                                    errorInfo.RowIndex = excelData.FindIndex(o => o.Equals(item)) + 2;
                                    errorInfo.FieldErrors.Add("姓名", "该人员存在相同日期和班次的记录！");
                                    errorList.Add(errorInfo);
                                }
                                else
                                {
                                    var wxid = weixinUserList.FirstOrDefault(c => c.Name == item.Name)?.Wxid;
                                    if (wxid.IsBlank())
                                    {
                                        errorInfo.RowIndex = excelData.FindIndex(o => o.Equals(item)) + 2;
                                        errorInfo.FieldErrors.Add("姓名", "根据姓名未查询到wxid，请先录入！");
                                        errorList.Add(errorInfo);
                                    }
                                    else
                                    {
                                        dtoExist.Wxid = wxid;
                                        dtoExist.Name = item.Name;
                                        dtoExist.WorkDate = item.WorkDate;
                                        dtoExist.WorkClassType = item.WorkClassType;
                                        dtoExist.IsNotice = false;
                                        updateListDto.Add(dtoExist);
                                    }

                                }

                            }
                            else
                            {
                                var wxid = weixinUserList.FirstOrDefault(c => c.Name == item.Name)?.Wxid;
                                if (wxid.IsBlank())
                                {
                                    errorInfo.RowIndex = excelData.FindIndex(o => o.Equals(item)) + 2;
                                    errorInfo.FieldErrors.Add("姓名", "根据姓名未查询到wxid，请确认！");
                                    errorList.Add(errorInfo);
                                }
                                else
                                {
                                    BusWorkTaskRecord dto = new BusWorkTaskRecord()
                                    {
                                        Wxid = wxid,
                                        Name = item.Name,
                                        WorkDate = item.WorkDate,
                                        WorkClassType = item.WorkClassType,
                                    };
                                    insertListDto.Add(dto);
                                }


                            }

                        });

                if (errorList.Count > 0)
                {
                    _excelService.OutputBussinessErrorData<WorkTaskRecordDto>(input.FilePath, errorList, out string msg);
                    result.ImportSuccess = false;
                    result.ErrorExcelPath = Path.GetFileName(msg);
                    result.ErrorMessage = "上传的数据填写不正确,我们已经将错误内容标示在以下文件中";
                }
                else
                {
                    //生成计划提醒时间
                    insertListDto.ForEach(x =>
                    {
                        x.WorkDate = x.WorkDate.Date;
                        switch (x.WorkClassType)
                        {
                            case WorkClassType.Dayeban:
                                x.PlanNoticeTime = x.WorkDate.AddHours(-1);//前一天晚上11点提醒
                                break;
                            case WorkClassType.Baiban:
                                x.PlanNoticeTime = x.WorkDate.AddHours(7);//当天早上7点提醒
                                break;
                            case WorkClassType.Xiaoyeban:
                                x.PlanNoticeTime = x.WorkDate.AddHours(17);//当天下午5点提醒
                                break;
                            default:
                                break;
                        }
                    });
                    updateListDto.ForEach(x =>
                    {
                        x.WorkDate = x.WorkDate.Date;
                        switch (x.WorkClassType)
                        {
                            case WorkClassType.Dayeban:
                                x.PlanNoticeTime = x.WorkDate.AddHours(-1);
                                break;
                            case WorkClassType.Baiban:
                                x.PlanNoticeTime = x.WorkDate.AddHours(7);
                                break;
                            case WorkClassType.Xiaoyeban:
                                x.PlanNoticeTime = x.WorkDate.AddHours(17);
                                break;
                            default:
                                break;
                        }
                    });

                    await _workTaskRecordService.BatchInsertAsync(insertListDto);
                    await _workTaskRecordService.BatchUpdateAsync(updateListDto);
                }

            }
            else
            {
                string excelPath = Path.Combine(Path.GetDirectoryName(input.FilePath) ?? string.Empty, $"{Path.GetFileNameWithoutExtension(input.FilePath) + "_.xlsx"}");

                if (System.IO.File.Exists(excelPath))
                {
                    result.ImportSuccess = false;
                    result.ErrorExcelPath = $"{Path.GetFileNameWithoutExtension(input.FilePath) + "_.xlsx"}";
                    result.ErrorMessage = "上传的数据填写不正确,我们已经将错误内容标示在以下文件中";
                }
                else
                {
                    throw new BusinessException("导入失败,请检查模板是否匹配");
                }

            }


            return result;

        }

        /// <summary>
        /// 工作任务导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<FileStreamResult> ExcelTemplateExport(string authPassword)
        {
            var authPasswordVerify = _configuration.GetRequiredSection("AuthPassword")?.Value;
            if (authPasswordVerify.IsNotBlank() && authPassword != authPasswordVerify)
            {
                throw new BusinessException("密码验证失败!");
            }
            var data = await _workTaskRecordService.Queryable().ProjectTo<WorkTaskRecordDto>(_mapper.ConfigurationProvider).ToListAsync();
            return await _excelService.Export($"工作任务导出", data);
        }

    }
}





