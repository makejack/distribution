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
        private readonly IPackageService _packageService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shipmentRepository"></param>
        /// <param name="packageService"></param>
        /// <param name="mapper"></param>
        public ShippingController(IRepositoryByInt<Shipment> shipmentRepository,
                                  IPackageService packageService,
                                  IMapper mapper)
        {
            _shipmentRepository = shipmentRepository;
            _packageService = packageService;
            _mapper = mapper;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Query(int id)
        {
            var shipment = await _shipmentRepository.Query().Include(e => e.ShippingAddress).FirstOrDefaultAsync(e => e.Id == id);
            if (shipment == null) return Result.Fail(ResultCodes.IdInvalid);

            var body = string.Empty;
            try
            {
                var tel = shipment.ShippingAddress.TelNumber;
                body = await _packageService.QueryAsync(shipment.CourierCompanyCode, shipment.TrackingNumber, tel);
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
                Id = shipment.Id,
                CourierCompany = shipment.CourierCompany,
                CourierCompanyCode = shipment.CourierCompanyCode,
                TrackingNumber = shipment.TrackingNumber,
                Body = body
            };

            return Result.Ok(response);
        }
    }
}