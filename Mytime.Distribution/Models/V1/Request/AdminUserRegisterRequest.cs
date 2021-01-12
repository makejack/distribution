namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台用户注册请求
    /// </summary>
    public class AdminUserRegisterRequest
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