namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 模型验证失败响应
    /// </summary>
    public class ModelValidatorFailedResponse
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msg"></param>
        public ModelValidatorFailedResponse(string key, string msg)
        {
            this.Key = key;
            this.Msg = msg;

        }
        /// <summary>
        /// Key
        /// </summary>
        /// <value></value>
        public string Key { get; set; }
        /// <summary>
        /// Msg
        /// </summary>
        /// <value></value>
        public string Msg { get; set; }
    }
}