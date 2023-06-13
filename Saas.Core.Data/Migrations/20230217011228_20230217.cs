using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saas.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20230217 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_interface_monitor_mdm_message_group_MdmMessageGroupId",
                table: "interface_monitor");

            migrationBuilder.DropForeignKey(
                name: "FK_jie_park_mdm_message_group_MessageGroupId",
                table: "jie_park");

            migrationBuilder.DropForeignKey(
                name: "FK_lucky_draw_record_lucky_draw_LuckyDrawId",
                table: "lucky_draw_record");

            migrationBuilder.DropForeignKey(
                name: "FK_lucky_draw_record_mdm_message_receiver_MessageReceiverId",
                table: "lucky_draw_record");

            migrationBuilder.DropForeignKey(
                name: "FK_notice_task_mdm_message_receiver_MessageReceiverId",
                table: "notice_task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_work_task_record",
                table: "work_task_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_weixin_user",
                table: "weixin_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_wake_on_lan_record",
                table: "wake_on_lan_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_remote_command",
                table: "remote_command");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pregnant_woman_event_record",
                table: "pregnant_woman_event_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pregnant_woman_eat_medicine_record",
                table: "pregnant_woman_eat_medicine_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_operation_log",
                table: "operation_log");

            migrationBuilder.DropPrimaryKey(
                name: "PK_notice_task",
                table: "notice_task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_notice_message",
                table: "notice_message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lucky_draw_record",
                table: "lucky_draw_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lucky_draw",
                table: "lucky_draw");

            migrationBuilder.DropPrimaryKey(
                name: "PK_jie_park",
                table: "jie_park");

            migrationBuilder.DropPrimaryKey(
                name: "PK_interface_monitor",
                table: "interface_monitor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_home_persion",
                table: "home_persion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_home_financial_record",
                table: "home_financial_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_camera",
                table: "camera");

            migrationBuilder.DropPrimaryKey(
                name: "PK_book_subscription",
                table: "book_subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_blood_pressure_record",
                table: "blood_pressure_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_block_keyword",
                table: "block_keyword");

            migrationBuilder.RenameTable(
                name: "work_task_record",
                newName: "bus_work_task_record");

            migrationBuilder.RenameTable(
                name: "weixin_user",
                newName: "bus_weixin_user");

            migrationBuilder.RenameTable(
                name: "wake_on_lan_record",
                newName: "bus_wake_on_lan_record");

            migrationBuilder.RenameTable(
                name: "remote_command",
                newName: "bus_remote_command");

            migrationBuilder.RenameTable(
                name: "pregnant_woman_event_record",
                newName: "bus_pregnant_woman_event_record");

            migrationBuilder.RenameTable(
                name: "pregnant_woman_eat_medicine_record",
                newName: "bus_pregnant_woman_eat_medicine_record");

            migrationBuilder.RenameTable(
                name: "operation_log",
                newName: "sys_operation_log");

            migrationBuilder.RenameTable(
                name: "notice_task",
                newName: "bus_notice_task");

            migrationBuilder.RenameTable(
                name: "notice_message",
                newName: "bus_notice_message");

            migrationBuilder.RenameTable(
                name: "lucky_draw_record",
                newName: "bus_lucky_draw_record");

            migrationBuilder.RenameTable(
                name: "lucky_draw",
                newName: "bus_lucky_draw");

            migrationBuilder.RenameTable(
                name: "jie_park",
                newName: "bus_jie_park");

            migrationBuilder.RenameTable(
                name: "interface_monitor",
                newName: "bus_interface_monitor");

            migrationBuilder.RenameTable(
                name: "home_persion",
                newName: "mdm_home_persion");

            migrationBuilder.RenameTable(
                name: "home_financial_record",
                newName: "bus_home_financial_record");

            migrationBuilder.RenameTable(
                name: "camera",
                newName: "mdm_camera");

            migrationBuilder.RenameTable(
                name: "book_subscription",
                newName: "bus_book_subscription");

            migrationBuilder.RenameTable(
                name: "blood_pressure_record",
                newName: "bus_blood_pressure_record");

            migrationBuilder.RenameTable(
                name: "block_keyword",
                newName: "bus_block_keyword");

            migrationBuilder.RenameIndex(
                name: "IX_notice_task_MessageReceiverId",
                table: "bus_notice_task",
                newName: "IX_bus_notice_task_MessageReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_lucky_draw_record_MessageReceiverId",
                table: "bus_lucky_draw_record",
                newName: "IX_bus_lucky_draw_record_MessageReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_lucky_draw_record_LuckyDrawId",
                table: "bus_lucky_draw_record",
                newName: "IX_bus_lucky_draw_record_LuckyDrawId");

            migrationBuilder.RenameIndex(
                name: "IX_jie_park_MessageGroupId",
                table: "bus_jie_park",
                newName: "IX_bus_jie_park_MessageGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_interface_monitor_MdmMessageGroupId",
                table: "bus_interface_monitor",
                newName: "IX_bus_interface_monitor_MdmMessageGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_work_task_record",
                table: "bus_work_task_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_weixin_user",
                table: "bus_weixin_user",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_wake_on_lan_record",
                table: "bus_wake_on_lan_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_remote_command",
                table: "bus_remote_command",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_pregnant_woman_event_record",
                table: "bus_pregnant_woman_event_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_pregnant_woman_eat_medicine_record",
                table: "bus_pregnant_woman_eat_medicine_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sys_operation_log",
                table: "sys_operation_log",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_notice_task",
                table: "bus_notice_task",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_notice_message",
                table: "bus_notice_message",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_lucky_draw_record",
                table: "bus_lucky_draw_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_lucky_draw",
                table: "bus_lucky_draw",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_jie_park",
                table: "bus_jie_park",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_interface_monitor",
                table: "bus_interface_monitor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mdm_home_persion",
                table: "mdm_home_persion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_home_financial_record",
                table: "bus_home_financial_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mdm_camera",
                table: "mdm_camera",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_book_subscription",
                table: "bus_book_subscription",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_blood_pressure_record",
                table: "bus_blood_pressure_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bus_block_keyword",
                table: "bus_block_keyword",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bus_interface_monitor_mdm_message_group_MdmMessageGroupId",
                table: "bus_interface_monitor",
                column: "MdmMessageGroupId",
                principalTable: "mdm_message_group",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bus_jie_park_mdm_message_group_MessageGroupId",
                table: "bus_jie_park",
                column: "MessageGroupId",
                principalTable: "mdm_message_group",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bus_lucky_draw_record_bus_lucky_draw_LuckyDrawId",
                table: "bus_lucky_draw_record",
                column: "LuckyDrawId",
                principalTable: "bus_lucky_draw",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bus_lucky_draw_record_mdm_message_receiver_MessageReceiverId",
                table: "bus_lucky_draw_record",
                column: "MessageReceiverId",
                principalTable: "mdm_message_receiver",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bus_notice_task_mdm_message_receiver_MessageReceiverId",
                table: "bus_notice_task",
                column: "MessageReceiverId",
                principalTable: "mdm_message_receiver",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bus_interface_monitor_mdm_message_group_MdmMessageGroupId",
                table: "bus_interface_monitor");

            migrationBuilder.DropForeignKey(
                name: "FK_bus_jie_park_mdm_message_group_MessageGroupId",
                table: "bus_jie_park");

            migrationBuilder.DropForeignKey(
                name: "FK_bus_lucky_draw_record_bus_lucky_draw_LuckyDrawId",
                table: "bus_lucky_draw_record");

            migrationBuilder.DropForeignKey(
                name: "FK_bus_lucky_draw_record_mdm_message_receiver_MessageReceiverId",
                table: "bus_lucky_draw_record");

            migrationBuilder.DropForeignKey(
                name: "FK_bus_notice_task_mdm_message_receiver_MessageReceiverId",
                table: "bus_notice_task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sys_operation_log",
                table: "sys_operation_log");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mdm_home_persion",
                table: "mdm_home_persion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mdm_camera",
                table: "mdm_camera");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_work_task_record",
                table: "bus_work_task_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_weixin_user",
                table: "bus_weixin_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_wake_on_lan_record",
                table: "bus_wake_on_lan_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_remote_command",
                table: "bus_remote_command");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_pregnant_woman_event_record",
                table: "bus_pregnant_woman_event_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_pregnant_woman_eat_medicine_record",
                table: "bus_pregnant_woman_eat_medicine_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_notice_task",
                table: "bus_notice_task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_notice_message",
                table: "bus_notice_message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_lucky_draw_record",
                table: "bus_lucky_draw_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_lucky_draw",
                table: "bus_lucky_draw");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_jie_park",
                table: "bus_jie_park");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_interface_monitor",
                table: "bus_interface_monitor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_home_financial_record",
                table: "bus_home_financial_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_book_subscription",
                table: "bus_book_subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_blood_pressure_record",
                table: "bus_blood_pressure_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bus_block_keyword",
                table: "bus_block_keyword");

            migrationBuilder.RenameTable(
                name: "sys_operation_log",
                newName: "operation_log");

            migrationBuilder.RenameTable(
                name: "mdm_home_persion",
                newName: "home_persion");

            migrationBuilder.RenameTable(
                name: "mdm_camera",
                newName: "camera");

            migrationBuilder.RenameTable(
                name: "bus_work_task_record",
                newName: "work_task_record");

            migrationBuilder.RenameTable(
                name: "bus_weixin_user",
                newName: "weixin_user");

            migrationBuilder.RenameTable(
                name: "bus_wake_on_lan_record",
                newName: "wake_on_lan_record");

            migrationBuilder.RenameTable(
                name: "bus_remote_command",
                newName: "remote_command");

            migrationBuilder.RenameTable(
                name: "bus_pregnant_woman_event_record",
                newName: "pregnant_woman_event_record");

            migrationBuilder.RenameTable(
                name: "bus_pregnant_woman_eat_medicine_record",
                newName: "pregnant_woman_eat_medicine_record");

            migrationBuilder.RenameTable(
                name: "bus_notice_task",
                newName: "notice_task");

            migrationBuilder.RenameTable(
                name: "bus_notice_message",
                newName: "notice_message");

            migrationBuilder.RenameTable(
                name: "bus_lucky_draw_record",
                newName: "lucky_draw_record");

            migrationBuilder.RenameTable(
                name: "bus_lucky_draw",
                newName: "lucky_draw");

            migrationBuilder.RenameTable(
                name: "bus_jie_park",
                newName: "jie_park");

            migrationBuilder.RenameTable(
                name: "bus_interface_monitor",
                newName: "interface_monitor");

            migrationBuilder.RenameTable(
                name: "bus_home_financial_record",
                newName: "home_financial_record");

            migrationBuilder.RenameTable(
                name: "bus_book_subscription",
                newName: "book_subscription");

            migrationBuilder.RenameTable(
                name: "bus_blood_pressure_record",
                newName: "blood_pressure_record");

            migrationBuilder.RenameTable(
                name: "bus_block_keyword",
                newName: "block_keyword");

            migrationBuilder.RenameIndex(
                name: "IX_bus_notice_task_MessageReceiverId",
                table: "notice_task",
                newName: "IX_notice_task_MessageReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_bus_lucky_draw_record_MessageReceiverId",
                table: "lucky_draw_record",
                newName: "IX_lucky_draw_record_MessageReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_bus_lucky_draw_record_LuckyDrawId",
                table: "lucky_draw_record",
                newName: "IX_lucky_draw_record_LuckyDrawId");

            migrationBuilder.RenameIndex(
                name: "IX_bus_jie_park_MessageGroupId",
                table: "jie_park",
                newName: "IX_jie_park_MessageGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_bus_interface_monitor_MdmMessageGroupId",
                table: "interface_monitor",
                newName: "IX_interface_monitor_MdmMessageGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_operation_log",
                table: "operation_log",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_home_persion",
                table: "home_persion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_camera",
                table: "camera",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_work_task_record",
                table: "work_task_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_weixin_user",
                table: "weixin_user",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_wake_on_lan_record",
                table: "wake_on_lan_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_remote_command",
                table: "remote_command",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pregnant_woman_event_record",
                table: "pregnant_woman_event_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pregnant_woman_eat_medicine_record",
                table: "pregnant_woman_eat_medicine_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notice_task",
                table: "notice_task",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notice_message",
                table: "notice_message",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lucky_draw_record",
                table: "lucky_draw_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lucky_draw",
                table: "lucky_draw",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_jie_park",
                table: "jie_park",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_interface_monitor",
                table: "interface_monitor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_home_financial_record",
                table: "home_financial_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_book_subscription",
                table: "book_subscription",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_blood_pressure_record",
                table: "blood_pressure_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_block_keyword",
                table: "block_keyword",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_interface_monitor_mdm_message_group_MdmMessageGroupId",
                table: "interface_monitor",
                column: "MdmMessageGroupId",
                principalTable: "mdm_message_group",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_jie_park_mdm_message_group_MessageGroupId",
                table: "jie_park",
                column: "MessageGroupId",
                principalTable: "mdm_message_group",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_lucky_draw_record_lucky_draw_LuckyDrawId",
                table: "lucky_draw_record",
                column: "LuckyDrawId",
                principalTable: "lucky_draw",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_lucky_draw_record_mdm_message_receiver_MessageReceiverId",
                table: "lucky_draw_record",
                column: "MessageReceiverId",
                principalTable: "mdm_message_receiver",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_notice_task_mdm_message_receiver_MessageReceiverId",
                table: "notice_task",
                column: "MessageReceiverId",
                principalTable: "mdm_message_receiver",
                principalColumn: "Id");
        }
    }
}
