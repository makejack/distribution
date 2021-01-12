namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 顾客更新用户信息
    /// </summary>
    public class CustomerUpdateUserInfoRequest
    {
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