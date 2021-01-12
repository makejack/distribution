using System;
using System.Collections.Generic;
namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台获取商品选项值
    /// </summary>
    public class AdminGoodsGetOptionValueResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <value></value>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        /// <value></value>
        public string Value { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}