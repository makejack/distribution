using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mytime.Distribution.Configs;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Mytime.Distribution.Services.SmsContent;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 短信服务
    /// </summary>
    public class SmsService : ISmsService
    {
        private readonly IRepositoryByInt<SmsLog> _smsLogRepository;
        private readonly SmsConfig _config;
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<SmsService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="smsLogRepository"></param>
        /// <param name="config"></param>
        /// <param name="httpFactory"></param>
        /// <param name="logger"></param>
        public SmsService(IRepositoryByInt<SmsLog> smsLogRepository,
                          IOptions<SmsConfig> config,
                          IHttpClientFactory httpFactory,
                          ILogger<SmsService> logger)
        {
            _smsLogRepository = smsLogRepository;
            _config = config.Value;
            _httpFactory = httpFactory;
            _logger = logger;
        }

        /// <summary>
        /// 短信发送
        /// </summary>
        /// <returns></returns>
        public async Task SendAsync(string tel, INotify notify)
        {
            if(string.IsNullOrEmpty(tel)) return;
            var httpClient = _httpFactory.CreateClient();

            var msg = notify.Execute();
            var request = new Cloud253SmsRequest()
            {
                Account = _config.Account,
                Password = _config.Password,
                Phone = tel,
                Msg = msg,
                Report = true
            };

            var log = new SmsLog
            {
                Tel = tel,
                Message = msg,
                Createat = DateTime.Now
            };

            try
            {
                var context = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(_config.Url, context))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonBody = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Cloud243SmsResponse>(jsonBody);

                        log.Code = result.Code;
                        log.ErrorMsg = result.ErrorMsg;
                        log.Time = result.Time;
                        log.MsgId = result.MsgId;
                    }

                    await _smsLogRepository.InsertAsync(log);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }

    /// <summary>
    /// cloud253 响应
    /// </summary>
    public class Cloud243SmsResponse
    {
        /// <summary>
        /// Code
        /// </summary>
        /// <value></value>
        public string Code { get; set; }
        /// <summary>
        /// MsgId
        /// </summary>
        /// <value></value>
        public string MsgId { get; set; }
        /// <summary>
        /// Time
        /// </summary>
        /// <value></value>
        public string Time { get; set; }
        /// <summary>
        /// ErrorMsg
        /// </summary>
        /// <value></value>
        public string ErrorMsg { get; set; }
    }

    /// <summary>
    /// cloud253 请求参数
    /// </summary>
    public class Cloud253SmsRequest
    {
        /// <summary>
        /// 账号
        /// </summary>
        /// <value></value>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <value></value>
        [JsonProperty("password")]
        public string Password { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <value></value>
        [JsonProperty("phone")]
        public string Phone { get; set; }
        /// <summary>
        /// 报告
        /// </summary>
        /// <value></value>
        [JsonProperty("report")]
        public bool Report { get; set; }
        /// <summary>
        /// 信息的内容
        /// </summary>
        /// <value></value>
        [JsonProperty("msg")]
        public string Msg { get; set; }
    }
}