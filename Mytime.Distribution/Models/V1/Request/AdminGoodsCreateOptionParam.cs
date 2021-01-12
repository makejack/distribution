using System.Collections.Generic;
namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 后台商品创建选项参数
    /// </summary>
    public class AdminGoodsCreateOptionParam
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 选项值
        /// </summary>
        /// <value></value>
        public List<AdminGoodsCreateOptionValueParam> Values { get; set; }
    }
}