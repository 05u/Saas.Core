using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 远程指令
    /// </summary>
    public class RemoteCommandController : BaseApiController
    {

        private readonly ILogger<RemoteCommandController> _logger;
        private readonly IMapper _mapper;
        private readonly BusRemoteCommandService _remoteCommandService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public RemoteCommandController(ILogger<RemoteCommandController> logger, IMapper mapper, BusRemoteCommandService remoteCommandService)
        {
            _logger = logger;
            _mapper = mapper;
            _remoteCommandService = remoteCommandService;
        }



        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<BusRemoteCommand>> Get()
        {

            List<BusRemoteCommand> list = await _remoteCommandService.Queryable().OrderByDescending(c => c.Id).Take(100).ToListAsync();
            return list;

        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<BusRemoteCommand>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
        {

            var list = await _remoteCommandService.Queryable().PagingResultAsync(filter.PageIndex, filter.PageSize, filter.SortField, filter.SortType);
            return list;

        }

        /// <summary>
        /// 客户端心跳更新
        /// </summary>
        /// <param name="clientName"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<BusRemoteCommand> ClientHeartbeat(string clientName)
        {
            return await _remoteCommandService.ClientHeartbeat(clientName);
        }


        /// <summary>
        /// 发送远程指令
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> SendRemoteCommand([FromBody] SendRemoteCommandInput input)
        {
            return await _remoteCommandService.SendRemoteCommand(input);

        }

        /// <summary>
        /// 查询指定客户端上次心跳时间
        /// </summary>
        /// <param name="clientName">客户端名称(不区分大小写)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> GetLastHeartTime(string clientName)
        {
            var list = await _remoteCommandService.Queryable().Where(c => c.ClientName.ToLower().Contains(clientName.ToLower())).ToListAsync();
            if (list.Count() == 0)
            {
                throw new BusinessException("客户端不存在,请确认客户端服务已运行并联网!");
            }
            if (list.Count() > 1)
            {
                throw new BusinessException("查询到多个客户端,请精确查询条件!");
            }
            return list[0].LastHeartTime.ToString();
        }

        /// <summary>
        /// 查询指定客户端是否在线
        /// </summary>
        /// <param name="clientName">客户端名称(不区分大小写)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<bool> GetIsOnline(string clientName)
        {
            var list = await _remoteCommandService.Queryable().Where(c => c.ClientName.ToLower().Contains(clientName.ToLower())).ToListAsync();
            if (list.Count() == 0)
            {
                throw new BusinessException("客户端不存在,请确认客户端服务已运行并联网!");
            }
            if (list.Count() > 1)
            {
                throw new BusinessException("查询到多个客户端,请精确查询条件!");
            }
            return list[0].IsOnline;
        }

        /// <summary>
        /// 设置指定客户端的心跳周期
        /// </summary>
        /// <param name="heartbeatCycle">心跳周期(秒)</param>
        /// <param name="clientName">客户端名称</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> SetHeartbeatCycle(int heartbeatCycle, string clientName)
        {
            var result = await _remoteCommandService.Queryable().WhereIf(clientName.IsNotBlank(), c => c.ClientName == clientName).ToListAsync();
            result.ForEach(x => { x.HeartbeatCycle = heartbeatCycle; });
            await _remoteCommandService.BatchUpdateAsync(result);
            return $"记录条数:{result.Count()},心跳周期更新为:{heartbeatCycle}秒";
        }



    }
}





