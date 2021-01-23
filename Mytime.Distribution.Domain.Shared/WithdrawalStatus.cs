namespace Mytime.Distribution.Domain.Shared
{
    /// <summary>
    /// 提现状态
    /// </summary>
    public enum WithdrawalStatus : byte
    {
        /// <summary>
        /// 申请
        /// </summary>
        Apply = 0,
        /// <summary>
        /// 失败
        /// </summary>
        Failed,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200
    }
}