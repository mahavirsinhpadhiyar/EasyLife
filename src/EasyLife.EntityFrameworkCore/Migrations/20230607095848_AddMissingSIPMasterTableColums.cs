using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyLife.Migrations
{
    public partial class AddMissingSIPMasterTableColums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AutoInstallmentDate",
                table: "EL_Financial_Investment_SIP_Master",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AutopayID",
                table: "EL_Financial_Investment_SIP_Master",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SIPID",
                table: "EL_Financial_Investment_SIP_Master",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoInstallmentDate",
                table: "EL_Financial_Investment_SIP_Master");

            migrationBuilder.DropColumn(
                name: "AutopayID",
                table: "EL_Financial_Investment_SIP_Master");

            migrationBuilder.DropColumn(
                name: "SIPID",
                table: "EL_Financial_Investment_SIP_Master");
        }
    }
}
