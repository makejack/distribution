namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 微信小程序用户请求
    /// </summary>
    public class WeChatLiteAppLoginUserInfoRequest
    {
        /// <summary>
        /// 昵称
        /// </summary>
        /// <value></value>
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        /// <value></value>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        /// <value></value>
        public int Gender { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        /// <value></value>
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        /// <value></value>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        /// <value></value>
        public string City { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        /// <value></value>
        public string Language { get; set; }
    }
}