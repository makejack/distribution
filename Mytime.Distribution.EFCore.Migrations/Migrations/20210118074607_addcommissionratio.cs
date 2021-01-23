using Microsoft.EntityFrameworkCore.Migrations;

namespace Mytime.Distribution.EFCore.Migrations.Migrations
{
    public partial class addcommissionratio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferralCommissionRatio",
                table: "PartnerApply",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RepurchaseCommissionRatio",
                table: "PartnerApply",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ParentId",
                table: "Customer",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Customer_ParentId",
                table: "Customer",
                column: "ParentId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Customer_ParentId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_ParentId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ReferralCommissionRatio",
                table: "PartnerApply");

            migrationBuilder.DropColumn(
                name: "RepurchaseCommissionRatio",
                table: "PartnerApply");
        }
    }
}
