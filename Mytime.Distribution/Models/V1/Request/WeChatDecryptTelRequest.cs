namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 微信解密手机号
    /// </summary>
    public class WeChatDecryptTelRequest
    {
        /// <summary>
        /// EncryptedData
        /// </summary>
        /// <value></value>
        public string EncryptedData { get; set; }
/// <summary>
/// iv
/// </summary>
/// <value></value>
        public string Iv { get; set; }
    }
}