using Microsoft.EntityFrameworkCore;
using Saas.Core.Data.Entities;

namespace Saas.Core.Data.Context
{
    /// <summary>
    /// 主数据库上下文
    /// </summary>
    public class MainDbContext : DbContext
    {
        public DbSet<SysUser> SysUser { get; set; }
        public DbSet<SysClient> SysClient { get; set; }
        public DbSet<BusRemoteCommand> BusRemoteCommand { get; set; }
        public DbSet<BusNoticeMessage> BusNoticeMessage { get; set; }
        public DbSet<MdmXiaoaiSpeaker> MdmXiaoaiSpeaker { get; set; }
        public DbSet<BusPregnantWomanEatMedicineRecord> BusPregnantWomanEatMedicineRecord { get; set; }
        public DbSet<BusWakeOnLanRecord> BusWakeOnLanRecord { get; set; }
        public DbSet<BusBloodPressureRecord> BusBloodPressureRecord { get; set; }
        public DbSet<BusWorkTaskRecord> BusWorkTaskRecord { get; set; }
        public DbSet<BusWeixinUser> BusWeixinUser { get; set; }
        public DbSet<MdmMessageGroup> MdmMessageGroup { get; set; }
        public DbSet<MdmMessageReceiver> MdmMessageReceiver { get; set; }
        public DbSet<MdmMessageGroupReceiver> MdmMessageGroupReceiver { get; set; }
        public DbSet<BusBookSubscription> BusBookSubscription { get; set; }
        public DbSet<BusInterfaceMonitor> BusInterfaceMonitor { get; set; }
        public DbSet<SysOperationLog> SysOperationLog { get; set; }
        public DbSet<BusPregnantWomanEventRecord> BusPregnantWomanEventRecord { get; set; }
        public DbSet<BusHomeFinancialRecord> BusHomeFinancialRecord { get; set; }
        public DbSet<BusJiePark> BusJiePark { get; set; }
        public DbSet<MdmHomePersion> MdmHomePersion { get; set; }
        public DbSet<SysMenu> SysMenu { get; set; }
        public DbSet<MdmCamera> MdmCamera { get; set; }
        public DbSet<BusBlockKeyword> BusBlockKeyword { get; set; }
        public DbSet<BusLuckyDraw> BusLuckyDraw { get; set; }
        public DbSet<BusLuckyDrawRecord> BusLuckyDrawRecord { get; set; }
        public DbSet<BusNoticeTask> BusNoticeTask { get; set; }
        public DbSet<BusChatGptContext> BusChatGptContext { get; set; }

        



        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLoggerFactory(loggerFactory);
            //optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            //optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        ///// <summary>
        ///// OnModelCreating
        ///// </summary>
        ///// <param name="modelBuilder"></param>
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Test>(entity =>
        //    {
        //        entity.HasNoKey();//忽略数据库映射
        //    });
        //}
    }
}