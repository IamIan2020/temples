using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Temples.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemSettingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "SystemSettings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "SystemSettings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "SystemSettings",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleMapUrl",
                table: "SystemSettings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LineUrl",
                table: "SystemSettings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "SystemSettings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "GoogleMapUrl",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "LineUrl",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "SystemSettings");
        }
    }
}
