using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyLife.Migrations
{
    public partial class addBoolFieldsInEarningAndExpenseCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ExpenseCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsForMeActive",
                table: "ExpenseCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EarningCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsForMeActive",
                table: "EarningCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ExpenseCategory");

            migrationBuilder.DropColumn(
                name: "IsForMeActive",
                table: "ExpenseCategory");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EarningCategory");

            migrationBuilder.DropColumn(
                name: "IsForMeActive",
                table: "EarningCategory");
        }
    }
}
