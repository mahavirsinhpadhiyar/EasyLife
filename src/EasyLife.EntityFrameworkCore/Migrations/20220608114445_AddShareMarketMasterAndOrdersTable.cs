using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyLife.Migrations
{
    public partial class AddShareMarketMasterAndOrdersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EL_Financial_Investment_Share_Master",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Share_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EL_Financial_Investment_Share_Master", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EL_Financial_Investment_Share_Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Share_Order_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Share_Average_Price = table.Column<double>(type: "float", nullable: false),
                    Share_Amount = table.Column<int>(type: "int", nullable: false),
                    Share_Order_Type = table.Column<int>(type: "int", nullable: false),
                    Share_Qty_Exchange_Type = table.Column<int>(type: "int", nullable: false),
                    Share_Price_Type = table.Column<int>(type: "int", nullable: false),
                    Share_Quantity = table.Column<int>(type: "int", nullable: false),
                    Share_Order_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Share_Transaction_Type = table.Column<int>(type: "int", nullable: false),
                    Share_App_Order_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EL_Financial_Investment_Share_Master_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EL_Financial_Investment_Share_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EL_Financial_Investment_Share_Orders_EL_Financial_Investment_Share_Master_EL_Financial_Investment_Share_Master_Id",
                        column: x => x.EL_Financial_Investment_Share_Master_Id,
                        principalTable: "EL_Financial_Investment_Share_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EL_Financial_Investment_Share_Orders_EL_Financial_Investment_Share_Master_Id",
                table: "EL_Financial_Investment_Share_Orders",
                column: "EL_Financial_Investment_Share_Master_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EL_Financial_Investment_Share_Orders");

            migrationBuilder.DropTable(
                name: "EL_Financial_Investment_Share_Master");
        }
    }
}
