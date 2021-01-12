using System.Linq;
using System;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Services;
using Mytime.Distribution.Models.V1.Response;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台授权
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/auth/[action]")]
    [Produces("application/json")]
    public class AdminAuthController : ControllerBase
    {
        private readonly IRepositoryByInt<AdminUser> _adminUserRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造波函数
        /// </summary>
        /// <param name="adminUserRepository"></param>
        /// <param name="tokenService"></param>
        /// <param name="mapper"></param>
        public AdminAuthController(
            IRepositoryByInt<AdminUser> adminUserRepository,
            ITokenService tokenService,
            IMapper mapper)
        {
            _adminUserRepository = adminUserRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Login([FromBody] AdminUserLoginRequest request)
        {
            var user = await _adminUserRepository.Query().FirstOrDefaultAsync(e => e.Name == request.Name);
            if (user == null) return Result.Fail(ResultCodes.UserNotExists);
            if (!user.Pwd.Equals(request.Pwd.ToMD5Base64())) return Result.Fail(ResultCodes.PasswordError);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("id", user.Id.ToString()));

            var token = _tokenService.JwtToken(claims);
            var userRes = _mapper.Map<AdminUserResponse>(user);

            return Result.Ok(new AdminLoginResponse(token, userRes));
        }
    }
}