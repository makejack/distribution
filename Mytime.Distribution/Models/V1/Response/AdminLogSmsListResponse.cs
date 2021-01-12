using System;

namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台短信列表
    /// </summary>
    public class AdminLogSmsListResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <value></value>
        public string Tel { get; set; }
        /// <summary>
        /// code
        /// </summary>
        /// <value></value>
        public string Code { get; set; }
        /// <summary>
        /// msgid
        /// </summary>
        /// <value></value>
        public string MsgId { get; set; }
        /// <summary>
        /// time
        /// </summary>
        /// <value></value>
        public string Time { get; set; }
        /// <summary>
        /// message
        /// </summary>
        /// <value></value>
        public string Message { get; set; }
        /// <summary>
        /// errormsg
        /// </summary>
        /// <value></value>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime Createat { get; set; }
    }
}