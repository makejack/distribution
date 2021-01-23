namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 付款方式
    /// </summary>
    public enum PaymentMethods : byte
    {
        Wechat = 0,
        AliPay,
        /// <summary>
        /// POS机
        /// </summary>
        POS,
        /// <summary>
        /// 钱包余额
        /// </summary>
        Wallet
    }
}