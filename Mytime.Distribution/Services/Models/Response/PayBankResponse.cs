namespace Mytime.Distribution.Services.Models.Response
{
    /// <summary>
    /// 提现到银行卡响应
    /// </summary>
    public class PayBankResponse
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PayBankResponse()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="message"></param>
        public PayBankResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <value></value>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        /// <value></value>
        public string Message { get; set; }
    }
}