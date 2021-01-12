namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 微信小程序Login
    /// </summary>
    public class WeChatLiteAppLoginRequest
    {
        /// <summary>
        /// 小程序 Login 的code
        /// </summary>
        /// <value></value>
        public string Code { get; set; }
        /// <summary>
        /// 上级Id 没有为0
        /// </summary>
        /// <value></value>
        public int ParentId { get; set; }
        /// <summary>
        /// 用户有信息
        /// </summary>
        /// <value></value>
        public WeChatLiteAppLoginUserInfoRequest UserInfo { get; set; }
    }
}