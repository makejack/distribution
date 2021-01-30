using System.Security.AccessControl;
namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台重置密码请求
    /// </summary>
    public class AdminForgetResetPwdRequest
    {
        /// <summary>
        /// 手机号
        /// </summary>
        /// <value></value>
        public string Tel { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        /// <value></value>
        public string Code { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <value></value>
        public string Password { get; set; }
    }
}