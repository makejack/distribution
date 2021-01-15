using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Services;
using Newtonsoft.Json;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 运输
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/shipping")]
    [Produces("application/json")]
    public class ShippingController : ControllerBase
    {
        private readonly IRepositoryByInt<Shipment> _shipmentRepository;
        private readonly IRepositoryByInt<OrderItem> _orderItemRepository;
        private readonly IPackageService _packageService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shipmentRepository"></param>
        /// <param name="orderItemRepository"></param>
        /// <param name="packageService"></param>
        /// <param name="mapper"></param>
        public ShippingController(IRepositoryByInt<Shipment> shipmentRepository,
        IRepositoryByInt<OrderItem> orderItemRepository,
                                  IPackageService packageService,
                                  IMapper mapper)
        {
            _shipmentRepository = shipmentRepository;
            _orderItemRepository = orderItemRepository;
            _packageService = packageService;
            _mapper = mapper;
        }

        /// <summary>
        /// 发货快递查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShippingQueryResponse), 200)]
        public async Task<Result> Query(int id)
        {
            var shipment = await _shipmentRepository.Query().Include(e => e.ShippingAddress).FirstOrDefaultAsync(e => e.Id == id);
            if (shipment == null) return Result.Fail(ResultCodes.IdInvalid);

            var tel = shipment.ShippingAddress.TelNumber;
            var courierCompany = shipment.CourierCompany;
            var courierCompanyCode = shipment.CourierCompanyCode;
            // if (string.IsNullOrEmpty(courierCompanyCode)) return Result.Fail(ResultCodes.RequestParamError, "快递公司编码参数无效");
            var trackingNumber = shipment.TrackingNumber;
            // if (string.IsNullOrEmpty(trackingNumber)) return Result.Fail(ResultCodes.RequestParamError, "快递单号无效");
            var response = await ShippingQuery(id, tel, courierCompany, courierCompanyCode, trackingNumber);

            return Result.Ok(response);
        }

        /// <summary>
        /// 退货快递查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("return/{id}")]
        public async Task<Result> Return(int id)
        {
            var item = await _orderItemRepository.Query()
            .Include(e => e.ReturnAddress)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (item == null) return Result.Ok(ResultCodes.IdInvalid);

            var tel = item.ReturnAddress.TelNumber;
            var courierCompany = item.CourierCompany;
            var courierCompanyCode = item.CourierCompanyCode;
            // if (string.IsNullOrEmpty(courierCompanyCode)) return Result.Fail(ResultCodes.RequestParamError, "快递公司编码参数无效");
            var trackingNumber = item.TrackingNumber;
            // if (string.IsNullOrEmpty(trackingNumber)) return Result.Fail(ResultCodes.RequestParamError, "快递单号无效");

            var response = await ShippingQuery(id, tel, courierCompany, courierCompanyCode, trackingNumber);

            return Result.Ok(response);
        }

        private async Task<ShippingQueryResponse> ShippingQuery(int id,
                                                                string tel,
                                                                string courierCompany,
                                                                string courierCompanyCode,
                                                                string trackingNumber)
        {
            var body = string.Empty;
            try
            {
                body = await _packageService.QueryAsync(courierCompanyCode, trackingNumber, tel);
            }
            catch (Exception ex)
            {
                body = JsonConvert.SerializeObject(new
                {
                    Result = false,
                    ReturnCode = "400",
                    Message = ex.Message
                });
            }
            var response = new ShippingQueryResponse
            {
                Id = id,
                CourierCompany = courierCompany,
                CourierCompanyCode = courierCompanyCode,
                TrackingNumber = trackingNumber,
                Body = body
            };
            return response;
        }
    }
}