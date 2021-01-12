using System.Collections.Generic;
namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台顾客信息
    /// </summary>
    public class AdminCustomerGetResponse : CustomerResponse
    {
        /// <summary>
        /// 资产
        /// </summary>
        /// <value></value>
        public AssetsResponse Assets { get; set; }
    }
}