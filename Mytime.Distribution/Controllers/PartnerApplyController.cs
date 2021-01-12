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
using Mytime.Distribution.Models.V1.Response;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 合伙人申请
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/partner/apply")]
    public class PartnerApplyController : ControllerBase
    {
        private IRepositoryByInt<PartnerApply> _partnerApplyRepository;
        private IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="partnerApplyRepository"></param>
        /// <param name="mapper"></param>
        public PartnerApplyController(IRepositoryByInt<PartnerApply> partnerApplyRepository,
                                      IMapper mapper)
        {
            _partnerApplyRepository = partnerApplyRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet("condition/{role}")]
        [ProducesResponseType(typeof(AdminPartnerApplyGetResponse), 200)]
        public async Task<Result> Condition(PartnerRole role)
        {
            var partnerApply = await _partnerApplyRepository.Query()
            .Include(e => e.PartnerApplyGoods).ThenInclude(e => e.Goods).ThenInclude(e => e.ThumbnailImage)
            .FirstOrDefaultAsync(e => e.PartnerRole == role);

            return Result.Ok(_mapper.Map<AdminPartnerApplyGetResponse>(partnerApply));
        }
    }
}