using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saas.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class _20230213 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "camera",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "camera");
        }
    }
}
