using System.Collections.Generic;

namespace Mytime.Distribution.Utils.Helpers
{
    /// <summary>
    /// 快递公司帮助
    /// </summary>
    public class CourierCompanyHelper
    {
        /// <summary>
        /// 获取快递公司名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetCompanyName(string code)
        {
            return (GetCompanys())[code];
        }

        private static Dictionary<string, string> GetCompanys()
        {
            return new Dictionary<string, string>
            {
                {"yuantong","圆通速递"},
                {"yunda","韵达快递"},
                {"zhongtong","中通快递"},
                {"youzhengguonei","邮政快递包裹"},
                {"huitongkuaidi","百世快递"},
                {"shunfeng","顺丰速运"},
                {"shentong","申通快递"},
                {"jd","京东物流"},
                {"ems","EMS"},
                {"tiantian","天天快递"},
                {"jtexpress","极兔速递"},
                {"youzhengbk","邮政标准快递"},
                {"debangwuliu","德邦"},
                {"debangkuaidi","德邦快递"},
                {"zhongyouex","众邮快递"},
                {"zhaijisong","宅急送"},
                {"youshuwuliu","优速快递"},
                {"baishiwuliu","百世快运"},
                {"suning","苏宁物流"},
            };
        }
    }
}