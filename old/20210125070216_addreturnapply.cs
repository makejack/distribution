using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mytime.Distribution.EFCore.Migrations.Migrations
{
    public partial class addreturnapply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnAddress_OrderItem_OrderItemId",
                table: "ReturnAddress");

            migrationBuilder.DropIndex(
                name: "IX_ReturnAddress_OrderItemId",
                table: "ReturnAddress");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "ReturnAddress");

            migrationBuilder.DropColumn(
                name: "CourierCompany",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "CourierCompanyCode",
                table: "OrderItem");

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
                name: "ShippingStatus",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ShippingTime",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "OrderItem");

            migrationBuilder.AddColumn<int>(
                name: "ReturnApplyId",
                table: "ReturnAddress",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "OrderItem",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "ReturnApply",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderItemId = table.Column<int>(nullable: false),
                    ShipmentId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ReturnType = table.Column<byte>(nullable: false),
                    LogisticsStatus = table.Column<byte>(nullable: false),
                    Reason = table.Column<string>(maxLength: 512, nullable: true),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    PaymentAmount = table.Column<int>(nullable: false),
                    RefundAmount = table.Column<int>(nullable: false),
                    RefundTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<byte>(nullable: false),
                    AuditTime = table.Column<DateTime>(nullable: true),
                    AuditMessage = table.Column<string>(maxLength: 512, nullable: true),
                    CourierCompany = table.Column<string>(maxLength: 512, nullable: true),
                    CourierCompanyCode = table.Column<string>(maxLength: 128, nullable: true),
                    TrackingNumber = table.Column<string>(maxLength: 32, nullable: true),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnApply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnApply_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnApply_OrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnApply_Shipment_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnAddress_ReturnApplyId",
                table: "ReturnAddress",
                column: "ReturnApplyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnApply_CustomerId",
                table: "ReturnApply",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnApply_OrderItemId",
                table: "ReturnApply",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnApply_ShipmentId",
                table: "ReturnApply",
                column: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnAddress_ReturnApply_ReturnApplyId",
                table: "ReturnAddress",
                column: "ReturnApplyId",
                principalTable: "ReturnApply",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnAddress_ReturnApply_ReturnApplyId",
                table: "ReturnAddress");

            migrationBuilder.DropTable(
                name: "ReturnApply");

            migrationBuilder.DropIndex(
                name: "IX_ReturnAddress_ReturnApplyId",
                table: "ReturnAddress");

            migrationBuilder.DropColumn(
                name: "ReturnApplyId",
                table: "ReturnAddress");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderItem");

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "ReturnAddress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CourierCompany",
                table: "OrderItem",
                type: "varchar(512) CHARACTER SET utf8mb4",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourierCompanyCode",
                table: "OrderItem",
                type: "varchar(128) CHARACTER SET utf8mb4",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RefundAmount",
                table: "OrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefundApplyTime",
                table: "OrderItem",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefundReason",
                table: "OrderItem",
                type: "varchar(512) CHARACTER SET utf8mb4",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RefundStatus",
                table: "OrderItem",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefundTime",
                table: "OrderItem",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ShippingStatus",
                table: "OrderItem",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingTime",
                table: "OrderItem",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingNumber",
                table: "OrderItem",
                type: "varchar(32) CHARACTER SET utf8mb4",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnAddress_OrderItemId",
                table: "ReturnAddress",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnAddress_OrderItem_OrderItemId",
                table: "ReturnAddress",
                column: "OrderItemId",
                principalTable: "OrderItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
