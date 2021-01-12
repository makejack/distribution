using System.Web;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Extensions;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台用户管理
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/user")]
    [Produces("application/json")]
    public class AdminUserController : ControllerBase
    {
        private readonly IRepositoryByInt<AdminUser> _adminUserRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="adminUserRepository"></param>
        /// <param name="mapper"></param>
        public AdminUserController(IRepositoryByInt<AdminUser> adminUserRepository,
                                   IMapper mapper)
        {
            _adminUserRepository = adminUserRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result> ChangePwd([FromBody] AdminUserChangePwdRequest request)
        {
            var id = HttpContext.GetUserId();
            var user = await _adminUserRepository.FirstOrDefaultAsync(id);

            var oldEntryPwd = request.OldPwd.ToMD5Base64();
            if (!user.Pwd.Equals(oldEntryPwd)) return Result.Fail(ResultCodes.PasswordError);

            var newEntryPwd = request.NewPwd.ToMD5Base64();
            user.Pwd = newEntryPwd;

            await _adminUserRepository.UpdateAsync(user);

            return Result.Ok();
        }
    }
}