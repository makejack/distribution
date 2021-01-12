using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mytime.Distribution.EFCore.Migrations.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    NickName = table.Column<string>(maxLength: 32, nullable: true),
                    Pwd = table.Column<string>(maxLength: 512, nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    Role = table.Column<byte>(nullable: false),
                    Tel = table.Column<string>(nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 32, nullable: true),
                    NickName = table.Column<string>(maxLength: 32, nullable: true),
                    OpenId = table.Column<string>(maxLength: 128, nullable: true),
                    SessionKey = table.Column<string>(maxLength: 32, nullable: true),
                    UnionId = table.Column<string>(maxLength: 512, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 32, nullable: true),
                    CountryCode = table.Column<string>(maxLength: 32, nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Country = table.Column<string>(maxLength: 512, nullable: true),
                    Province = table.Column<string>(maxLength: 512, nullable: true),
                    City = table.Column<string>(maxLength: 512, nullable: true),
                    AvatarUrl = table.Column<string>(maxLength: 512, nullable: true),
                    Language = table.Column<string>(maxLength: 32, nullable: true),
                    Role = table.Column<byte>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    BankNo = table.Column<string>(maxLength: 32, nullable: true),
                    BankCode = table.Column<string>(maxLength: 32, nullable: true),
                    IsRealName = table.Column<bool>(nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(maxLength: 512, nullable: false),
                    Method = table.Column<string>(maxLength: 32, nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(maxLength: 32, nullable: true),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodsOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsOption", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(maxLength: 512, nullable: false),
                    Type = table.Column<string>(maxLength: 32, nullable: false),
                    Size = table.Column<long>(nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartnerApply",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PartnerRole = table.Column<byte>(nullable: false),
                    ApplyType = table.Column<byte>(nullable: false),
                    TotalQuantity = table.Column<int>(nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerApply", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "RequestResponseLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(maxLength: 512, nullable: false),
                    Headers = table.Column<string>(nullable: true),
                    Method = table.Column<string>(maxLength: 32, nullable: true),
                    RequestBody = table.Column<string>(nullable: true),
                    ResponseBody = table.Column<string>(nullable: true),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestResponseLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tel = table.Column<string>(maxLength: 32, nullable: false),
                    Code = table.Column<string>(maxLength: 32, nullable: true),
                    MsgId = table.Column<string>(maxLength: 32, nullable: true),
                    Time = table.Column<string>(maxLength: 32, nullable: true),
                    Message = table.Column<string>(maxLength: 512, nullable: true),
                    ErrorMsg = table.Column<string>(maxLength: 512, nullable: true),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    TotalAssets = table.Column<int>(nullable: false),
                    AvailableAmount = table.Column<int>(nullable: false),
                    TotalCommission = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerRelation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(nullable: false),
                    ChildrenId = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerRelation_Customer_ChildrenId",
                        column: x => x.ChildrenId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerRelation_Customer_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    OrderNo = table.Column<string>(maxLength: 32, nullable: true),
                    TotalFee = table.Column<int>(nullable: false),
                    TotalWithDiscount = table.Column<int>(nullable: false),
                    OrderStatus = table.Column<byte>(nullable: false),
                    PaymentType = table.Column<byte>(nullable: false),
                    PaymentMethod = table.Column<byte>(nullable: false),
                    PaymentFee = table.Column<int>(nullable: false),
                    PaymentTime = table.Column<DateTime>(nullable: true),
                    RefundReason = table.Column<string>(maxLength: 512, nullable: true),
                    RefundTime = table.Column<DateTime>(nullable: true),
                    RefundFee = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(maxLength: 512, nullable: true),
                    CancelReason = table.Column<string>(nullable: true),
                    CancelTime = table.Column<DateTime>(nullable: true),
                    Createat = table.Column<DateTime>(nullable: false),
                    ExtendParams = table.Column<string>(nullable: true),
                    IsFistOrder = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    TotalWeight = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    CourierCompany = table.Column<string>(maxLength: 512, nullable: true),
                    CourierCompanyCode = table.Column<string>(maxLength: 512, nullable: true),
                    TrackingNumber = table.Column<string>(maxLength: 32, nullable: true),
                    Remarks = table.Column<string>(maxLength: 512, nullable: true),
                    ShippingStatus = table.Column<byte>(nullable: false),
                    ShippingTime = table.Column<DateTime>(nullable: true),
                    CompleteTime = table.Column<DateTime>(nullable: true),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipment_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawalHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    PartnerTradeNo = table.Column<string>(maxLength: 32, nullable: false),
                    Total = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    HandlingFee = table.Column<int>(nullable: false),
                    IsSuccess = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(maxLength: 512, nullable: true),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawalHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WithdrawalHistory_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodsOptionData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OptionId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsOptionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsOptionData_GoodsOption_OptionId",
                        column: x => x.OptionId,
                        principalTable: "GoodsOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Goods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 512, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    ThumbnailImageId = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    HasOptions = table.Column<bool>(nullable: false),
                    IsVisible = table.Column<bool>(nullable: false),
                    StockQuantity = table.Column<int>(nullable: false),
                    PublishedOn = table.Column<DateTime>(nullable: true),
                    CityDiscount = table.Column<int>(nullable: false),
                    BranchDiscount = table.Column<int>(nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goods_Goods_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Goods_Media_ThumbnailImageId",
                        column: x => x.ThumbnailImageId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    ProvinceCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Code);
                    table.ForeignKey(
                        name: "FK_City_Province_ProvinceCode",
                        column: x => x.ProvinceCode,
                        principalTable: "Province",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShippingAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShipmentId = table.Column<int>(nullable: false),
                    PostalCode = table.Column<string>(maxLength: 32, nullable: false),
                    ProvinceName = table.Column<string>(maxLength: 512, nullable: false),
                    CityName = table.Column<string>(maxLength: 512, nullable: false),
                    AreaName = table.Column<string>(maxLength: 512, nullable: false),
                    DetailInfo = table.Column<string>(maxLength: 512, nullable: false),
                    TelNumber = table.Column<string>(maxLength: 32, nullable: false),
                    UserName = table.Column<string>(maxLength: 32, nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingAddress_Shipment_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodsMedia",
                columns: table => new
                {
                    GoodsId = table.Column<int>(nullable: false),
                    MediaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsMedia", x => new { x.GoodsId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_GoodsMedia_Goods_GoodsId",
                        column: x => x.GoodsId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsMedia_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodsOptionCombination",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GoodsId = table.Column<int>(nullable: false),
                    OptionId = table.Column<int>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 32, nullable: true),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsOptionCombination", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsOptionCombination_Goods_GoodsId",
                        column: x => x.GoodsId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsOptionCombination_GoodsOption_OptionId",
                        column: x => x.OptionId,
                        principalTable: "GoodsOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoodsOptionValue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GoodsId = table.Column<int>(nullable: false),
                    OptionId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsOptionValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsOptionValue_Goods_GoodsId",
                        column: x => x.GoodsId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsOptionValue_GoodsOption_OptionId",
                        column: x => x.OptionId,
                        principalTable: "GoodsOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(nullable: false),
                    GoodsId = table.Column<int>(nullable: true),
                    GoodsItemId = table.Column<int>(nullable: true),
                    GoodsName = table.Column<string>(maxLength: 512, nullable: false),
                    GoodsPrice = table.Column<int>(nullable: false),
                    DiscountAmount = table.Column<int>(nullable: false),
                    GoodsMediaUrl = table.Column<string>(maxLength: 512, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 512, nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(maxLength: 512, nullable: true),
                    ShippingStatus = table.Column<byte>(nullable: false),
                    ShippingTime = table.Column<DateTime>(nullable: true),
                    CompleteTime = table.Column<DateTime>(nullable: true),
                    WarrantyDeadline = table.Column<DateTime>(nullable: true),
                    IsFirstBatchGoods = table.Column<bool>(nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Goods_GoodsId",
                        column: x => x.GoodsId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OrderItem_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartnerApplyGoods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PartnerApplyId = table.Column<int>(nullable: false),
                    GoodsId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerApplyGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerApplyGoods_Goods_GoodsId",
                        column: x => x.GoodsId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartnerApplyGoods_PartnerApply_PartnerApplyId",
                        column: x => x.PartnerApplyId,
                        principalTable: "PartnerApply",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    CityCode = table.Column<int>(nullable: true),
                    ProvinceCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Area_City_CityCode",
                        column: x => x.CityCode,
                        principalTable: "City",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Area_Province_ProvinceCode",
                        column: x => x.ProvinceCode,
                        principalTable: "Province",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommissionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    OrderItemId = table.Column<int>(nullable: true),
                    Commission = table.Column<int>(nullable: false),
                    Percentage = table.Column<int>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    SettlementTime = table.Column<DateTime>(nullable: true),
                    Createat = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommissionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommissionHistory_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommissionHistory_OrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentOrderItem",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(nullable: false),
                    OrderItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentOrderItem", x => new { x.OrderItemId, x.ShipmentId });
                    table.ForeignKey(
                        name: "FK_ShipmentOrderItem_OrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShipmentOrderItem_Shipment_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_CustomerAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Area_AreaCode",
                        column: x => x.AreaCode,
                        principalTable: "Area",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_City_CityCode",
                        column: x => x.CityCode,
                        principalTable: "City",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Province_ProvinceCode",
                        column: x => x.ProvinceCode,
                        principalTable: "Province",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUser_Name",
                table: "AdminUser",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Area_CityCode",
                table: "Area",
                column: "CityCode");

            migrationBuilder.CreateIndex(
                name: "IX_Area_ProvinceCode",
                table: "Area",
                column: "ProvinceCode");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CustomerId",
                table: "Assets",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_City_ProvinceCode",
                table: "City",
                column: "ProvinceCode");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionHistory_CustomerId",
                table: "CommissionHistory",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionHistory_OrderItemId",
                table: "CommissionHistory",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_OpenId",
                table: "Customer",
                column: "OpenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_AreaCode",
                table: "CustomerAddress",
                column: "AreaCode");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_CityCode",
                table: "CustomerAddress",
                column: "CityCode");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_CustomerId",
                table: "CustomerAddress",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_ProvinceCode",
                table: "CustomerAddress",
                column: "ProvinceCode");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRelation_ChildrenId",
                table: "CustomerRelation",
                column: "ChildrenId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRelation_ParentId",
                table: "CustomerRelation",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_ParentId",
                table: "Goods",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_ThumbnailImageId",
                table: "Goods",
                column: "ThumbnailImageId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsMedia_MediaId",
                table: "GoodsMedia",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsOptionCombination_GoodsId",
                table: "GoodsOptionCombination",
                column: "GoodsId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsOptionCombination_OptionId",
                table: "GoodsOptionCombination",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsOptionData_OptionId",
                table: "GoodsOptionData",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsOptionValue_GoodsId",
                table: "GoodsOptionValue",
                column: "GoodsId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsOptionValue_OptionId",
                table: "GoodsOptionValue",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_GoodsId",
                table: "OrderItem",
                column: "GoodsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNo",
                table: "Orders",
                column: "OrderNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartnerApplyGoods_GoodsId",
                table: "PartnerApplyGoods",
                column: "GoodsId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerApplyGoods_PartnerApplyId",
                table: "PartnerApplyGoods",
                column: "PartnerApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_CustomerId",
                table: "Shipment",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentOrderItem_ShipmentId",
                table: "ShipmentOrderItem",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_ShipmentId",
                table: "ShippingAddress",
                column: "ShipmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalHistory_CustomerId",
                table: "WithdrawalHistory",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUser");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "CommissionHistory");

            migrationBuilder.DropTable(
                name: "CustomerAddress");

            migrationBuilder.DropTable(
                name: "CustomerRelation");

            migrationBuilder.DropTable(
                name: "ErrorLog");

            migrationBuilder.DropTable(
                name: "GoodsMedia");

            migrationBuilder.DropTable(
                name: "GoodsOptionCombination");

            migrationBuilder.DropTable(
                name: "GoodsOptionData");

            migrationBuilder.DropTable(
                name: "GoodsOptionValue");

            migrationBuilder.DropTable(
                name: "PartnerApplyGoods");

            migrationBuilder.DropTable(
                name: "RequestResponseLog");

            migrationBuilder.DropTable(
                name: "ShipmentOrderItem");

            migrationBuilder.DropTable(
                name: "ShippingAddress");

            migrationBuilder.DropTable(
                name: "SmsLog");

            migrationBuilder.DropTable(
                name: "WithdrawalHistory");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "GoodsOption");

            migrationBuilder.DropTable(
                name: "PartnerApply");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Shipment");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Goods");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
