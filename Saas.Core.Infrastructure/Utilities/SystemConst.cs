namespace Saas.Core.Infrastructure.Utilities
{
    public class SystemConst
    {

        #region 缓存KEY名称
        public const string IkuaiCookie = "Ikuai.Cookie";
        #endregion


        #region 消息队列名称
        public const string NoticeMessage = "System.NoticeMessage";
        #endregion

    }


    public static class HttpClientConst
    {

        /// <summary>
        /// /
        /// </summary>
        public static string IdentityServer4 = nameof(IdentityServer4);

        /// <summary>
        /// 
        /// </summary>
        public static string CommonClient = nameof(CommonClient);


    }


    public static class SystemInfo
    {
        public static DateTime SystemStartTime { get; set; } = DateTime.Now;
    }
}
