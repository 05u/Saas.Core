using Microsoft.AspNetCore.Http;
using Saas.Core.Infrastructure.Extentions;
using System.Security.Claims;
using static Saas.Core.Infrastructure.Utilities.Constants;

namespace Saas.Core.Infrastructure.Utilities
{
    /// <summary>
    /// 当前登陆用户信息服务(仅限web应用程序使用;其它应用程序(如Job,Winform等)请自行实现ICurrentUserService)
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IEnumerable<Claim> _claims;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context"></param>
        public CurrentUserService(IHttpContextAccessor context)
        {
            _context = context;
            _claims = _context.HttpContext?.User?.Claims;
            //if (_context?.HttpContext?.User != null) 
            //{
            //    var sss = _context.HttpContext.User;
            //}


        }

        ///// <summary>
        ///// 获取当前登陆公司的Id
        ///// </summary>
        ///// <returns></returns>
        //public string GetCompanyId()
        //{
        //    var companyId = GetClaimValue(ClaimType.CompanyId);
        //    return companyId;
        //}
        ///// <summary>
        ///// 获取当前登陆公司社区号
        ///// </summary>
        ///// <returns></returns>
        //public int GetCommunityId()
        //{
        //    var strCommunityId = GetClaimValue(ClaimType.CommunityId);
        //    int.TryParse(strCommunityId, out int communityId);
        //    return communityId;
        //}

        /// <summary>
        /// 获取当前用户的头像
        /// </summary>
        /// <returns></returns>
        public string GetAvaterPath()
        {
            var avaId = GetClaimValue(ClaimType.AvaterPath);
            return avaId;
        }



        /// <summary>
        /// 获取当前登陆用户的Id
        /// </summary>
        /// <returns></returns>
        public string GetUserId()
        {
            var userId = GetClaimValue(ClaimType.UserId);
            return userId;
        }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            return bool.Parse(GetClaimValue(ClaimType.IsAdmin, bool.FalseString.ToLower()));
        }

        ///// <summary>
        ///// 是否是系统公司
        ///// </summary>
        ///// <returns></returns>
        //public bool IsSystemCompany()
        //{
        //    return bool.Parse(GetClaimValue(ClaimType.IsSystemCompany, bool.FalseString.ToLower()));
        //}

        /// <summary>
        /// 获取当前登陆用户信息
        /// </summary>
        /// <returns></returns>
        public CurrentUserDto GetCurrentUser()
        {
            var userId = GetClaimValue(ClaimType.UserId);
            var userName = GetClaimValue(ClaimType.UserName);
            var loginName = GetClaimValue(ClaimType.LoginName);
            var avaterPath = GetClaimValue(ClaimType.AvaterPath);
            var sub = GetClaimValue(ClaimType.Sub);
            var isAdmin = bool.Parse(GetClaimValue(ClaimType.IsAdmin, bool.FalseString.ToLower()));

            return new CurrentUserDto
            {
                UserId = userId,
                UserName = userName,
                LoginName = loginName,
                AvaterPath = avaterPath,
                Sub = sub,
                IsAdmin = isAdmin,
            };
        }

        /// <summary>
        /// 获取User认证信息中的Claim信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        private string GetClaimValue(string type, string defaultValue = "")
        {
            if (!_claims.AnyOne())
            {
                return defaultValue;
            }
            var value = _claims.FirstOrDefault(x => x.Type == type)?.Value;
            //尝试获取客户端携带的用户公司信息
            if (value.IsBlank())
            {
                value = _claims.FirstOrDefault(x => x.Type == "client_" + type)?.Value;
            }
            return value.IsBlank() ? defaultValue : value;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public string GetDepartmentId()
        //{
        //    var departmentId = GetClaimValue(ClaimType.DepartmentId);
        //    return departmentId;
        //}

        ///// <summary>
        ///// 获取登录单位Id
        ///// </summary>
        ///// <returns></returns>
        //public string GetOrganizationId()
        //{
        //    var organizationId = GetClaimValue(ClaimType.OrganizationId);
        //    return organizationId;
        //}


    }
}
