namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台登录响应
    /// </summary>
    public class AdminLoginResponse
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <param name="user"></param>
        public AdminLoginResponse(JwtTokenResponse jwtToken, AdminUserResponse user)
        {
            JwtToken = jwtToken;
            User = user;
        }

        /// <summary>
        /// token
        /// </summary>
        /// <value></value>
        public JwtTokenResponse JwtToken { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <value></value>
        public AdminUserResponse User { get; set; }
    }
}