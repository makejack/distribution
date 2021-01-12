using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mytime.Distribution.Configs;
using Mytime.Distribution.Utils.Helpers;
using Newtonsoft.Json;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 快递服务
    /// </summary>
    public class PackageService : IPackageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Kuaidi100Config _config;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="options"></param>
        public PackageService(IHttpClientFactory httpClientFactory,
                              IOptions<Kuaidi100Config> options)
        {
            _httpClientFactory = httpClientFactory;
            _config = options.Value;
        }

        /// <summary>
        /// 快递查询
        /// </summary>
        /// <param name="com">快递公司</param>
        /// <param name="num">快递单号</param>
        /// <param name="phone">收、寄件人手机号</param>
        /// <returns></returns>
        public async Task<string> QueryAsync(string com, string num, string phone)
        {
            string result = string.Empty;
            var client = _httpClientFactory.CreateClient();

            string customer = _config.Customer;
            string key = _config.Key;

            IDictionary<string, string> dicParam = new Dictionary<string, string>();
            dicParam.Add("com", com);
            dicParam.Add("num", num);
            dicParam.Add("phone", phone);
            dicParam.Add("from", "");
            dicParam.Add("to", "");
            dicParam.Add("resultv2", "1");
            dicParam.Add("show", "0");
            dicParam.Add("order", "desc");

            string param = JsonConvert.SerializeObject(dicParam);
            string sign = EncryptHelper.MD5ToHex(param + key + customer, Encoding.UTF8);

            string url = $"https://poll.kuaidi100.com/poll/query.do?customer={customer}&sign={sign}&param={param}";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync(); ;
            }
            return result;
        }
    }
}