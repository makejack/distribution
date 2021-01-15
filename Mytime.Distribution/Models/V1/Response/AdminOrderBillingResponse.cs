using System;
namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台订单票据响应
    /// </summary>
    public class AdminOrderBillingResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        /// <value></value>
        public int OrderId { get; set; }
        /// <summary>
        /// 是否已开票
        /// </summary>
        /// <value></value>
        public bool IsInvoiced { get; set; }
        /// <summary>
        /// 抬头名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 抬头税号
        /// </summary>
        public string TaxNumber { get; set; }
        /// <summary>
        /// 单位地址
        /// </summary>
        public string CompanyAddress { get; set; }
        /// <summary>
        /// 公司电话
        /// </summary>
        public string TelePhone { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankAccount { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}