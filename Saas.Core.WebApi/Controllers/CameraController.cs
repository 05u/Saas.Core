using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Dtos;
using Saas.Core.Infrastructure.Enums;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Infrastructure.Utilities;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;
using XiaoFeng;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 摄像头管理
    /// </summary>
    public class CameraController : BaseApiController
    {
        private readonly MdmCameraService _service;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// ctor
        /// </summary>
        public CameraController(MdmCameraService CameraService, IMapper mapper, IConfiguration configuration)
        {
            _service = CameraService;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// 分页获取记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedResult<MdmCamera>> SearchPaged([FromBody] PageBaseFilter<SearchInput> filter)
        {
            var list = await _service.Queryable().PagingResultAsync(filter.PageIndex, filter.PageSize, filter.SortField, filter.SortType);
            return list;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Create([FromBody] MdmCamera dto)
        {
            if (dto.Name.IsBlank())
            {
                throw new BusinessException("摄像头名称必填");
            }
            if (await _service.ExistsAsync(x => x.Name == dto.Name))
            {
                throw new BusinessException("摄像头名称重复");
            }
            var encryptionKey = _configuration.GetRequiredSection("EncryptionKey")?.Value;
            if (dto.Pass.IsNotBlank() && encryptionKey.IsNotBlank())
            {
                dto.Pass = AESEncryption.EncryptAES(dto.Pass, encryptionKey);
            }
            var Id = await _service.InsertAsync(dto);
            return Id;
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<MdmCamera> Get([FromRoute] string id)
        {
            var dto = await _service.FindAsync(id);
            return dto;
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Edit([FromBody] MdmCamera dto)
        {
            if (dto.Name.IsBlank())
            {
                throw new BusinessException("摄像头名称必填");
            }
            if (await _service.ExistsAsync(x => x.Name == dto.Name && x.Id != dto.Id))
            {
                throw new BusinessException("摄像头名称重复");
            }
            var encryptionKey = _configuration.GetRequiredSection("EncryptionKey")?.Value;
            if (dto.Pass.IsNotBlank() && encryptionKey.IsNotBlank())
            {
                dto.Pass = AESEncryption.EncryptAES(dto.Pass, encryptionKey);
            }
            await _service.UpdateAsync(dto);
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Delete([FromBody] IList<string> ids)
        {
            await _service.DeleteAsync(ids);
            return true;
        }

        /// <summary>
        /// 设置摄像头角度
        /// </summary>
        /// <param name="name">所属家庭名称(非必须)+摄像头名称</param>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpGet]
        public async Task<bool> SetCameraArea(string name, double x, double y)
        {
            var dtoList = await _service.Queryable().Where(c => name.Contains(c.Name) && (c.HomeName == null || name.Contains(c.HomeName))).ToListAsync();
            if (dtoList.Count == 0)
            {
                throw new BusinessException("未查询到摄像头");
            }
            if (dtoList.Count > 1)
            {
                throw new BusinessException("查询到多个摄像头");
            }
            if (x < -1 || x > 1 || y < -1 || y > 1)
            {
                throw new BusinessException("坐标值必须是-1到1之间的浮点数");
            }
            var dto = dtoList.FirstOrDefault();
            var encryptionKey = _configuration.GetRequiredSection("EncryptionKey")?.Value;
            if (dto.Pass.IsNotBlank() && encryptionKey.IsNotBlank())
            {
                dto.Pass = AESEncryption.DecryptAES(dto.Pass, encryptionKey);
            }
            await _service.SetCameraArea(dto.Ip, dto.Port, dto.User, dto.Pass, x, y);
            return true;
        }

        /// <summary>
        /// 设置摄像头开关状态
        /// </summary>
        /// <param name="status">true:开 false:关</param>
        /// <param name="name">所属家庭名称(非必须)+摄像头名称(全部则操作所有)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> SetCameraStatus(bool status, string name)
        {
            return await _service.SetCameraStatus(status, name);
        }

    }
}
