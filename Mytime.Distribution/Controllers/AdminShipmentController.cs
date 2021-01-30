using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Events;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Services;
using Mytime.Distribution.Services.SmsContent;
using Mytime.Distribution.Utils.Constants;
using Mytime.Distribution.Utils.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台装货
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/shipment")]
    [Produces("application/json")]
    public class AdminShipmentController : ControllerBase
    {
        private readonly IRepositoryByInt<Shipment> _shipmentRepository;
        private readonly ISmsService _smsService;
        private readonly IRabbitMQClient _client;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shipmentRepository"></param>
        /// <param name="smsService"></param>
        /// <param name="client"></param>
        /// <param name="mapper"></param>
        public AdminShipmentController(IRepositoryByInt<Shipment> shipmentRepository,
                                       ISmsService smsService,
                                       IRabbitMQClient client,
                                       IMapper mapper)
        {
            _shipmentRepository = shipmentRepository;
            _smsService = smsService;
            _client = client;
            _mapper = mapper;
        }

        /// <summary>
        /// 装货列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] AdminShipmentListRequest request)
        {
            var queryable = _shipmentRepository.Query();

            if (request.Status.HasValue)
            {
                queryable = queryable.Where(e => e.ShippingStatus == request.Status.Value);
            }

            var totalRows = await queryable.CountAsync();
            var shipments = await queryable.Include(e => e.ShippingAddress)
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminShipmentListResponse>>(shipments)));
        }

        /// <summary>
        /// 获取装货详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var shipment = await _shipmentRepository.Query()
            .Include(e => e.ShipmentOrderItems).ThenInclude(e => e.OrderItem)
            .Include(e => e.ShippingAddress)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (shipment == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminShipmentGetResponse>(shipment));
        }

        /// <summary>
        /// 设置快递单号
        /// </summary>
        /// <returns></returns>
        [HttpPut("settrackingnumber")]
        public async Task<Result> SetTrackingNumber(AdminShipmentSetTrackingNumberRequest request)
        {
            var shipment = await _shipmentRepository.Query()
            .Include(e => e.ShipmentOrderItems).ThenInclude(e => e.OrderItem)
            .Include(e => e.ShippingAddress)
            .FirstOrDefaultAsync(e => e.Id == request.Id);
            if (shipment == null) return Result.Fail(ResultCodes.IdInvalid);

            var status = new[] { ShippingStatus.PendingShipment, ShippingStatus.Shipped };
            if (!status.Contains(shipment.ShippingStatus)) return Result.Fail(ResultCodes.RequestParamError, "当前订单不允许修改状态");

            if (string.IsNullOrEmpty(request.CourierCompany))
            {
                shipment.CourierCompany = CourierCompanyHelper.GetCompanyName(request.CourierCompanyCode);
            }
            else
            {
                shipment.CourierCompany = request.CourierCompany;
            }
            shipment.TrackingNumber = request.TrackingNumber;
            shipment.CourierCompanyCode = request.CourierCompanyCode;
            if (shipment.ShippingStatus != ShippingStatus.Shipped)
            {
                shipment.ShippingStatus = ShippingStatus.Shipped;
                shipment.ShippingTime = DateTime.Now;

                var orderItems = shipment.ShipmentOrderItems.Select(e => e.OrderItem);
                foreach (var item in orderItems)
                {
                    item.Status = OrderItemStatus.Shipped;
                }

                var message = new AutoReceivedShippingEvent
                {
                    ShipmentId = shipment.Id
                };
                var headers = new Dictionary<string, object>(){
                    {"x-delay", 10 * 24 * 60 * 60 * 1000} // 延迟10天自动收货
                };
                var properties = _client.CreateProperties();
                properties.Headers = headers;

                _client.PushMessage(MQConstants.Exchange, MQConstants.AutoReceivedShippingKey, message, properties);

                var address = shipment.ShippingAddress;
                var notify = new ShippingNotify
                {
                    TrackingNumber = request.TrackingNumber,
                    UserName = address.UserName,
                    CourierCompany = shipment.CourierCompany
                };
                await _smsService.SendAsync(address.TelNumber, notify);
            }

            _shipmentRepository.Update(shipment, false);

            using (var transaction = _shipmentRepository.BeginTransaction())
            {
                await _shipmentRepository.SaveAsync();

                transaction.Commit();
            }

            return Result.Ok();
        }

        /// <summary>
        /// 删除装货数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var shippint = await _shipmentRepository.FirstOrDefaultAsync(id);
            if (shippint == null) Result.Fail(ResultCodes.IdInvalid);

            await _shipmentRepository.RemoveAsync(shippint);

            return Result.Ok();
        }
    }
}