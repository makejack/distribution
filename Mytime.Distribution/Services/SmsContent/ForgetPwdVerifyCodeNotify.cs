namespace Mytime.Distribution.Services.SmsContent
{
    /// <summary>
    /// 忘记密码验证码通知
    /// </summary>
    public class ForgetPwdVerifyCodeNotify : NotifyBase
    {
        /// <summary>
        /// 验证码
        /// </summary>
        /// <value></value>
        public string Code { get; set; }

        /// <summary>
        /// 处理
        /// </summary>
        /// <returns></returns>
        public override string Execute()
        {
            return $"您的短信验证码是{Code}.您正在通过手机号重置登录密码，如非本人操作，请忽略该短信。";
        }
    }
}