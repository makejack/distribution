namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// JwtToken响应
    /// </summary>
    public class JwtTokenResponse
    {
        /// <summary>
        /// 授权令牌
        /// </summary>
        /// <value></value>
        public string AuthToken { get; set; }
        /// <summary>
        /// 过期分钟
        /// </summary>
        /// <value></value>
        public int ExpiressIn { get; set; }
        /// <summary>
        /// Token类型
        /// </summary>
        /// <value></value>
        public string TokenType { get; set; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <value></value>
        public string RefereshToken { get; set; }
    }
}