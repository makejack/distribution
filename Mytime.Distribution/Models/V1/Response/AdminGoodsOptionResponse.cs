using System.Collections.Generic;
using System;
using System.Data.Common;
namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台商品选项响应
    /// </summary>
    public class AdminGoodsOptionResponse
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
    }
}