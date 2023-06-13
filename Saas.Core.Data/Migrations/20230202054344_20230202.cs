using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saas.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20230202 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomeName",
                table: "camera",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeName",
                table: "camera");
        }
    }
}
