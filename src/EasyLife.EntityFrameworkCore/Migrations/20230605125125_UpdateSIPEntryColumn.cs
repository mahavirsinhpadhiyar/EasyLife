using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyLife.Migrations
{
    public partial class UpdateSIPEntryColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Share_Units",
                table: "EL_Financial_Investment_SIP_Entry",
                newName: "SIP_Units");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SIP_Units",
                table: "EL_Financial_Investment_SIP_Entry",
                newName: "Share_Units");
        }
    }
}
