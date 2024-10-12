using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyLife.Migrations
{
    public partial class addNotesInShareMasterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EL_Financial_Investment_Share_Orders_EL_Financial_Investment_Share_Master_EL_Financial_Investment_Share_Master_Id",
                table: "EL_Financial_Investment_Share_Orders");

            migrationBuilder.DropIndex(
                name: "IX_EL_Financial_Investment_Share_Orders_EL_Financial_Investment_Share_Master_Id",
                table: "EL_Financial_Investment_Share_Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "EL_Financial_Investment_Share_MasterId",
                table: "EL_Financial_Investment_Share_Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "EL_Financial_Investment_Share_Master",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EL_Financial_Investment_Share_Orders_EL_Financial_Investment_Share_MasterId",
                table: "EL_Financial_Investment_Share_Orders",
                column: "EL_Financial_Investment_Share_MasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_EL_Financial_Investment_Share_Orders_EL_Financial_Investment_Share_Master_EL_Financial_Investment_Share_MasterId",
                table: "EL_Financial_Investment_Share_Orders",
                column: "EL_Financial_Investment_Share_MasterId",
                principalTable: "EL_Financial_Investment_Share_Master",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EL_Financial_Investment_Share_Orders_EL_Financial_Investment_Share_Master_EL_Financial_Investment_Share_MasterId",
                table: "EL_Financial_Investment_Share_Orders");

            migrationBuilder.DropIndex(
                name: "IX_EL_Financial_Investment_Share_Orders_EL_Financial_Investment_Share_MasterId",
                table: "EL_Financial_Investment_Share_Orders");

            migrationBuilder.DropColumn(
                name: "EL_Financial_Investment_Share_MasterId",
                table: "EL_Financial_Investment_Share_Orders");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "EL_Financial_Investment_Share_Master");

            migrationBuilder.CreateIndex(
                name: "IX_EL_Financial_Investment_Share_Orders_EL_Financial_Investment_Share_Master_Id",
                table: "EL_Financial_Investment_Share_Orders",
                column: "EL_Financial_Investment_Share_Master_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EL_Financial_Investment_Share_Orders_EL_Financial_Investment_Share_Master_EL_Financial_Investment_Share_Master_Id",
                table: "EL_Financial_Investment_Share_Orders",
                column: "EL_Financial_Investment_Share_Master_Id",
                principalTable: "EL_Financial_Investment_Share_Master",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
