using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyLife.Migrations
{
    public partial class AddSIPMasterAndEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EL_Financial_Investment_SIP_Master",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SIP_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SIP_Folio_No = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_EL_Financial_Investment_SIP_Master", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EL_Financial_Investment_SIP_Master_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EL_Financial_Investment_SIP_Entry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SIP_Order_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SIP_Average_Price = table.Column<double>(type: "float", nullable: false),
                    SIP_Amount = table.Column<double>(type: "float", nullable: false),
                    Share_Units = table.Column<double>(type: "float", nullable: false),
                    SIP_Order_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EL_Financial_Investment_SIP_Master_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_EL_Financial_Investment_SIP_Entry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EL_Financial_Investment_SIP_Entry_EL_Financial_Investment_SIP_Master_EL_Financial_Investment_SIP_Master_Id",
                        column: x => x.EL_Financial_Investment_SIP_Master_Id,
                        principalTable: "EL_Financial_Investment_SIP_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EL_Financial_Investment_SIP_Entry_EL_Financial_Investment_SIP_Master_Id",
                table: "EL_Financial_Investment_SIP_Entry",
                column: "EL_Financial_Investment_SIP_Master_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EL_Financial_Investment_SIP_Master_UserId",
                table: "EL_Financial_Investment_SIP_Master",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EL_Financial_Investment_SIP_Entry");

            migrationBuilder.DropTable(
                name: "EL_Financial_Investment_SIP_Master");
        }
    }
}
