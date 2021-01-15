using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mytime.Distribution.EFCore.Migrations.Migrations
{
    public partial class orderbilling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderBilling",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(nullable: false),
                    IsInvoiced = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 32, nullable: false),
                    TaxNumber = table.Column<string>(maxLength: 32, nullable: false),
                    CompanyAddress = table.Column<string>(maxLength: 512, nullable: true),
                    TelePhone = table.Column<string>(maxLength: 32, nullable: true),
                    BankName = table.Column<string>(maxLength: 512, nullable: true),
                    BankAccount = table.Column<string>(maxLength: 32, nullable: true),
                    Email = table.Column<string>(maxLength: 512, nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBilling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderBilling_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderBilling_OrderId",
                table: "OrderBilling",
                column: "OrderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderBilling");
        }
    }
}
