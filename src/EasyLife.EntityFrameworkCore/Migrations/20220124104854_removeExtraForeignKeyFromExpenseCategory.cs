using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyLife.Migrations
{
    public partial class removeExtraForeignKeyFromExpenseCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseCategory_ExpenseCategory_FKExpenseCategoryId",
                table: "ExpenseCategory");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseCategory_FKExpenseCategoryId",
                table: "ExpenseCategory");

            migrationBuilder.DropColumn(
                name: "FKExpenseCategoryId",
                table: "ExpenseCategory");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategory_ParentId",
                table: "ExpenseCategory",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseCategory_ExpenseCategory_ParentId",
                table: "ExpenseCategory",
                column: "ParentId",
                principalTable: "ExpenseCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseCategory_ExpenseCategory_ParentId",
                table: "ExpenseCategory");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseCategory_ParentId",
                table: "ExpenseCategory");

            migrationBuilder.AddColumn<Guid>(
                name: "FKExpenseCategoryId",
                table: "ExpenseCategory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategory_FKExpenseCategoryId",
                table: "ExpenseCategory",
                column: "FKExpenseCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseCategory_ExpenseCategory_FKExpenseCategoryId",
                table: "ExpenseCategory",
                column: "FKExpenseCategoryId",
                principalTable: "ExpenseCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
