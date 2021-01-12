namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台首页销售额
    /// </summary>
    public class AdminHomeSalesResponse<T>
    {
        /// <summary>
        /// 日期
        /// </summary>
        /// <value></value>
        public T Key { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        /// <value></value>
        public int Amount { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        /// <value></value>
        public int Quantity { get; set; }
    }
}