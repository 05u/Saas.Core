﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Saas.Core.Data.Context;

#nullable disable

namespace Saas.Core.Data.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20230213102003_20230213-3")]
    partial class _202302133
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Saas.Core.Data.Entities.BlockKeyword", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("block_keyword");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.BloodPressureRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("BloodPressureResult")
                        .HasColumnType("int");

                    b.Property<int>("BloodPressureTimeFrame")
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("HighPressure")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("LowPressure")
                        .HasColumnType("int");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Pulse")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("blood_pressure_record");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.BookSubscription", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Identification")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsInStock")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MessageType")
                        .HasColumnType("int");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("book_subscription");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.Camera", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("HomeName")
                        .HasColumnType("longtext");

                    b.Property<string>("Ip")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<double>("OffAreaX")
                        .HasColumnType("double");

                    b.Property<double>("OffAreaY")
                        .HasColumnType("double");

                    b.Property<double>("OnAreaX")
                        .HasColumnType("double");

                    b.Property<double>("OnAreaY")
                        .HasColumnType("double");

                    b.Property<string>("Pass")
                        .HasColumnType("longtext");

                    b.Property<int?>("Port")
                        .HasColumnType("int");

                    b.Property<string>("User")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("camera");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.HomeFinancialRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FinancialAccountType")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("Money")
                        .HasColumnType("double");

                    b.Property<string>("Remark")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("home_financial_record");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.HomePersion", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsAtHome")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("JudgmentThreshold")
                        .HasColumnType("int");

                    b.Property<string>("Mac")
                        .HasColumnType("longtext");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("NotCheckedNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("home_persion");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.InterfaceMonitor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Cycle")
                        .HasColumnType("int");

                    b.Property<int>("InterfaceType")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsMonitoringAlarm")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MdmMessageGroupId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("NextTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Url")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("MdmMessageGroupId");

                    b.ToTable("interface_monitor");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.JiePark", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MessageGroupId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Mobile")
                        .HasColumnType("longtext");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ParkCode")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("MessageGroupId");

                    b.ToTable("jie_park");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.LuckyDraw", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsFinish")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MaxCount")
                        .HasColumnType("int");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("WinCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("lucky_draw");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.LuckyDrawRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GroupId")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsWin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LuckyDrawId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("MessageReceiverId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("No")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LuckyDrawId");

                    b.HasIndex("MessageReceiverId");

                    b.ToTable("lucky_draw_record");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.MdmMessageGroup", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("AllowQuickJoin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("mdm_message_group");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.MdmMessageGroupReceiver", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MessageGroupId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("MessageReceiverId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("MessageGroupId");

                    b.HasIndex("MessageReceiverId");

                    b.ToTable("mdm_message_group_receiver");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.MdmMessageReceiver", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Identification")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MessageType")
                        .HasColumnType("int");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("mdm_message_receiver");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.MdmXiaoaiSpeaker", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DeviceID")
                        .HasColumnType("longtext");

                    b.Property<string>("Hardware")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Mac")
                        .HasColumnType("longtext");

                    b.Property<string>("MiotDID")
                        .HasColumnType("longtext");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("mdm_xiaoai_speaker");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.NoticeMessage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FailureReasons")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("IsSendSuccess")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MessageType")
                        .HasColumnType("int");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MsgText")
                        .HasColumnType("longtext");

                    b.Property<string>("Receiver")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("SendTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Sender")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("notice_message");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.OperationLog", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("OperationType")
                        .HasColumnType("int");

                    b.Property<string>("RequestAddress")
                        .HasColumnType("longtext");

                    b.Property<string>("RequestIp")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("operation_log");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.PregnantWomanEatMedicineRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FailRemark")
                        .HasColumnType("longtext");

                    b.Property<int?>("Fines")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("IsEatSuccess")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MedicineName")
                        .HasColumnType("longtext");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("NoticeSecond")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("pregnant_woman_eat_medicine_record");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.PregnantWomanEventRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PregnantWomanEventType")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("pregnant_woman_event_record");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.RemoteCommand", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("ActionType")
                        .HasColumnType("int");

                    b.Property<string>("ClientMac")
                        .HasColumnType("longtext");

                    b.Property<string>("ClientName")
                        .HasColumnType("longtext");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("HeartbeatCycle")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsMonitoringAlarm")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastHeartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("LastRebootTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("LastShutdownTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool?>("RebootAction")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("ShutdownAction")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("remote_command");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.SysClient", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ClientId")
                        .HasColumnType("longtext");

                    b.Property<string>("ClientKey")
                        .HasColumnType("longtext");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Remark")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("sys_client");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.SysMenu", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Icon")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsSystem")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("ParentId")
                        .HasColumnType("longtext");

                    b.Property<string>("Remark")
                        .HasColumnType("longtext");

                    b.Property<int>("Sort")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("sys_menu");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.SysUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("AvaterPath")
                        .HasColumnType("longtext");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LoginName")
                        .HasColumnType("longtext");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<string>("Sub")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("sys_user");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.WakeOnLanRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Mac")
                        .HasColumnType("longtext");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Remark")
                        .HasColumnType("longtext");

                    b.Property<string>("SenderIP")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("wake_on_lan_record");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.WeixinUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Wxid")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("weixin_user");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.WorkTaskRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsNotice")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ModifyTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("PlanNoticeTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("WorkClassType")
                        .HasColumnType("int");

                    b.Property<DateTime>("WorkDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Wxid")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("work_task_record");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.InterfaceMonitor", b =>
                {
                    b.HasOne("Saas.Core.Data.Entities.MdmMessageGroup", "MdmMessageGroup")
                        .WithMany()
                        .HasForeignKey("MdmMessageGroupId");

                    b.Navigation("MdmMessageGroup");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.JiePark", b =>
                {
                    b.HasOne("Saas.Core.Data.Entities.MdmMessageGroup", "MessageGroup")
                        .WithMany()
                        .HasForeignKey("MessageGroupId");

                    b.Navigation("MessageGroup");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.LuckyDrawRecord", b =>
                {
                    b.HasOne("Saas.Core.Data.Entities.LuckyDraw", "LuckyDraw")
                        .WithMany("LuckyDrawRecords")
                        .HasForeignKey("LuckyDrawId");

                    b.HasOne("Saas.Core.Data.Entities.MdmMessageReceiver", "MessageReceiver")
                        .WithMany()
                        .HasForeignKey("MessageReceiverId");

                    b.Navigation("LuckyDraw");

                    b.Navigation("MessageReceiver");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.MdmMessageGroupReceiver", b =>
                {
                    b.HasOne("Saas.Core.Data.Entities.MdmMessageGroup", "MessageGroup")
                        .WithMany("MessageGroupReceivers")
                        .HasForeignKey("MessageGroupId");

                    b.HasOne("Saas.Core.Data.Entities.MdmMessageReceiver", "MessageReceiver")
                        .WithMany("MessageGroupReceivers")
                        .HasForeignKey("MessageReceiverId");

                    b.Navigation("MessageGroup");

                    b.Navigation("MessageReceiver");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.LuckyDraw", b =>
                {
                    b.Navigation("LuckyDrawRecords");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.MdmMessageGroup", b =>
                {
                    b.Navigation("MessageGroupReceivers");
                });

            modelBuilder.Entity("Saas.Core.Data.Entities.MdmMessageReceiver", b =>
                {
                    b.Navigation("MessageGroupReceivers");
                });
#pragma warning restore 612, 618
        }
    }
}
