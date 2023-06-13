using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Context;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 网络唤醒
    /// </summary>
    public class WakeOnLanController : BaseApiController
    {

        private readonly ILogger<RemoteCommandController> _logger;
        private readonly MainDbContext _dbContext;
        private readonly BusWakeOnLanService _wakeOnLanService;
        private readonly BusRemoteCommandService _remoteCommandService;

        /// <summary>
        /// 
        /// </summary>
        public WakeOnLanController(ILogger<RemoteCommandController> logger, MainDbContext myDbContext, BusWakeOnLanService wakeOnLanService, BusRemoteCommandService remoteCommandService)
        {
            _logger = logger;
            _dbContext = myDbContext;
            _wakeOnLanService = wakeOnLanService;
            _remoteCommandService = remoteCommandService;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<BusWakeOnLanRecord>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
        {

            var list = await _wakeOnLanService.Queryable().PagingResultAsync(filter.PageIndex, filter.PageSize, filter.SortField, filter.SortType);
            return list;

        }

        /// <summary>
        /// 获取可唤醒主机列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<List<BusRemoteCommand>> WakeList()
        {

            return await _remoteCommandService.Queryable().Where(c => c.ClientMac != null).ToListAsync();

        }

        /// <summary>
        /// 唤醒指定主机
        /// </summary>
        /// <param name="mac">需要被唤醒主机的MAC地址</param>
        /// <param name="remark">唤醒说明(非必填)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> WOL(string mac, string remark)
        {
            await _wakeOnLanService.WOL(mac, remark);
            return "发送成功!";

        }

        /// <summary>
        /// 唤醒书房台式机
        /// </summary>
        /// <param name="remark">唤醒说明(非必填)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> WakeShufangPC(string remark)
        {
            await _wakeOnLanService.WOL("8c:82:b9:51:14:21", remark);
            return "发送成功!";

        }

        /// <summary>
        /// 唤醒神舟笔记本
        /// </summary>
        /// <param name="remark">唤醒说明(非必填)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> WakeHaseeBook(string remark)
        {
            await _wakeOnLanService.WOL("80:fa:5b:53:87:68", remark);
            return "发送成功!";

        }

        ///// <summary>
        ///// 唤醒测试服务器
        ///// </summary>
        ///// <param name="remark">唤醒说明(非必填)</param>
        ///// <returns></returns>
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<string> WakeTestServer(string remark)
        //{
        //    await _wakeOnLanService.WOL("8c:82:b9:51:04:03", remark);
        //    return "发送成功!";

        //}

        /// <summary>
        /// 唤醒工控机
        /// </summary>
        /// <param name="remark">唤醒说明(非必填)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> WakeIndustrialPC(string remark)
        {
            await _wakeOnLanService.WOL("00:e0:4c:68:d5:2d", remark);
            return "发送成功!";

        }





    }
}





