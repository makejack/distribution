using System;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 用户响应
    /// </summary>
    public class CustomerResponse
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
        /// 昵称
        /// </summary>
        /// <value></value>
        public string NickName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <value></value>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 区号
        /// </summary>
        /// <value></value>
        public string CountryCode { get; set; }
        /// <summary>
        /// 性别 
        /// </summary>
        /// <value></value>
        public int Gender { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        /// <value></value>
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        /// <value></value>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        /// <value></value>
        public string City { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        /// <value></value>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        /// <value></value>
        public string Language { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        /// <value></value>
        public PartnerRole Role { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <value></value>
        public byte Status { get; set; }
        /// <summary>
        /// 是否实名
        /// </summary>
        /// <value></value>
        public bool IsRealName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}