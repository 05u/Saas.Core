namespace Saas.Core.Infrastructure.Utilities
{
    public static class Constants
    {
        public static class Log4net
        {
            /// <summary>
            /// RepositoryName
            /// </summary>
            public static string RepositoryName = "OxygenCoreRepository";

            /// <summary>
            /// LoggerName
            /// </summary>
            public static string LoggerName = "SystemError";
        }

        public static class ClaimType
        {
            public const string UserId = nameof(UserId);
            public const string UserName = nameof(UserName);
            public const string LoginName = nameof(LoginName);
            public const string AvaterPath = nameof(AvaterPath);
            public const string Phone = nameof(Phone);
            public const string Email = nameof(Email);
            public const string Sub = nameof(Sub);
            public const string IsAdmin = nameof(IsAdmin);
        }

        public static class DatabaseConstValue
        {
            public const string SqlServerDbType = "SqlServer";

            public const string MySqlDbType = "MySql";

            public const string PostgresSqlDbType = "PostgresSql";
        }


        public static class DateFormatType
        {
            public const string LongDateString = "yyyy-MM-dd HH:mm:ss";

            public const string ShortDateString = "yyyy-MM-dd";

            public const string LongDateString2 = "yyyyMMddHHmmss";
            public const string ExcelLongDateString = "yyyy-m-d h:mm:ss";

        }


        public static class CachePrefix
        {
            /// <summary>
            /// 用户名前缀
            /// </summary>
            public const string UserID = nameof(UserID);

            /// <summary>
            /// 短信验证码
            /// </summary>
            public const string SMS = nameof(SMS);

            /// <summary>
            /// 用户输入手机号码验证次数限制
            /// </summary>
            public const string UserMobileNoCheck = nameof(UserMobileNoCheck);

            /// <summary>
            /// 用户请求短信验证次数限制
            /// </summary>
            public const string UserMobileNoSMS = nameof(UserMobileNoSMS);
            /// <summary>
            /// 未注册用户请求短信验证次数限制
            /// </summary>
            public const string UnregisterUserMobileNoSMS = nameof(UnregisterUserMobileNoSMS);


            /// <summary>
            /// 
            /// </summary>
            public const string AndonEventVoiceReplay = nameof(AndonEventVoiceReplay);



        }


    }
}
