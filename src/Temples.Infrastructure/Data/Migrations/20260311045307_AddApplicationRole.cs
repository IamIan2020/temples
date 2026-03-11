using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Temples.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetRoles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuiltIn",
                table: "AspNetRoles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsBuiltIn",
                table: "AspNetRoles");
        }
    }
}
