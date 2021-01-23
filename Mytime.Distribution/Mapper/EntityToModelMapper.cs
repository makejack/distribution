using System;
using System.Runtime.InteropServices;
using System.Linq;
using AutoMapper;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Models.V1.Response;

namespace Mytime.Distribution.Mapper
{
    /// <summary>
    /// 实体转模型
    /// </summary>
    public class EntityToModelMapper : Profile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntityToModelMapper()
        {
            CreateMap<Media, MediaResponse>();
            CreateMap<AdminUser, AdminUserResponse>();
            CreateMap<AdminUser, AdminEmployeeResponse>();
            CreateMap<AdminAddress, CustomerAddressResponse>()
            .ForMember(s => s.ProvinceName, o => o.MapFrom(s => s.Province.Name))
            .ForMember(s => s.CityName, o => o.MapFrom(s => s.City.Name))
            .ForMember(s => s.AreaName, o => o.MapFrom(s => s.Area.Name));

            CreateMap<Category, AdminCategoryResponse>();
            CreateMap<GoodsOption, AdminGoodsOptionResponse>();
            CreateMap<GoodsOptionData, AdminGoodsOptionDataResponse>();
            CreateMap<Goods, AdminGoodsGetResponse>()
            .ForMember(s => s.ThumbnailImageUrl, o => o.MapFrom(s => s.ThumbnailImage.Url))
            .ForMember(s => s.GoodsMedias, o => o.MapFrom(s => s.GoodsMedias.Select(e => e.Media)));
            CreateMap<Goods, AdminGoodsListResponse>()
            .ForMember(s => s.ThumbnailImageUrl, o => o.MapFrom(s => s.ThumbnailImage.Url));
            CreateMap<Goods, GoodsGetResponse>()
            .ForMember(s => s.ThumbnailImageUrl, o => o.MapFrom(s => s.ThumbnailImage.Url))
            .ForMember(s => s.GoodsMedias, o => o.MapFrom(s => s.GoodsMedias.Select(e => e.Media)));
            CreateMap<Goods, GoodsListResponse>()
            .ForMember(s => s.ThumbnailImageUrl, o => o.MapFrom(s => s.ThumbnailImage.Url))
            .ForMember(s => s.GoodsMedias, o => o.MapFrom(s => s.GoodsMedias.Select(e => e.Media)));

            CreateMap<Customer, CustomerResponse>();
            CreateMap<Customer, AdminCustomerGetResponse>();
            CreateMap<Customer, CustomerTeamResponse>();
            CreateMap<CustomerAddress, CustomerAddressResponse>()
            .ForMember(s => s.ProvinceName, o => o.MapFrom(s => s.Province.Name))
            .ForMember(s => s.CityName, o => o.MapFrom(s => s.City.Name))
            .ForMember(s => s.AreaName, o => o.MapFrom(s => s.Area.Name));
            CreateMap<Assets, AssetsResponse>();
            CreateMap<WithdrawalHistory, WithdrawalHistoryResponse>();
            CreateMap<WithdrawalHistory, AdminWithdrawalHistoryResponse>();
            CreateMap<CommissionHistory, CommissionHistoryResponse>();
            CreateMap<CommissionHistory, AdminCommissionHistoryResponse>();
            CreateMap<BankCard, BankCardResponse>();

            CreateMap<PartnerApply, AdminPartnerApplyGetResponse>()
            .ForMember(s => s.Goods, o => o.MapFrom(s => s.PartnerApplyGoods));
            CreateMap<PartnerApply, PartnerApplyConditionResponse>()
            .ForMember(s => s.Goods, o => o.MapFrom(s => s.PartnerApplyGoods));
            CreateMap<PartnerApply, AdminPartnerApplyListResponse>();
            CreateMap<PartnerApplyGoods, AdminPartnerApplyGoodsGetResponse>()
            .ForMember(s => s.Name, o => o.MapFrom(s => s.Goods.Name))
            .ForMember(s => s.OriginalPrice, o => o.MapFrom(s => s.Goods.Price))
            .ForMember(s => s.ThumbnailImageUrl, o => o.MapFrom(s => s.Goods.ThumbnailImage.Url));

            CreateMap<Orders, OrderCreateResponse>();
            CreateMap<Orders, AdminOrderListResponse>();
            CreateMap<Orders, AdminOrderGetResponse>()
            .ForMember(s => s.Items, o => o.MapFrom(s => s.OrderItems))
            .ForMember(s => s.Billing, o => o.MapFrom(s => s.OrderBilling));
            CreateMap<OrderItem, AdminOrderItemResponse>();
            CreateMap<OrderItem, AdminRefundListResponse>()
            .ForMember(s => s.OrderNo, o => o.MapFrom(s => s.Order.OrderNo))
            .ForMember(s => s.CustomerId, o => o.MapFrom(s => s.Order.CustomerId))
            .ForMember(s => s.CustomerName, o => o.MapFrom(s => s.Order.Customer.NickName))
            .ForMember(s => s.AvatarUrl, o => o.MapFrom(s => s.Order.Customer.AvatarUrl));
            CreateMap<OrderItem, ShipmentListOrderItemResponse>();
            CreateMap<OrderItem, ShipmentPendingListResponse>();
            CreateMap<OrderItem, RefundGetResponse>()
            .ForMember(s => s.Address, o => o.MapFrom(s => s.ReturnAddress));
            CreateMap<ReturnAddress, ReturnAddressResponse>();
            CreateMap<OrderBilling, AdminOrderBillingResponse>();

            CreateMap<Shipment, AdminShipmentListResponse>()
            .ForMember(s => s.Address, o => o.MapFrom(s => s.ShippingAddress));
            CreateMap<Shipment, AdminShipmentGetResponse>()
            .ForMember(s => s.Items, o => o.MapFrom(s => s.ShipmentOrderItems.Select(c => c.OrderItem)))
            .ForMember(s => s.Address, o => o.MapFrom(s => s.ShippingAddress));
            CreateMap<Shipment, ShipmentResponse>();
            CreateMap<Shipment, ShipmentListResponse>()
            .ForMember(s => s.Items, o => o.MapFrom(s => s.ShipmentOrderItems.Select(c => c.OrderItem)));
            CreateMap<Shipment, AdminShipmentShippedListResponse>()
            .ForMember(s => s.Items, o => o.MapFrom(s => s.ShipmentOrderItems.Select(c => c.OrderItem)));
            CreateMap<Shipment, AdminShipmentPendingListResponse>()
            .ForMember(s => s.Items, o => o.MapFrom(s => s.ShipmentOrderItems.Select(c => c.OrderItem)));

            CreateMap<ShippingAddress, ShippingAddressResponse>();

            CreateMap<ErrorLog, AdminLogErrorListResponse>();
            CreateMap<ErrorLog, AdminLogErrorGetResponse>();
            CreateMap<RequestResponseLog, AdminLogListResponse>();
            CreateMap<RequestResponseLog, AdminLogGetResponse>();
            CreateMap<SmsLog, AdminLogSmsListResponse>();
        }
    }
}