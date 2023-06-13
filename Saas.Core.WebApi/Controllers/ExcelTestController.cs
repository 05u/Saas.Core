using AutoMapper;
using AutoMapper.QueryableExtensions;
using Magicodes.ExporterAndImporter.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Service.Base;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;
using Saas.Core.Service.Dtos.HM;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// Excel导入导出
    /// </summary>
    public class ExcelTestController : BaseApiController
    {

        private readonly ILogger<MainBusinessController> _logger;
        private readonly MainDbContext _dbContext;
        private readonly ExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly MainBusinessService _mainBusinessService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="myDbContext"></param>
        /// <param name="excelService"></param>
        public ExcelTestController(
            ILogger<MainBusinessController> logger,
            MainDbContext myDbContext,
            ExcelService excelService,
            IMapper mapper,
            MainBusinessService mainBusinessService)
        {
            _logger = logger;
            _dbContext = myDbContext;
            _excelService = excelService;
            _mapper = mapper;
            _mainBusinessService = mainBusinessService;
        }


        /// <summary>
        /// 获取导入小爱设备模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileStreamResult> ExcelTemplate()
        {
            return await _excelService.GetTemplate<MdmXiaoaiSpeakerDto>(ModuleType.MdmXiaoaiSpeaker);
        }


        /// <summary>
        /// 小爱设备导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ExcelImportResult> ExcelTemplateImport([FromBody] ExcelImportBase input)
        {

            ExcelImportResult result = new ExcelImportResult();

            var import = await _excelService.Import<MdmXiaoaiSpeakerDto>(input.FilePath);
            if (!import.HasError)
            {
                var excelData = import.Data.ToList();
                //查询数据库中已存在code
                var excelCodes = excelData.ConvertAll(c => c.Code);
                var dbLists = await _dbContext.MdmXiaoaiSpeaker.Where(c => excelCodes.Contains(c.Code)).ToListAsync();

                List<MdmXiaoaiSpeaker> insertListDto = new List<MdmXiaoaiSpeaker>();
                List<MdmXiaoaiSpeaker> updateListDto = new List<MdmXiaoaiSpeaker>();
                List<DataRowErrorInfo> errorList = new List<DataRowErrorInfo>();


                excelData.ForEach(item =>
                {
                    DataRowErrorInfo errorInfo = new DataRowErrorInfo();
                    result.Count = excelData.Count;

                    var dtoExist = dbLists.FirstOrDefault(b => b.Code == item.Code);
                    if (dtoExist != null)
                    {
                        if (!input.Override)
                        {
                            errorInfo.RowIndex = excelData.FindIndex(o => o.Equals(item)) + 2;
                            errorInfo.FieldErrors.Add("代码", "数据库代码重复！");
                            errorList.Add(errorInfo);
                        }
                        else
                        {
                            dtoExist.Code = item.Code;
                            dtoExist.Name = item.Name;
                            dtoExist.DeviceID = item.DeviceID;
                            dtoExist.SerialNumber = item.SerialNumber;
                            dtoExist.Address = item.Address;
                            dtoExist.MiotDID = item.MiotDID;
                            dtoExist.Hardware = item.Hardware;
                            dtoExist.Mac = item.Mac;
                            updateListDto.Add(dtoExist);
                        }

                    }
                    else
                    {
                        MdmXiaoaiSpeaker dto = new MdmXiaoaiSpeaker()
                        {
                            Code = item.Code,
                            Name = item.Name,
                            DeviceID = item.DeviceID,
                            SerialNumber = item.SerialNumber,
                            Address = item.Address,
                            MiotDID = item.MiotDID,
                            Hardware = item.Hardware,
                            Mac = item.Mac,
                        };
                        insertListDto.Add(dto);
                    }

                });

                if (errorList.Count > 0)
                {
                    _excelService.OutputBussinessErrorData<MdmXiaoaiSpeakerDto>(input.FilePath, errorList, out string msg);
                    result.ImportSuccess = false;
                    result.ErrorExcelPath = Path.GetFileName(msg);
                    result.ErrorMessage = "上传的数据填写不正确,我们已经将错误内容标示在以下文件中";
                }
                else
                {
                    await _dbContext.MdmXiaoaiSpeaker.AddRangeAsync(insertListDto);
                    await _dbContext.MdmXiaoaiSpeaker.BulkUpdateAsync(updateListDto);
                    await _dbContext.SaveChangesAsync();
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
                    //return Ok(ResponseModel.CreateError("导入失败,请检查模板是否匹配"));
                    throw new BusinessException("导入失败,请检查模板是否匹配");
                }

            }


            return result;

        }

        /// <summary>
        /// 小爱设备导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileStreamResult> ExcelTemplateExport()
        {
            var data = await _dbContext.MdmXiaoaiSpeaker.Where(c => !c.IsDeleted).ProjectTo<MdmXiaoaiSpeakerDto>(_mapper.ConfigurationProvider).ToListAsync();
            return await _excelService.Export($"小爱设备导出", data);
        }

    }
}





