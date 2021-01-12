using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Filters;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台注册
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/register")]
    [Produces("application/json")]
    public class AdminRegisterController : ControllerBase
    {
        private readonly IRepositoryByInt<AdminUser> _adminUserRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="adminUserRepository"></param>
        /// <param name="mapper"></param>
        public AdminRegisterController(IRepositoryByInt<AdminUser> adminUserRepository,
                                       IMapper mapper)
        {
            _adminUserRepository = adminUserRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(AdminUserRegisterFilterAttribute))]
        public async Task<Result> Register([FromBody] AdminUserRegisterRequest request)
        {
            var user = await _adminUserRepository.Query().FirstOrDefaultAsync(e => e.Name == request.Name);
            if (user != null) return Result.Fail(ResultCodes.UserExists);

            var enctryPwd = request.Pwd.ToMD5Base64();

            var adminUser = new AdminUser(request.Name, request.Name, enctryPwd, true, EmployeeRole.Admin, string.Empty);

            await _adminUserRepository.InsertAsync(adminUser);

            return Result.Ok();
        }
    }
}