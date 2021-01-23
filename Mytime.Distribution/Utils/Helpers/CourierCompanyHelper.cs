using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
            var codes = JsonConvert.DeserializeObject<List<KuaidiCode>>(Kuaidi100Code.Codes);
            return codes.Where(e => e.Code == code).Select(e => e.Title).FirstOrDefault();
        }
    }
}