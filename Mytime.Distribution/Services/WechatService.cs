using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Template;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 微信服务
    /// </summary>
    public class WechatService : IWechatService
    {

        /// <summary>
        /// 小程序Token
        /// </summary>
        public static readonly string Token = Config.SenparcWeixinSetting.WxOpenToken;//与微信小程序后台的Token设置保持一致，区分大小写。
        /// <summary>
        /// 小程序Key
        /// </summary>
        public static readonly string EncodingAESKey = Config.SenparcWeixinSetting.WxOpenEncodingAESKey;//与微信小程序后台的EncodingAESKey设置保持一致，区分大小写。
        /// <summary>
        /// 小程序AppId
        /// </summary>
        public static readonly string WxOpenAppId = Config.SenparcWeixinSetting.WxOpenAppId;//与微信小程序后台的AppId设置保持一致，区分大小写。
        /// <summary>
        /// 小程序Secret
        /// </summary>
        public static readonly string WxOpenAppSecret = Config.SenparcWeixinSetting.WxOpenAppSecret;//与微信小


    }

    /// <summary>
    /// 发送模板结果
    /// /// </summary>
    public class TemplateResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <value></value>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        /// <value></value>
        public string Message { get; set; }
    }
}