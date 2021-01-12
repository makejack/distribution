using System;
using System.Linq;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;
using System.Threading.Tasks;
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Services;
using MediatR;
using Mytime.Distribution.Events;
using Mytime.Distribution.Services.SmsContent;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 发货商品
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/shipment")]
    [Produces("application/json")]
    public class ShipmentController : ControllerBase
    {
        private readonly IRepositoryByInt<AdminUser> _adminUserRepository;
        private readonly IRepositoryByInt<Shipment> _shipmentRepository;
        private readonly IRepositoryByInt<OrderItem> _orderItemRepository;
        private readonly IRepositoryByInt<Goods> _goodsRepository;
        private readonly IRepositoryByInt<CustomerAddress> _customerAddressRepository;
        private readonly IShipmentService _shipmentService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="adminUserRepository"></param>
        /// <param name="shipmentRepository"></param>
        /// <param name="orderItemRepository"></param>
        /// <param name="goodsRepository"></param>
        /// <param name="customerAddressRepository"></param>
        /// <param name="shipmentService"></param>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        public ShipmentController(IRepositoryByInt<AdminUser> adminUserRepository,
                                  IRepositoryByInt<Shipment> shipmentRepository,
                                  IRepositoryByInt<OrderItem> orderItemRepository,
                                  IRepositoryByInt<Goods> goodsRepository,
                                  IRepositoryByInt<CustomerAddress> customerAddressRepository,
                                  IShipmentService shipmentService,
                                  IMediator mediator,
                                  IMapper mapper)
        {
            _adminUserRepository = adminUserRepository;
            _shipmentRepository = shipmentRepository;
            _orderItemRepository = orderItemRepository;
            _goodsRepository = goodsRepository;
            _customerAddressRepository = customerAddressRepository;
            _shipmentService = shipmentService;
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// 待发货处理的订单
        /// </summary>
        /// <returns></returns>
        [HttpGet("pendinglist")]
        public async Task<Result> PendingList([FromQuery] PaginationRequest request)
        {
            var page = request.Page;
            var limit = request.Limit;
            if (page <= 0) page = 1;
            if (limit <= 0) limit = 10;

            var userId = HttpContext.GetUserId();

            var queryable = _orderItemRepository.Query().Where(e => e.Order.CustomerId == userId && e.Order.OrderStatus == OrderStatus.PaymentReceived && e.ShippingStatus == ShippingStatus.Default);
            var totalRows = await queryable.CountAsync();
            var orderItems = await queryable
            .Include(e => e.Goods).ThenInclude(e => e.Childrens).ThenInclude(e => e.OptionCombinations).ThenInclude(e => e.Option)
            .Include(e => e.Goods).ThenInclude(e => e.OptionValues).ThenInclude(e => e.Option)
            .OrderByDescending(e => e.Id)
            .Skip((page - 1) * limit).Take(limit)
            .ToListAsync();

            var orderItemsResponse = _mapper.Map<List<ShipmentPendingListResponse>>(orderItems);

            for (int i = 0; i < orderItems.Count; i++)
            {
                var item = orderItems[i];
                var optionIds = item.Goods.OptionValues.OrderBy(e => e.DisplayOrder)
                .ThenBy(x => x.Option.Name)
                .GroupBy(c => c.OptionId)
                .Select(s => s.Key)
                .OrderBy(c => c);
                var options = new List<AdminGoodsGetOptionResponse>();
                foreach (var optionId in optionIds)
                {
                    var first = item.Goods.OptionValues.First(c => c.OptionId == optionId);
                    var result = new AdminGoodsGetOptionResponse
                    {
                        Id = first.OptionId,
                        Name = first.Option.Name,
                        Createat = first.Createat,
                        Values = item.Goods.OptionValues.Where(e => e.OptionId == optionId).Select(s => new AdminGoodsGetOptionValueResponse
                        {
                            Id = s.Id,
                            DisplayOrder = s.DisplayOrder,
                            Value = s.Value,
                            Createat = s.Createat
                        }).OrderBy(c => c.DisplayOrder).ToList()
                    };
                    options.Add(result);
                }
                orderItemsResponse[i].Options = options;

                orderItemsResponse[i].Variations = item.Goods.Childrens.Select(s => new AdminGoodsGetVariationResponse
                {
                    Id = s.Id,
                    NormalizedName = s.NormalizedName,
                    StockQuantity = s.StockQuantity,
                    Price = s.Price,
                    OptionCombinations = s.OptionCombinations.Select(e => new AdminGoodsGetOptionCombinationResponse
                    {
                        OptionId = e.OptionId,
                        OptionName = e.Option?.Name,
                        Value = e.Value,
                        DisplayOrder = e.DisplayOrder
                    }).OrderBy(e => e.DisplayOrder).ToList()
                }).ToList();
            }

            var pagination = new PaginationResponse(page, totalRows, orderItemsResponse);

            return Result.Ok(pagination);
        }

        /// <summary>
        /// 待收货或已完成列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] ShipmentListRequest request)
        {
            var page = request.Page;
            var limit = request.Limit;
            if (page <= 0) page = 1;
            if (limit <= 0) limit = 10;

            var userId = HttpContext.GetUserId();

            var queryable = _shipmentRepository.Query().Where(e => e.CustomerId == userId && e.ShippingStatus == request.Status);
            var totalRows = await queryable.CountAsync();
            var shipments = await queryable.Include(e => e.ShipmentOrderItems).ThenInclude(e => e.OrderItem)
            .OrderByDescending(e => e.Id)
            .Skip((page - 1) * limit).Take(limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(page, totalRows, _mapper.Map<List<ShipmentListResponse>>(shipments)));
        }

        /// <summary>
        /// 获取装货信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var shipment = await _shipmentRepository.Query()
            .Include(e => e.ShipmentOrderItems).ThenInclude(e => e.OrderItem)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (shipment == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<ShipmentListResponse>(shipment));
        }

        /// <summary>
        /// 申请发货
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("apply")]
        public async Task<Result> Apply([FromBody] ShipmentApplyRequest request)
        {
            var userId = HttpContext.GetUserId();

            ShippingAddress shippingAddress = null;
            if (request.ShippingAddressId > 0)
            {
                var address = await _customerAddressRepository.Query()
                .Include(e => e.Province)
                .Include(e => e.City)
                .Include(e => e.Area)
                .FirstOrDefaultAsync(e => e.Id == request.ShippingAddressId);
                if (address == null) return Result.Fail(ResultCodes.RequestParamError, "送货地址不存在");

                shippingAddress = new ShippingAddress
                {
                    PostalCode = address.PostalCode.ToString(),
                    ProvinceName = address.Province.Name,
                    CityName = address.City.Name,
                    AreaName = address.Area.Name,
                    DetailInfo = address.DetailInfo,
                    TelNumber = address.TelNumber,
                    UserName = address.UserName,
                    Createat = DateTime.Now
                };
            }
            else if (request.ShippingAddress != null)
            {
                shippingAddress = new ShippingAddress
                {
                    PostalCode = request.ShippingAddress.PostalCode,
                    ProvinceName = request.ShippingAddress.ProvinceName,
                    CityName = request.ShippingAddress.CityName,
                    AreaName = request.ShippingAddress.CountyName,
                    DetailInfo = request.ShippingAddress.DetailInfo,
                    TelNumber = request.ShippingAddress.TelNumber,
                    UserName = request.ShippingAddress.UserName,
                    Createat = DateTime.Now
                };
            }

            var orderItemIds = request.Items.Select(e => e.Id).ToList();
            var orderItems = await _orderItemRepository.Query().Where(e => orderItemIds.Contains(e.Id)).ToListAsync();
            // var goodsIds = request.Items.Select(e => e.GoodsId);
            // var goodses = await _goodsRepository.Query().Where(e => goodsIds.Contains(e.Id)).ToListAsync();
            var shipmentOrderItems = new List<ShipmentOrderItem>();
            foreach (var item in request.Items)
            {
                var first = orderItems.FirstOrDefault(e => e.Id == item.Id);
                if (first == null) return Result.Fail(ResultCodes.RequestParamError, $"订单不存在 {item.Id}");

                // var goods = goodses.FirstOrDefault(e => e.Id == item.GoodsId);
                // if (goods == null) return Result.Fail(ResultCodes.RequestParamError, "商品不存在");
                // if (goods.ParentId == null) return Result.Fail(ResultCodes.RequestParamError, "选择的商品不是有效的商品");
                // if (goods.StockQuantity <= 0) return Result.Fail(ResultCodes.RequestParamError, $"商品{goods.Name} {goods.NormalizedName} 库存不足");
                // //减库存
                // goods.StockQuantity -= first.Quantity;

                first.GoodsItemId = item.GoodsId;
                first.NormalizedName = item.NormalizedName;
                // first.ShippingStatus = ShippingStatus.PendingShipment;
                // first.ShippingTime = DateTime.Now;

                shipmentOrderItems.Add(new ShipmentOrderItem(first.Id));
            }

            var shipment = new Shipment
            {
                CustomerId = userId,
                Quantity = orderItems.Count,
                ShippingStatus = ShippingStatus.PendingShipment,
                ShippingTime = DateTime.Now,
                Createat = DateTime.Now,
                Remarks = request.Remarks,
                ShippingAddress = shippingAddress,
                ShipmentOrderItems = shipmentOrderItems
            };

            _shipmentRepository.Insert(shipment, false);

            using (var transaction = _shipmentRepository.BeginTransaction())
            {
                await _shipmentRepository.SaveAsync();

                // await _goodsRepository.UpdateRangeAsync(goodses);

                transaction.Commit();
            }

            await _orderItemRepository.UpdateRangeAsync(orderItems);

            return Result.Ok(_mapper.Map<ShipmentResponse>(shipment));
        }

        /// <summary>
        /// 送货地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("address")]
        public async Task<Result> Address(ShipmentAddressRequest request)
        {
            var shipment = await _shipmentRepository.Query()
            .Include(e => e.ShipmentOrderItems).ThenInclude(e => e.OrderItem)
            .FirstOrDefaultAsync(e => e.Id == request.Id);
            if (shipment == null) return Result.Fail(ResultCodes.IdInvalid);

            ShippingAddress shippingAddress = null;
            if (request.ShippingAddressId > 0)
            {
                var address = await _customerAddressRepository.Query()
                .Include(e => e.Province)
                .Include(e => e.City)
                .Include(e => e.Area)
                .FirstOrDefaultAsync(e => e.Id == request.ShippingAddressId);
                if (address == null) return Result.Fail(ResultCodes.RequestParamError, "送货地址不存在");

                shippingAddress = new ShippingAddress
                {
                    ShipmentId = request.Id,
                    PostalCode = address.PostalCode.ToString(),
                    ProvinceName = address.Province.Name,
                    CityName = address.City.Name,
                    AreaName = address.Area.Name,
                    DetailInfo = address.DetailInfo,
                    TelNumber = address.TelNumber,
                    UserName = address.UserName,
                    Createat = DateTime.Now
                };
            }
            else if (request.ShippingAddress != null)
            {
                shippingAddress = new ShippingAddress
                {
                    ShipmentId = request.Id,
                    PostalCode = request.ShippingAddress.PostalCode,
                    ProvinceName = request.ShippingAddress.ProvinceName,
                    CityName = request.ShippingAddress.CityName,
                    AreaName = request.ShippingAddress.CountyName,
                    DetailInfo = request.ShippingAddress.DetailInfo,
                    TelNumber = request.ShippingAddress.TelNumber,
                    UserName = request.ShippingAddress.UserName,
                    Createat = DateTime.Now
                };
            }
            else
            {
                return Result.Fail(ResultCodes.RequestParamError, "请选择送货地址");
            }

            var orderItems = shipment.ShipmentOrderItems.Select(e => e.OrderItem);
            var goodsIds = orderItems.Select(e => e.GoodsItemId);
            var goodses = await _goodsRepository.Query().Where(e => goodsIds.Contains(e.Id)).ToListAsync();
            foreach (var item in orderItems)
            {
                var goods = goodses.FirstOrDefault(e => e.Id == item.GoodsItemId);
                if (goods == null) return Result.Fail(ResultCodes.RequestParamError, "商品不存在");
                if (goods.ParentId == null) return Result.Fail(ResultCodes.RequestParamError, "选择的商品不是有效的商品");
                if (goods.StockQuantity <= 0) return Result.Fail(ResultCodes.RequestParamError, $"商品{goods.Name} {goods.NormalizedName} 库存不足");
                //减库存
                goods.StockQuantity -= item.Quantity;

                item.ShippingStatus = ShippingStatus.PendingShipment;
                item.ShippingTime = DateTime.Now;
            }

            shipment.Remarks = request.Remarks;
            shipment.ShippingAddress = shippingAddress;

            _shipmentRepository.Update(shipment, false);

            using (var transaction = _shipmentRepository.BeginTransaction())
            {
                await _shipmentRepository.SaveAsync();

                await _goodsRepository.UpdateRangeAsync(goodses);

                transaction.Commit();
            }

            var employeeTels = await _adminUserRepository.Query().Where(e => e.Role == EmployeeRole.AfterSale).Select(e => e.Tel).ToListAsync();
            if (employeeTels.Count > 0)
            {
                var shippingApplyNotify = new ShippingApplyNotify
                {
                    Name = shippingAddress.UserName,
                    Tel = shippingAddress.TelNumber,
                };
                var notifyEvent = new SmsNotifyEvent
                {
                    Tels = employeeTels,
                    Message = shippingApplyNotify
                };
                await _mediator.Publish(notifyEvent);
            }

            return Result.Ok();
        }

        /// <summary>
        /// 删除装货申请 (无效接口)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var shipment = await _shipmentRepository.Query()
            .Include(e => e.ShipmentOrderItems).ThenInclude(e => e.OrderItem)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (shipment == null) return Result.Fail(ResultCodes.IdInvalid);

            if (shipment.ShippingStatus != ShippingStatus.PendingShipment) return Result.Fail(ResultCodes.RequestParamError, "当前状态不允许删除");
            var orderItems = shipment.ShipmentOrderItems.Select(e => e.OrderItem).ToList();
            var goodsIds = orderItems.Select(e => e.GoodsId);
            var goodses = await _goodsRepository.Query().Where(e => goodsIds.Contains(e.Id)).ToListAsync();
            foreach (var item in orderItems)
            {
                var goods = goodses.FirstOrDefault(e => e.Id == item.GoodsId);
                if (goods != null)
                {
                    goods.StockQuantity += item.Quantity;
                }

                item.ShippingStatus = ShippingStatus.Default;
                item.ShippingTime = null;
            }

            _shipmentRepository.Remove(shipment, false);

            using (var transaction = _shipmentRepository.BeginTransaction())
            {
                await _shipmentRepository.SaveAsync();

                // await _orderItemRepository.UpdateRangeAsync(orderItems);

                await _goodsRepository.UpdateRangeAsync(goodses);

                transaction.Commit();
            }

            return Result.Ok();
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("confirm/{id}")]
        public async Task<Result> Confirm(int id)
        {
            return await _shipmentService.Confirm(id);
        }
    }
}