using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Mytime.Distribution.Configs;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Utils.Helpers;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 授权服务
    /// </summary>
    public class TokenService : ITokenService
    {

        private AuthenticationConfig _config;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        public TokenService(AuthenticationConfig config)
        {
            _config = config;
        }


        #region 刷新令牌
        /*

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="token"></param>
        /// <param name="refereshToken"></param>
        public JwtTokenResponse RefershJwtToken(string token, string refereshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new JwtTokenResponse { Message = "Invalid Token" };
            }

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new JwtTokenResponse { Message = "This token hasn't expired yet" };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return new JwtTokenResponse { Message = "This refresh token does not exist" };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new JwtTokenResponse { Message = "This refresh token has expired" };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new JwtTokenResponse { Message = "This refresh token has been invalidated" };
            }

            if (storedRefreshToken.Used)
            {
                return new JwtTokenResponse { Message = "This refresh token has been used" };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new JwtTokenResponse { Message = "This refresh token does not match this JWT" };
            }

            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return await GenerateAuthenticationResultForUserAsync(user);
        }


        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        */
        #endregion
        
        /// <summary>
        /// 生成Jwt授权
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public JwtTokenResponse JwtToken(List<Claim> claims)
        {
            //生成刷新令牌
            var refreshToken = GenerateHelper.GenerateRefreshToken();
            //添加刷新令牌
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, refreshToken));

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.Key));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _config.Issuer,
                Audience = _config.Audience,
                Expires = DateTime.Now.AddSeconds(_config.Expiress),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var jwtSecurityToken = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityToken.CreateToken(tokenDescriptor);

            //保存刷新令牌
            /*

            Refresh_Token refresh_Token = new Refresh_Token
            {
                JwtId = securityToken.Id,
                Token = refreshToken,
                Customer_Id = user.Id,
                ExpiryDate = DateTime.Now.AddMinutes(5)
            };
            await _refreshTokenRepository.AddAsync(refresh_Token);

            */

            var response = new JwtTokenResponse();
            response.AuthToken = jwtSecurityToken.WriteToken(securityToken);
            response.ExpiressIn = _config.Expiress;
            response.TokenType = "Bearer";
            response.RefereshToken = refreshToken;

            return response;
        }

    }
}