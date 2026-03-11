using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Temples.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceItemOptionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ServiceItemOptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ServiceItemOptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ServiceItemOptions",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ServiceItemOptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ServiceItemOptions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ServiceItemOptions");
        }
    }
}
