using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyLife.Migrations
{
    public partial class AddNewColumnInEarning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DoNotConsiderInTotal",
                table: "Earnings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoNotConsiderInTotal",
                table: "Earnings");
        }
    }
}
