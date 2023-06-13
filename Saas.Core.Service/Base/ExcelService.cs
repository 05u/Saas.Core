using Magicodes.ExporterAndImporter.Core.Models;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Infrastructures;
using System.Data;

namespace Saas.Core.Service.Base
{
    /// <summary>
    /// Excel业务处理
    /// </summary>
    public class ExcelService
    {
        private readonly IExcelImporter _excelImporter;
        private readonly IExcelExporter _excelExporter;

        public ExcelService(
            IExcelImporter excelImporter,
            IExcelExporter excelExporter)
        {
            _excelImporter = excelImporter;
            _excelExporter = excelExporter;
        }


        /// <summary>
        /// 
        /// </summary>
        //public ExcelImporter Importer = new ExcelImporter();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public async Task<FileStreamResult> GetTemplate<T>(ModuleType moduleType) where T : class, new()
        {
            var filePath = Path.Combine("UpLoads", $"{Convert.ToString(moduleType)}",
                Guid.NewGuid().ToString("N") + ".xlsx");
            var result = await _excelImporter.GenerateTemplate<T>(filePath);
            return await FileDownLoad(result.FileName, $"{moduleType}.xlsx");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileFullPath">文件全路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        private async Task<FileStreamResult> FileDownLoad(string fileFullPath, string fileName)
        {
            var filePath = Path.Combine(fileFullPath);

            //文件名必须编码，否则会有特殊字符(如中文)无法在此下载。
            var memoryStream = new MemoryStream();

            await using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))//防止文件被占用启用文件共享
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            File.Delete(filePath);
            var encodeFileName = System.Net.WebUtility.UrlEncode(fileName);
            //文件名必须编码，否则会有特殊字符(如中文)无法在此下载。
            //Response.Headers.Add("Content-Disposition", "attachment; filename=" + encodeFileName);
            //return new FileStreamResult(memoryStream, "application/octet-stream");//文件流方式，指定文件流对应的ContenType。
            return new FileStreamResult(memoryStream, new MediaTypeHeaderValue("application/octet-stream"))
            {
                FileDownloadName = fileName,
                EnableRangeProcessing = true
            };
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<ImportResult<T>> Import<T>(string filePath) where T : class, new()
        {
            if (!File.Exists(filePath))
            {
                throw new BusinessException("文件不存在,请稍候再试");

            }
            var data = await _excelImporter.Import<T>(filePath);

            if (data.HasError)
            {
                string excelPath = Path.Combine(Path.GetDirectoryName(filePath) ?? string.Empty, $"{Path.GetFileNameWithoutExtension(filePath) + "_.xlsx"}");

                if (File.Exists(excelPath))
                {

                }
                else
                {
                    throw new BusinessException("导入失败,请检查模板是否匹配");
                }
            }

            if (data.Data == null || data.Data.Count == 0)
                throw new BusinessException("导入文件未填写相关数据");

            return data;

        }
        /// <summary>
        /// 导入返回业务错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="bussinessErrorDataList"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool OutputBussinessErrorData<T>(string filePath, List<DataRowErrorInfo> bussinessErrorDataList, out string msg) where T : class, new()
        {
            if (!File.Exists(filePath))
            {
                //throw new BusinessException(Infrastructure.Enums.BusinessExceptionType.FileDoesNotExist, "文件不存在");
            }

            return _excelImporter.OutputBussinessErrorData<T>(filePath, bussinessErrorDataList, out msg);
        }


        /// <summary>
        /// Excel导出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="dataItems"></param>
        /// <returns></returns>
        public async Task<FileStreamResult> Export<T>(string fileName, ICollection<T> dataItems) where T : class, new()
        {
            var filePath = Path.Combine("UpLoads", $"{fileName}{Guid.NewGuid():N}.xlsx");
            var result = await _excelExporter.Export(filePath, dataItems);

            return await FileDownLoad(result.FileName, $"{fileName}.xlsx");
        }


        /// <summary>
        /// Excel导出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="dataItems"></param>
        /// <returns></returns>
        public async Task<FileStreamResult> Export(string fileName, DataTable dt)
        {
            var filePath = Path.Combine("UpLoads", $"{fileName}{Guid.NewGuid():N}.xlsx");
            var result = await _excelExporter.Export(filePath, dt);

            return await FileDownLoad(result.FileName, $"{fileName}.xlsx");
        }

        /// <summary>
        /// 默认将excel第一个sheet 读取成dt,如果存在列名重复则抛出异常
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public DataTable GetDataFromExcelFile(string filePath, int maxColCount)
        {
            if (!filePath.EndsWith(".xlsx"))
            {
                //throw new BusinessException(Infrastructure.Enums.BusinessExceptionType.FileFormatIsNotCorrect, "只支持.xlsx格式的excel文档");
            }
            //if (data == null || !data.Any()|| data.Any(c=>c.ColumnName.IsBlank())) 
            //{
            //    throw new BusinessException("数据源无效");
            //}
            var package = new ExcelPackage(new FileInfo(filePath));
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            //获取worksheet的行数
            int rows = worksheet.Dimension.End.Row;
            //获取worksheet的列数
            int cols = worksheet.Dimension.End.Column;
            if (cols > maxColCount)
            {
                //throw new BusinessException(Infrastructure.Enums.BusinessExceptionType.ExcelColumnNumberIsNotCorrect, "请检查模板列!模板列数量大于期望数量");
            }
            DataTable dt = new DataTable(worksheet.Name);
            DataRow dr = null;
            for (int i = 1; i <= rows; i++)
            {
                if (i > 1)
                    dr = dt.Rows.Add();

                for (int j = 1; j <= cols; j++)
                {
                    //默认将第一行设置为datatable的标题
                    if (i == 1)
                        dt.Columns.Add(GetString(worksheet.Cells[i, j].Value));
                    //剩下的写入datatable
                    else
                        dr[j - 1] = GetString(worksheet.Cells[i, j].Value);
                }
            }
            return dt;

        }
        private static string GetString(object obj)
        {
            try
            {
                return obj?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }



    }
}
