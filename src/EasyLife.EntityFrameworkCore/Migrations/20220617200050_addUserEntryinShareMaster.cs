using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyLife.Migrations
{
    public partial class addUserEntryinShareMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "EL_Financial_Investment_Share_Master",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_EL_Financial_Investment_Share_Master_UserId",
                table: "EL_Financial_Investment_Share_Master",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EL_Financial_Investment_Share_Master_AbpUsers_UserId",
                table: "EL_Financial_Investment_Share_Master",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EL_Financial_Investment_Share_Master_AbpUsers_UserId",
                table: "EL_Financial_Investment_Share_Master");

            migrationBuilder.DropIndex(
                name: "IX_EL_Financial_Investment_Share_Master_UserId",
                table: "EL_Financial_Investment_Share_Master");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EL_Financial_Investment_Share_Master");
        }
    }
}
