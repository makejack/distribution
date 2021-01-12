namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台用户修改密码请求
    /// </summary>
    public class AdminUserChangePwdRequest
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        /// <value></value>
        public string OldPwd { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        /// <value></value>
        public string NewPwd { get; set; }
    }
}