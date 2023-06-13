using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saas.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class _202302133 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lucky_draw_record_mdm_message_receiver_MdmMessageReceiverId",
                table: "lucky_draw_record");

            migrationBuilder.RenameColumn(
                name: "MdmMessageReceiverId",
                table: "lucky_draw_record",
                newName: "MessageReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_lucky_draw_record_MdmMessageReceiverId",
                table: "lucky_draw_record",
                newName: "IX_lucky_draw_record_MessageReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_lucky_draw_record_mdm_message_receiver_MessageReceiverId",
                table: "lucky_draw_record",
                column: "MessageReceiverId",
                principalTable: "mdm_message_receiver",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lucky_draw_record_mdm_message_receiver_MessageReceiverId",
                table: "lucky_draw_record");

            migrationBuilder.RenameColumn(
                name: "MessageReceiverId",
                table: "lucky_draw_record",
                newName: "MdmMessageReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_lucky_draw_record_MessageReceiverId",
                table: "lucky_draw_record",
                newName: "IX_lucky_draw_record_MdmMessageReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_lucky_draw_record_mdm_message_receiver_MdmMessageReceiverId",
                table: "lucky_draw_record",
                column: "MdmMessageReceiverId",
                principalTable: "mdm_message_receiver",
                principalColumn: "Id");
        }
    }
}
