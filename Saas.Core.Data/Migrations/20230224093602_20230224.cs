using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saas.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20230224 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "sys_user",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "sys_user",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "sys_operation_log",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "sys_operation_log",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "sys_menu",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "sys_menu",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "sys_client",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "sys_client",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "mdm_xiaoai_speaker",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "mdm_xiaoai_speaker",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "mdm_message_receiver",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "mdm_message_receiver",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "mdm_message_group_receiver",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "mdm_message_group_receiver",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "mdm_message_group",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "mdm_message_group",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "mdm_home_persion",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "mdm_home_persion",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "mdm_camera",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "mdm_camera",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_work_task_record",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_work_task_record",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_weixin_user",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_weixin_user",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_wake_on_lan_record",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_wake_on_lan_record",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_remote_command",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_remote_command",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_pregnant_woman_event_record",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_pregnant_woman_event_record",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_pregnant_woman_eat_medicine_record",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_pregnant_woman_eat_medicine_record",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_notice_task",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_notice_task",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_notice_message",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_notice_message",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_lucky_draw_record",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_lucky_draw_record",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_lucky_draw",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_lucky_draw",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_jie_park",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_jie_park",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_interface_monitor",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_interface_monitor",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_home_financial_record",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_home_financial_record",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_chat_gpt_context",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_chat_gpt_context",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_book_subscription",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_book_subscription",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_blood_pressure_record",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_blood_pressure_record",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "bus_block_keyword",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "ModifyBy",
                table: "bus_block_keyword",
                newName: "UpdateBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "sys_user",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "sys_user",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "sys_operation_log",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "sys_operation_log",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "sys_menu",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "sys_menu",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "sys_client",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "sys_client",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "mdm_xiaoai_speaker",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "mdm_xiaoai_speaker",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "mdm_message_receiver",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "mdm_message_receiver",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "mdm_message_group_receiver",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "mdm_message_group_receiver",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "mdm_message_group",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "mdm_message_group",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "mdm_home_persion",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "mdm_home_persion",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "mdm_camera",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "mdm_camera",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_work_task_record",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_work_task_record",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_weixin_user",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_weixin_user",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_wake_on_lan_record",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_wake_on_lan_record",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_remote_command",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_remote_command",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_pregnant_woman_event_record",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_pregnant_woman_event_record",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_pregnant_woman_eat_medicine_record",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_pregnant_woman_eat_medicine_record",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_notice_task",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_notice_task",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_notice_message",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_notice_message",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_lucky_draw_record",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_lucky_draw_record",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_lucky_draw",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_lucky_draw",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_jie_park",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_jie_park",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_interface_monitor",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_interface_monitor",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_home_financial_record",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_home_financial_record",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_chat_gpt_context",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_chat_gpt_context",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_book_subscription",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_book_subscription",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_blood_pressure_record",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_blood_pressure_record",
                newName: "ModifyBy");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "bus_block_keyword",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "bus_block_keyword",
                newName: "ModifyBy");
        }
    }
}
