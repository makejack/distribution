using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台地址
    /// </summary>
    [Authorize(Roles = "Admin")]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/address")]
    [Produces("application/json")]
    public class AdminAddressController : ControllerBase
    {
        private readonly IRepositoryByInt<AdminAddress> _addressRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="addressRepository"></param>
        /// <param name="mapper"></param>
        public AdminAddressController(IRepositoryByInt<AdminAddress> addressRepository,
                                      IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取所有地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<Result> All()
        {
            var addresses = await _addressRepository.Query()
            .Include(e => e.Province)
            .Include(e => e.City)
            .Include(e => e.Area)
            .ToListAsync();

            return Result.Ok(_mapper.Map<List<CustomerAddressResponse>>(addresses));
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] PaginationRequest request)
        {
            var queryable = _addressRepository.Query();

            var totalRows = await queryable.CountAsync();
            var addresses = await queryable.Include(e => e.Province)
            .Include(e => e.City)
            .Include(e => e.Area)
            .OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<CustomerAddressResponse>>(addresses)));
        }

        /// <summary>
        /// 地址详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var address = await _addressRepository.Query()
            .Include(e => e.Province)
            .Include(e => e.City)
            .Include(e => e.Area)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (address == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<CustomerAddressResponse>(address));
        }

        /// <summary>
        /// 创建地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody] CustomerAddressCreateRequest request)
        {
            var address = new AdminAddress
            {
                AreaCode = request.AreaCode,
                CityCode = request.CityCode,
                Createat = DateTime.Now,
                DetailInfo = request.DetailInfo,
                IsDefault = request.IsDefault,
                PostalCode = request.PostalCode,
                ProvinceCode = request.ProvinceCode,
                TelNumber = request.TelNumber,
                UserName = request.UserName
            };

            if (request.IsDefault)
            {
                var defaultAddress = await _addressRepository.Query().FirstOrDefaultAsync(e => e.IsDefault);
                if (defaultAddress != null)
                {
                    defaultAddress.IsDefault = false;

                    await _addressRepository.UpdateProperyAsync(defaultAddress, nameof(defaultAddress.IsDefault));
                }
            }

            await _addressRepository.InsertAsync(address);

            return Result.Ok();
        }

        /// <summary>
        /// 编辑地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result> Edit([FromBody] CustomerAddressEditRequest request)
        {
            var address = await _addressRepository.FirstOrDefaultAsync(request.Id);
            if (address == null) return Result.Fail(ResultCodes.IdInvalid);

            if (request.IsDefault)
            {
                var defaultAddress = await _addressRepository.Query().FirstOrDefaultAsync(e => e.IsDefault);
                if (defaultAddress != null)
                {
                    defaultAddress.IsDefault = false;

                    await _addressRepository.UpdateProperyAsync(defaultAddress, nameof(defaultAddress.IsDefault));
                }
            }

            address.DetailInfo = request.DetailInfo;
            address.AreaCode = request.AreaCode;
            address.CityCode = request.CityCode;
            address.IsDefault = request.IsDefault;
            address.PostalCode = request.PostalCode;
            address.ProvinceCode = request.ProvinceCode;
            address.TelNumber = request.TelNumber;
            address.UserName = request.UserName;

            await _addressRepository.UpdateAsync(address);

            return Result.Ok();
        }

        /// <summary>
        /// 修改默认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<Result> ChangeDefault(int id)
        {
            var address = await _addressRepository.FirstOrDefaultAsync(id);
            if (address == null) return Result.Fail(ResultCodes.IdInvalid);

            if (!address.IsDefault)
            {
                var defaultAddress = await _addressRepository.Query().FirstOrDefaultAsync(e => e.IsDefault);
                if (defaultAddress != null)
                {
                    defaultAddress.IsDefault = false;

                    await _addressRepository.UpdateProperyAsync(defaultAddress, nameof(defaultAddress.IsDefault));
                }
            }

            address.IsDefault = !address.IsDefault;

            await _addressRepository.UpdateProperyAsync(address, nameof(address.IsDefault));

            return Result.Ok();
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var address = await _addressRepository.FirstOrDefaultAsync(id);
            if (address == null) return Result.Fail(ResultCodes.IdInvalid);

            await _addressRepository.RemoveAsync(address);

            return Result.Ok();
        }
    }
}