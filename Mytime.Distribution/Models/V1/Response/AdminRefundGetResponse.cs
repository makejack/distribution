using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台退款详情
    /// </summary>
    public class AdminRefundGetResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 装货Id
        /// </summary>
        /// <value></value>
        public int ShipmentId { get; set; }
        /// <summary>
        /// 退货类型
        /// </summary>
        /// <value></value>
        public ReturnTypes ReturnType { get; set; }
        /// <summary>
        /// 物流状态
        /// </summary>
        /// <value></value>
        public ReturnLogisticsStatus LogisticsStatus { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        /// <value></value>
        public string Reason { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string Description { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        /// <value></value>
        public int PaymentAmount { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        /// <value></value>
        public int RefundAmount { get; set; }
        /// <summary>
        /// 退款时间
        /// </summary>
        /// <value></value>
        public DateTime? RefundTime { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        /// <value></value>
        public ReturnAuditStatus Status { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        /// <value></value>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 审核消息
        /// </summary>
        /// <value></value>
        public string AuditMessage { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        /// <value></value>
        public string CourierCompany { get; set; }
        /// <summary>
        /// 快递公司Code
        /// </summary>
        /// <value></value>
        public string CourierCompanyCode { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        /// <value></value>
        public string TrackingNumber { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
        /// <summary>
        /// 退货商品
        /// </summary>
        /// <value></value>
        public AdminOrderItemResponse Goods { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        /// <value></value>
        public CustomerResponse Customer { get; set; }

    }
}