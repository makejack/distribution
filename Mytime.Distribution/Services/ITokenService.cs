using System.Collections.Generic;
using System.Security.Claims;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Models.V1.Response;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 授权服务
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 生成Jwt授权
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        JwtTokenResponse JwtToken(List<Claim> claims);
    }
}