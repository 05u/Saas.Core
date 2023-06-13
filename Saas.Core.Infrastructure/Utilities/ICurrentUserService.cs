namespace Saas.Core.Infrastructure.Utilities
{

    /// <summary>
    /// 当前登陆用户信息
    /// </summary>
    public class CurrentUserDto
    {

        /// <summary>
        /// 登陆用户Id(或客户端Id)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登陆账号
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string AvaterPath { get; set; }

        /// <summary>
        /// 身份
        /// </summary>
        public string Sub { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }


    }

    /// <summary>
    /// 全局用户获取接口(获取当前登陆用户的公司Id，需要使用者自己去实现这个接口)
    /// </summary>
    public interface ICurrentUserService
    {
        ///// <summary>
        ///// 获取公司Id
        ///// </summary>
        ///// <returns></returns>
        //string GetCompanyId();


        ///// <summary>
        ///// 获取公司的社区号
        ///// </summary>
        ///// <returns></returns>
        //int GetCommunityId();

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //string GetDepartmentId();

        /// <summary>
        /// 获取当前登陆用户的Id
        /// </summary>
        /// <returns></returns>
        string GetUserId();

        /// <summary>
        /// 是否是管理员
        /// </summary>
        /// <returns></returns>
        bool IsAdmin();

        ///// <summary>
        ///// 获取当前登陆用户的Id
        ///// </summary>
        ///// <returns></returns>
        //bool IsSystemCompany();

        /// <summary>
        /// 获取当前登陆用户
        /// </summary>
        /// <returns></returns>
        CurrentUserDto GetCurrentUser();

        ///// <summary>
        ///// 获取组织Id
        ///// </summary>
        ///// <returns></returns>
        //string GetOrganizationId();

    }
}
