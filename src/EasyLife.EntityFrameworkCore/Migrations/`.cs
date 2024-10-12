using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyLife.Migrations
{
    public partial class AddMissingSIPEntryTableColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SIPType",
                table: "EL_Financial_Investment_SIP_Entry",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SIPType",
                table: "EL_Financial_Investment_SIP_Entry");
        }
    }
}
