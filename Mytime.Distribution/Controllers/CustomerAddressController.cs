using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Models.V1.Response;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 用户地址
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/customer/address")]
    [Produces("application/json")]
    public class CustomerAddressController : ControllerBase
    {
        private readonly IRepositoryByInt<CustomerAddress> _customerAddressRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customerAddressRepository"></param>
        /// <param name="mapper"></param>
        public CustomerAddressController(IRepositoryByInt<CustomerAddress> customerAddressRepository,
                                         IMapper mapper)
        {
            _customerAddressRepository = customerAddressRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List()
        {
            var userAddresses = await _customerAddressRepository.Query()
            .Include(e => e.Province)
            .Include(e => e.City)
            .Include(e => e.Area)
            .OrderByDescending(e => e.Id).ToListAsync();

            return Result.Ok(_mapper.Map<List<CustomerAddressResponse>>(userAddresses));
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var userAddress = await _customerAddressRepository.Query()
            .Include(e => e.Province)
            .Include(e => e.City)
            .Include(e => e.Area)
            .FirstOrDefaultAsync(e => e.Id == id);
            if (userAddress == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<CustomerAddressResponse>(userAddress));
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody] WeChatUserAddressCreateRequest request)
        {
            var userId = HttpContext.GetUserId();
            var userAddress = new CustomerAddress(userId, request.IsDefault, request.PostalCode, request.ProvinceCode, request.CityCode, request.AreaCode, request.DetailInfo, request.TelNumber, request.UserName);

            var defaultAddress = await _customerAddressRepository.Query().FirstOrDefaultAsync(e => e.CustomerId == userId && e.IsDefault);
            if (defaultAddress != null)
            {
                defaultAddress.IsDefault = false;

                await _customerAddressRepository.UpdateAsync(defaultAddress);
            }

            await _customerAddressRepository.InsertAsync(userAddress, false);

            return Result.Ok();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result> Edit([FromBody] WeChatUserAddressEditRequest request)
        {
            var userAddress = await _customerAddressRepository.FirstOrDefaultAsync(request.Id);
            if (userAddress == null) return Result.Fail(ResultCodes.IdInvalid);

            if (userAddress.IsDefault)
            {
                var defaultAddress = await _customerAddressRepository.Query().FirstOrDefaultAsync(e => e.CustomerId == userAddress.CustomerId && e.IsDefault);
                if (defaultAddress != null)
                {
                    defaultAddress.IsDefault = false;

                    await _customerAddressRepository.UpdateAsync(defaultAddress);
                }
            }

            userAddress.IsDefault = request.IsDefault;
            userAddress.PostalCode = request.PostalCode;
            userAddress.ProvinceCode = request.ProvinceCode;
            userAddress.CityCode = request.CityCode;
            userAddress.AreaCode = request.AreaCode;
            userAddress.DetailInfo = request.DetailInfo;
            userAddress.TelNumber = request.TelNumber;
            userAddress.UserName = request.UserName;

            await _customerAddressRepository.UpdateAsync(userAddress);

            return Result.Ok();
        }

        /// <summary>
        /// 修改默认地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<Result> ChangeDefult(int id)
        {
            var address = await _customerAddressRepository.FirstOrDefaultAsync(id);
            if (address == null) return Result.Fail(ResultCodes.IdInvalid);

            var userId = HttpContext.GetUserId();

            var defaultAddress = await _customerAddressRepository.Query().FirstOrDefaultAsync(e => e.CustomerId == userId && e.IsDefault);
            if (defaultAddress != null)
            {
                defaultAddress.IsDefault = false;

                await _customerAddressRepository.UpdateProperyAsync(defaultAddress, nameof(defaultAddress.IsDefault));
            }

            address.IsDefault = true;
            await _customerAddressRepository.UpdateProperyAsync(address, nameof(address.IsDefault));

            return Result.Ok();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var userAddress = await _customerAddressRepository.FirstOrDefaultAsync(id);
            if (userAddress == null) return Result.Fail(ResultCodes.IdInvalid);

            await _customerAddressRepository.RemoveAsync(userAddress);

            return Result.Ok();
        }

    }
}