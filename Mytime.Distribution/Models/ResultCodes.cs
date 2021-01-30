using System.ComponentModel;

namespace Mytime.Distribution.Models
{
    /// <summary>
    /// 结果码
    /// </summary>
    public enum ResultCodes : int
    {
        /// <summary>
        /// 系统错误，请稍后尝试。
        /// </summary>
        [Description("系统错误，请稍后尝试。")]
        SysError = -1,
        /// <summary>
        /// 请求成功
        /// </summary>
        [Description("请求成功")]
        Ok = 0,

        /// <summary>
        /// 余额支付
        /// </summary>
        BalancePayment = 200,

        /// <summary>
        /// 请求参数错误
        /// </summary>
        [Description("请求参数错误")]
        RequestParamError = 4000,
        /// <summary>
        /// Id无效
        /// </summary>
        [Description("Id无效")]
        IdInvalid,
        /// <summary>
        /// 无效Token
        /// </summary>
        [Description("无效Token")]
        InvalidToken,
        /// <summary>
        /// Token有效未过期
        /// </summary>
        [Description("Token有效未过期")]
        TokenValid,
        /// <summary>
        /// 无效刷新Token
        /// </summary>
        [Description("无效刷新Token")]
        InvalidRefreshToken,
        /// <summary>
        /// Token过期
        /// </summary>
        [Description("Token过期")]
        TokenTimeout,
        /// <summary>
        /// Token已使用
        /// </summary>
        [Description("Token已使用")]
        TokenUsed,
        /// <summary>
        /// 用户不存在
        /// </summary>
        [Description("用户不存在")]
        UserNotExists,
        /// <summary>
        /// 密码错误
        /// </summary>
        [Description("密码错误")]
        PasswordError,
        /// <summary>
        /// 用户存在
        /// </summary>
        [Description("用户存在")]
        UserExists,
        /// <summary>
        /// 不可以注册
        /// </summary>
        [Description("不可以注册")]
        NotRegister,
        /// <summary>
        /// 支付请求错误
        /// </summary>
        [Description("支付请求错误")]
        PaymentRequestError,
    }
}