namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 微信用户地址编辑请求
    /// </summary>
    public class CustomerAddressEditRequest : CustomerAddressCreateRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
    }
}