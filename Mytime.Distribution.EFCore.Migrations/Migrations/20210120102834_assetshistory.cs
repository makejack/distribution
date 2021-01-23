using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mytime.Distribution.EFCore.Migrations.Migrations
{
    public partial class assetshistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Message = table.Column<string>(maxLength: 512, nullable: true),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetsHistory_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetsHistory_CustomerId",
                table: "AssetsHistory",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetsHistory");
        }
    }
}
