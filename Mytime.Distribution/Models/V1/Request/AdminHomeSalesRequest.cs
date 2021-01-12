using System;

namespace Mytime.Distribution.Models.V1.Request
{
    /// <summary>
    /// 日期类型
    /// </summary>
    public enum DateTypes : byte
    {
        /// <summary>
        /// 天
        /// </summary>
        Day,
        /// <summary>
        /// 周
        /// </summary>
        Week,
        /// <summary>
        /// 月
        /// </summary>
        Month,
        /// <summary>
        /// 年
        /// </summary>
        Year,
        /// <summary>
        /// 自定义
        /// </summary>
        Customize
    }

    /// <summary>
    /// 后台销售额请求
    /// </summary>
    public class AdminHomeSalesRequest
    {
        /// <summary>
        /// 搜索类型
        /// </summary>
        /// <value>Day 今天，Week 周， Month 月 ,year 年 ,customize 自定义</value>
        public string Type { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        /// <value></value>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        /// <value></value>
        public DateTime? EndDate { get; set; }
    }
}