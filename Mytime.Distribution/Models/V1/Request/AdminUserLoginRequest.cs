namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台用户登录请求
    /// </summary>
    public class AdminUserLoginRequest
    {
        /// <summary>
        /// 账号
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <value></value>
        public string Pwd { get; set; }
    }
}