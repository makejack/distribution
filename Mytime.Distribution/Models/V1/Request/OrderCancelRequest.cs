using System.Reflection.Metadata.Ecma335;
namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 订单取消请求
    /// </summary>
    public class OrderCancelRequest
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        /// <value></value>
        public string Reason { get; set; }
    }
}