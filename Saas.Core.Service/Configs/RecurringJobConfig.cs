using Hangfire;
using Saas.Core.Service.Business;

namespace Saas.Core.Service.Configs
{
    /// <summary>
    /// 定时任务配置类
    /// </summary>
    public static class RecurringJobConfig
    {


        /// <summary>
        /// 定时任务配置
        /// </summary>
        public static void ConfigureRecurringJobs()
        {
            //cron表达式 使用UTC时间

            ////每天定时生成孕妇吃药计划 每天6点
            //RecurringJob.AddOrUpdate<PregnantWomanEatMedicineRecordService>("Job_PregnantWomanEatMedicineRecordCreat", x => x.PregnantWomanEatMedicineRecordCreat(), "0 0 22 1/1 * ? ");

            //每日0点重置ChatGPT上下文可用次数
            RecurringJob.AddOrUpdate<BusChatGptContextService>("Job_ResetAvailableCount", x => x.ResetAvailableCount(), "0 0 16 * * ? ");


            //每天定时发送吃药提醒 每30分钟一次
            RecurringJob.AddOrUpdate<BusPregnantWomanEatMedicineRecordService>("Job_SendEatMedicineNotice", x => x.SendEatMedicineNotice(), "0 0/30 * * * ? ");

            //监控服务 每分钟运行一次
            RecurringJob.AddOrUpdate<BusRemoteCommandService>("Job_MonitoringServices", x => x.MonitoringServices(), "0 * * * * ? ");
            RecurringJob.AddOrUpdate<HomeNetworkService>("Job_GetPensionAtHome", x => x.GetPensionAtHome(true), "0 * * * * ? ");
            RecurringJob.AddOrUpdate<ToolService>("Job_CheckIntranetPenetration", x => x.CheckIntranetPenetration(), "0 * * * * ? ");


            //获取和更新QQ机器人心跳 每分钟运行一次
            RecurringJob.AddOrUpdate<BusRemoteCommandService>("Job_GetQQRobotHeartbeat", x => x.GetQQRobotHeartbeat(), "0 * * * * ? ");

            //发送工作任务通知 每分钟运行一次
            RecurringJob.AddOrUpdate<BusWorkTaskRecordService>("Job_SendWorkTaskNotice", x => x.SendWorkTaskNotice(), "0 * * * * ? ");

            //图书馆订阅 每30分钟一次
            RecurringJob.AddOrUpdate<BusBookSubscriptionService>("Job_CheckStock", x => x.CheckStock(), "0 0/30 * * * ? ");

            //接口异常监控 每10分钟一次
            RecurringJob.AddOrUpdate<BusInterfaceMonitorService>("Job_CheckInterface", x => x.CheckInterface(), "0 0/10 * * * ? ");

            //每天停车场领券 8.30
            RecurringJob.AddOrUpdate<BusJieParkService>("Job_GetParkCoupon", x => x.GetParkCoupon(false), "0 30 0 1/1 * ? ");

            //抽奖 每分钟
            RecurringJob.AddOrUpdate<BusLuckyDrawService>("Job_FinishLuckyDraw", x => x.FinishLuckyDraw(), "0 * * * * ? ");

            //通知提醒 每分钟
            RecurringJob.AddOrUpdate<BusNoticeTaskService>("Job_SendNoticeTask", x => x.SendNoticeTask(), "0 * * * * ? ");

        }
    }
}
