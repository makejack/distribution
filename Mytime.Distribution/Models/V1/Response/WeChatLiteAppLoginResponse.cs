namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 微信小程序登录响应
    /// </summary>
    public class WeChatLiteAppLoginResponse
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WeChatLiteAppLoginResponse()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <param name="user"></param>
        public WeChatLiteAppLoginResponse(JwtTokenResponse jwtToken, CustomerResponse user)
        {
            JwtToken = jwtToken;
            User = user;
        }
        /// <summary>
        /// Token
        /// </summary>
        /// <value></value>
        public JwtTokenResponse JwtToken { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <value></value>
        public CustomerResponse User { get; set; }
    }
}