using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mytime.Distribution.EFCore.Migrations.Migrations
{
    public partial class addbankcard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCode",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BankNo",
                table: "Customer");

            migrationBuilder.AddColumn<string>(
                name: "BankCode",
                table: "WithdrawalHistory",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankNo",
                table: "WithdrawalHistory",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WithdrawalHistory",
                maxLength: 32,
                nullable: false);

            migrationBuilder.CreateTable(
                name: "BankCard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 32, nullable: true),
                    BankCode = table.Column<string>(maxLength: 32, nullable: false),
                    BankNo = table.Column<string>(maxLength: 32, nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankCard_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankCard_CustomerId",
                table: "BankCard",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankCard");

            migrationBuilder.DropColumn(
                name: "BankCode",
                table: "WithdrawalHistory");

            migrationBuilder.DropColumn(
                name: "BankNo",
                table: "WithdrawalHistory");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "WithdrawalHistory");

            migrationBuilder.AddColumn<string>(
                name: "BankCode",
                table: "Customer",
                type: "varchar(32) CHARACTER SET utf8mb4",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankNo",
                table: "Customer",
                type: "varchar(32) CHARACTER SET utf8mb4",
                maxLength: 32,
                nullable: true);
        }
    }
}
