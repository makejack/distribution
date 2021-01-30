using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Models;
using Mytime.Distribution.Services;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 小程序
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/liteapp")]
    [Produces("application/json")]
    public class LiteAppController : ControllerBase
    {
        private readonly IRepositoryByInt<LiteAppSetting> _repository;
        private readonly ICustomerManager _customerManger;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="customerManger"></param>
        /// <param name="mapper"></param>
        public LiteAppController(IRepositoryByInt<LiteAppSetting> repository,
                                 ICustomerManager customerManger,
                                 IMapper mapper)
        {
            _repository = repository;
            _customerManger = customerManger;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取会员权益内容
        /// </summary>
        /// <returns></returns>
        [HttpGet("membershiprights")]
        public async Task<Result> GetMembershipRights()
        {
            var user = await _customerManger.GetUserAsync();
            var setting = await _repository.Query().FirstOrDefaultAsync();

            var content = string.Empty;
            if (user.Role == PartnerRole.CityPartner)
            {
                content = setting.CityMembershipRights;
            }
            else
            {
                content = setting.BranchMembershipRights;
            }

            return Result.Ok(content);
        }
    }
}