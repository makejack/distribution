using System;
using System.Collections.Generic;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台获取商品选项响应
    /// </summary>
    public class AdminGoodsGetOptionResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
/// <summary>
/// 名称
/// </summary>
/// <value></value>
        public string Name { get; set; }
/// <summary>
/// 创建时间
/// </summary>
/// <value></value>
        public DateTime Createat { get; set; }
/// <summary>
/// 商品选项值
/// </summary>
/// <value></value>
        public List<AdminGoodsGetOptionValueResponse> Values { get; set; }
    }
}