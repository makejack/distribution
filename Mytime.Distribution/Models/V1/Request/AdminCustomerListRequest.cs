using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台顾客列表请求
    /// </summary>
    public class AdminCustomerListRequest : PaginationRequest
    {
        /// <summary>
        /// 角色
        /// </summary>
        /// <value></value>
        public PartnerRole? Role { get; set; }
        /// <summary>
        /// 顾客名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <value></value>
        public string Tel { get; set; }
    }
}