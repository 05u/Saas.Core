using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saas.Core.Data.Entities;
using Saas.Core.Infrastructure.Extentions;
using Saas.Core.Infrastructure.Infrastructures;
using Saas.Core.Service.Business;
using Saas.Core.Service.Dtos;

namespace Saas.Core.WebApi.Controllers
{
    /// <summary>
    /// 微信用户
    /// </summary>
    public class WeixinUserController : BaseApiController
    {
        private readonly RobotService _robotService;
        private readonly BusWeixinUserService _weixinUserService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// ctor
        /// </summary>
        public WeixinUserController(
            RobotService robotService,
            BusWeixinUserService weixinUserService,
            IConfiguration configuration
            )
        {
            _robotService = robotService;
            _weixinUserService = weixinUserService;
            _configuration = configuration;
        }

        /// <summary>
        /// 获取微信机器人好友和群列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<WeixinResult<List<ContactListContent>>> GetContactList(ContactListContent input)
        {
            //var authPasswordVerify = _configuration.GetRequiredSection("AuthPassword")?.Value;
            //if (authPasswordVerify.IsNotBlank() && input.authPassword != authPasswordVerify)
            //{
            //    throw new BusinessException("密码验证失败!");
            //}
            return await _robotService.GetContactList(input);
        }

        /// <summary>
        /// 获取微信群成员昵称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<WeixinResult<MemberNick>> GetMemberNick(string wx_id, string roomid)
        {
            //var authPasswordVerify = _configuration.GetRequiredSection("AuthPassword")?.Value;
            //if (authPasswordVerify.IsNotBlank() && input.authPassword != authPasswordVerify)
            //{
            //    throw new BusinessException("密码验证失败!");
            //}
            return await _robotService.GetMemberNick(wx_id, roomid);
        }

        /// <summary>
        /// 新增姓名和wxid对应关系
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> Create(string authPassword, string wxid, string name)
        {
            var authPasswordVerify = _configuration.GetRequiredSection("AuthPassword")?.Value;
            if (authPasswordVerify.IsNotBlank() && authPassword != authPasswordVerify)
            {
                throw new BusinessException("密码验证失败!");
            }
            if (wxid.IsBlank() || name.IsBlank())
            {
                throw new BusinessException("微信Id和姓名必填");
            }
            if (await _weixinUserService.ExistsAsync(x => x.Name == name))
            {
                throw new BusinessException("姓名重复");
            }
            var Id = await _weixinUserService.InsertAsync(new BusWeixinUser()
            {
                Wxid = wxid,
                Name = name,
            });
            return Id;
        }
    }
}
