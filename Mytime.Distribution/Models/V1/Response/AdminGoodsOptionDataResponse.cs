using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台商品选项数据响应
    /// </summary>
    public class AdminGoodsOptionDataResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 选项Id
        /// </summary>
        /// <value></value>
        public int OptionId { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        /// <value></value>
        public string Value { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}