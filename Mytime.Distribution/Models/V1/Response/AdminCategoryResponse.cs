using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台分类响应
    /// </summary>
    public class AdminCategoryResponse
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
        /// 排序
        /// </summary>
        /// <value></value>
        public int Sort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}