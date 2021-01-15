using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mytime.Distribution.EFCore.Migrations.Migrations
{
    public partial class refund : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RefundAmount",
                table: "OrderItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefundApplyTime",
                table: "OrderItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefundReason",
                table: "OrderItem",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RefundStatus",
                table: "OrderItem",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefundTime",
                table: "OrderItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourierCompany",
                maxLength: 512,
                table: "OrderItem",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "CourierCompanyCode",
                table: "OrderItem",
                maxLength: 128,
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "TrackingNumber",
                table: "OrderItem",
                maxLength: 32,
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "AdminAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDefault = table.Column<bool>(nullable: false),
                    PostalCode = table.Column<int>(nullable: false),
                    ProvinceCode = table.Column<int>(nullable: false),
                    CityCode = table.Column<int>(nullable: false),
                    AreaCode = table.Column<int>(nullable: false),
                    DetailInfo = table.Column<string>(maxLength: 512, nullable: false),
                    TelNumber = table.Column<string>(maxLength: 32, nullable: false),
                    UserName = table.Column<string>(maxLength: 128, nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminAddress_Area_AreaCode",
                        column: x => x.AreaCode,
                        principalTable: "Area",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdminAddress_City_CityCode",
                        column: x => x.CityCode,
                        principalTable: "City",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdminAddress_Province_ProvinceCode",
                        column: x => x.ProvinceCode,
                        principalTable: "Province",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderItemId = table.Column<int>(nullable: false),
                    PostalCode = table.Column<string>(maxLength: 32, nullable: false),
                    ProvinceName = table.Column<string>(maxLength: 512, nullable: false),
                    CityName = table.Column<string>(maxLength: 512, nullable: false),
                    AreaName = table.Column<string>(maxLength: 512, nullable: false),
                    DetailInfo = table.Column<string>(maxLength: 512, nullable: false),
                    TelNumber = table.Column<string>(maxLength: 32, nullable: false),
                    UserName = table.Column<string>(maxLength: 32, nullable: false),
                    Remarks = table.Column<string>(maxLength: 512, nullable: true),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnAddress_OrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminAddress_AreaCode",
                table: "AdminAddress",
                column: "AreaCode");

            migrationBuilder.CreateIndex(
                name: "IX_AdminAddress_CityCode",
                table: "AdminAddress",
                column: "CityCode");

            migrationBuilder.CreateIndex(
                name: "IX_AdminAddress_ProvinceCode",
                table: "AdminAddress",
                column: "ProvinceCode");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnAddress_OrderItemId",
                table: "ReturnAddress",
                column: "OrderItemId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminAddress");

            migrationBuilder.DropTable(
                name: "ReturnAddress");

            migrationBuilder.DropColumn(
                name: "RefundAmount",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "RefundApplyTime",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "RefundReason",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "RefundStatus",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "RefundTime",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "CourierCompany",
                table: "OrderItem"
            );

            migrationBuilder.DropColumn(
                name: "CourierCompanyCode",
                table: "OrderItem"
            );

            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "OrderItem"
            );
        }
    }
}
