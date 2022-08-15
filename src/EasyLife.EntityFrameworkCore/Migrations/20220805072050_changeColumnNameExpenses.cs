using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyLife.Migrations
{
    public partial class changeColumnNameExpenses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConsiderInTotal",
                table: "Expenses",
                newName: "DoNotConsiderInTotal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoNotConsiderInTotal",
                table: "Expenses",
                newName: "ConsiderInTotal");
        }
    }
}
