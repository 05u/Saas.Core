using AutoMapper;
using AutoMapper.QueryableExtensions;
using Magicodes.ExporterAndImporter.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.XWPF.UserModel;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Base;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;
using Saas.Core.Service.Dtos.HM;
using System.Numerics;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 慧明
    /// </summary>
    public class HuimingController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ExcelService _excelService;
        private readonly MainBusinessService _mainBusinessService;
        private IHttpClientFactory _httpClientFactory;



        /// <summary>
        /// ctor
        /// </summary>
        public HuimingController(IMapper mapper, ExcelService excelService, MainBusinessService mainBusinessService, IHttpClientFactory httpClientFactory)
        {
            _mapper = mapper;
            _excelService = excelService;
            _mainBusinessService = mainBusinessService;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 节能环保项导入生成SQL
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ContentResult> JienenghuanbaoExcelToSQL(IFormFile file)
        {
            var input = new ExcelImportBase()
            {
                FilePath = _mainBusinessService.SaveFile(file, ModuleType.WorkTaskRecord.ToString()),
                Override = true,
            };

            var sql = "";
            ExcelImportResult result = new ExcelImportResult();

            var import = await _excelService.Import<JienenghuanbaoExcelDto>(input.FilePath);
            if (!import.HasError)
            {
                var excelData = import.Data.ToList();
                List<DataRowErrorInfo> errorList = new List<DataRowErrorInfo>();


                var index = 1;
                excelData.ForEach(item =>
                {
                    DataRowErrorInfo errorInfo = new DataRowErrorInfo();
                    result.Count = excelData.Count;
                    index += 1;
                    sql += $@"INSERT INTO [secda000001].[dbo].[BaseItems] ([ID], [Name], [Code], [Unit], [TargetType], [Exp], [Deleted], [CreatedOn], [ModifiedOn], [Domain], [TenantId], [Sort], [CreatedBy], [ModifiedBy], [IsPriceComparison], [ExcelCode]) VALUES ('{Guid.NewGuid()}', N'{item.Name}', N'{item.Code}', N'{item.Unit}', {(int)item.TargetType}, NULL, '0', '{DateTime.Now}', NULL, N'000', N'', 0, N'9a354808-b626-4270-9442-2772c937d6d9', NULL, '0', N'D{index}');{Environment.NewLine}";
                });

                if (errorList.Count > 0)
                {
                    _excelService.OutputBussinessErrorData<JienenghuanbaoExcelDto>(input.FilePath, errorList, out string msg);
                    result.ImportSuccess = false;
                    result.ErrorExcelPath = Path.GetFileName(msg);
                    result.ErrorMessage = "上传的数据填写不正确,我们已经将错误内容标示在以下文件中";
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
            return new ContentResult() { Content = sql };
        }

        /// <summary>
        /// 安全标准化Word导入
        /// </summary>
        /// <param name="url">远端接口地址</param>
        /// <param name="file">导入的文件</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<object> SafetyStandardWordImportAsync(string url, IFormFile file)
        {

            var filePath = _mainBusinessService.SaveFile(file, ModuleType.SafetyStandard.ToString());

            List<string> result = new List<string>();//结果容器

            FileStream stream = new FileStream(filePath, FileMode.Open);//打开流
            XWPFDocument docx = new XWPFDocument(stream);
            var list = new List<XWPFTableCell>();

            //循环遍历表格内容
            foreach (var row in docx.Tables[0].Rows)
            {
                foreach (var cell in row.GetTableCells())
                {
                    if (!list.Contains(cell))
                    {
                        list.Add(cell);
                        result.Add(cell.GetText());
                    }
                }
            }
            stream.Close();
            if ((result.Count % 10) != 0)
            {
                throw new BusinessException("Word文档内表格数据不符合规范,请检查");
            }

            var dtoList = new List<SafetyStandardWordDto>();
            string cacheFactor1 = null;
            string cacheFactor2 = null;
            string cacheFactor3 = null;
            string cacheFactor1No = null;
            string cacheFactor2No = null;
            string cacheFactor3No = null;
            string cacheLibraryName = null;
            var factor4List = new List<Factor4Info>();
            for (int i = 1; i < result.Count / 10; i++)
            {
                var x = i * 10;
                var dto = new SafetyStandardWordDto()
                {
                    Factor1No = result[x].IsBlank() ? cacheFactor1No + cacheFactor1 : result[x],
                    Factor2No = result[x + 1].IsBlank() ? cacheFactor2No + cacheFactor2 : result[x + 1],
                    Factor3No = result[x + 2].IsBlank() ? cacheFactor3No + cacheFactor3 : result[x + 2],
                    LibraryName = result[x + 3].IsBlank() ? cacheLibraryName : result[x + 3],
                    CreateStandard = result[x + 4],
                    LibraryScore = result[x + 5].ToDynamic(),
                    CheckMethod = result[x + 6],
                    Insider = result[x + 7],
                    CheckSituation1 = result[x + 8],
                    CheckSituation2 = result[x + 9],
                };

                dto.Factor1 = dto.Factor1No.Replace(".", string.Empty).Replace("1", string.Empty).Replace("2", string.Empty).Replace("3", string.Empty).Replace("4", string.Empty).Replace("5", string.Empty).Replace("6", string.Empty).Replace("7", string.Empty).Replace("8", string.Empty).Replace("9", string.Empty).Replace("0", string.Empty);
                dto.Factor2 = dto.Factor2No.Replace(".", string.Empty).Replace("1", string.Empty).Replace("2", string.Empty).Replace("3", string.Empty).Replace("4", string.Empty).Replace("5", string.Empty).Replace("6", string.Empty).Replace("7", string.Empty).Replace("8", string.Empty).Replace("9", string.Empty).Replace("0", string.Empty);
                dto.Factor3 = dto.Factor3No.Replace(".", string.Empty).Replace("1", string.Empty).Replace("2", string.Empty).Replace("3", string.Empty).Replace("4", string.Empty).Replace("5", string.Empty).Replace("6", string.Empty).Replace("7", string.Empty).Replace("8", string.Empty).Replace("9", string.Empty).Replace("0", string.Empty);
                dto.Factor1No = dto.Factor1No.Substring(0, dto.Factor1No.Length - dto.Factor1.Length).Replace(".", string.Empty);//一级编号里不允许有.
                dto.Factor2No = dto.Factor2No.Substring(0, dto.Factor2No.Length - dto.Factor2.Length);
                dto.Factor3No = dto.Factor3No.Substring(0, dto.Factor3No.Length - dto.Factor3.Length);

                var currentFactor4 = factor4List.Where(c => c.Factor3No == dto.Factor3No).OrderByDescending(c => c.Factor4LastNo).FirstOrDefault();
                if (currentFactor4 == null)
                {
                    dto.Factor4No = dto.Factor3No + ".1";
                    factor4List.Add(new Factor4Info() { Factor3No = dto.Factor3No, Factor4LastNo = 1, LibraryName = dto.LibraryName });
                }
                else
                {
                    if (currentFactor4.LibraryName == dto.LibraryName)
                    {
                        dto.Factor4No = dto.Factor3No + "." + currentFactor4.Factor4LastNo;
                    }
                    else
                    {
                        dto.Factor4No = dto.Factor3No + "." + (currentFactor4.Factor4LastNo + 1);
                        factor4List.Add(new Factor4Info() { Factor3No = dto.Factor3No, Factor4LastNo = currentFactor4.Factor4LastNo + 1, LibraryName = dto.LibraryName });
                    }
                }


                dtoList.Add(dto);
                cacheFactor1 = dto.Factor1;
                cacheFactor2 = dto.Factor2;
                cacheFactor3 = dto.Factor3;
                cacheFactor1No = dto.Factor1No;
                cacheFactor2No = dto.Factor2No;
                cacheFactor3No = dto.Factor3No;
                cacheLibraryName = dto.LibraryName;
            }

            if (url.IsBlank())
            {
                return dtoList;
            }
            var client = _httpClientFactory.CreateClient(HttpClientConst.CommonClient);
            var content = JsonContent.Create(dtoList);
            var response = await client.PostAsync(url, content);
            var remoteResult = await response.Content.ReadAsStringAsync();

            return "远端接口返回内容如下:" + remoteResult;
        }

    }
}
