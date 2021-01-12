using System.IO;
using System;
using System.Net.Sockets;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Entities;
using Mytime.Distribution.Services;
using System.Collections.Generic;
using System.Security.Claims;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Extensions;
using Microsoft.Extensions.Logging;
using MediatR;
using Mytime.Distribution.Events;
using Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 微信
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/wechat")]
    [Produces("application/json")]
    public class WeChatController : ControllerBase
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

        private readonly IWebHostEnvironment _enviromenet;
        private readonly IRepositoryByInt<Customer> _customerRepository;
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enviromenet"></param>
        /// <param name="customerRepository"></param>
        /// <param name="mediator"></param>
        /// <param name="tokenService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public WeChatController(IWebHostEnvironment enviromenet,
                                IRepositoryByInt<Customer> customerRepository,
                                IMediator mediator,
                                ITokenService tokenService,
                                ILogger<WeChatController> logger,
                                IMapper mapper)
        {
            _enviromenet = enviromenet;
            _customerRepository = customerRepository;
            _mediator = mediator;
            _tokenService = tokenService;
            _logger = logger;
            _mapper = mapper;
        }


        /// <summary>
        /// 小程序授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("liteapplogin")]
        public async Task<Result> LiteAppLogin([FromBody] WeChatLiteAppLoginRequest request)
        {
            string errorMsg = string.Empty;
            int? parentId = null;
            if (request.ParentId > 0)
            {
                parentId = request.ParentId;
            }
            try
            {
                JsCode2JsonResult jsonResult = SnsApi.JsCode2Json(WxOpenAppId, WxOpenAppSecret, request.Code);
                if (jsonResult != null && jsonResult.errcode == ReturnCode.请求成功)
                {
                    var customer = await _customerRepository.Query()
                    .FirstOrDefaultAsync(e => e.OpenId == jsonResult.openid);
                    if (customer == null)
                    {
                        var userInfo = request.UserInfo;
                        customer = new Customer
                        {
                            NickName = userInfo.NickName,
                            OpenId = jsonResult.openid,
                            SessionKey = jsonResult.session_key,
                            UnionId = jsonResult.unionid,
                            Gender = userInfo.Gender,
                            Country = userInfo.Country,
                            Province = userInfo.Province,
                            City = userInfo.City,
                            AvatarUrl = userInfo.AvatarUrl,
                            Language = userInfo.Language,
                            ParentId = parentId,
                            Createat = DateTime.Now,
                            Assets = new Assets()
                            {
                                TotalAssets = 0,
                                AvailableAmount = 0,
                                TotalCommission = 0,
                                UpdateTime = DateTime.Now,
                                Createat = DateTime.Now
                            }
                        };
                        await _customerRepository.InsertAsync(customer);

                        if (parentId.HasValue)
                        {
                            await _mediator.Publish(new CustomerRelationEvent
                            {
                                ParentId = parentId.Value,
                                ChildrenId = customer.Id
                            });
                        }
                    }
                    else
                    {
                        if (!customer.ParentId.HasValue && parentId.HasValue)
                        {
                            customer.ParentId = parentId;

                            await _mediator.Publish(new CustomerRelationEvent
                            {
                                ParentId = parentId.Value,
                                ChildrenId = customer.Id
                            });
                        }
                        if (customer.SessionKey != jsonResult.session_key)
                        {
                            customer.SessionKey = jsonResult.session_key;
                            await _customerRepository.UpdateProperyAsync(customer, nameof(customer.SessionKey), nameof(customer.ParentId));
                        }
                    }

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("id", customer.Id.ToString()));
                    // claims.Add(new Claim("openId", customer.OpenId));

                    var token = _tokenService.JwtToken(claims);
                    var customerRes = _mapper.Map<CustomerResponse>(customer);

                    return Result.Ok(new WeChatLiteAppLoginResponse(token, customerRes));
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return Result.Fail(ResultCodes.RequestParamError, errorMsg);
        }

        /// <summary>
        /// 解密手机号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("decrypttel")]
        public async Task<Result> DecryptTel([FromBody] WeChatDecryptTelRequest request)
        {
            var userId = HttpContext.GetUserId();
            var customer = await _customerRepository.FirstOrDefaultAsync(userId);
            try
            {
                string data = Senparc.Weixin.WxOpen.Helpers.EncryptHelper.DecodeEncryptedData(customer.SessionKey, request.EncryptedData, request.Iv);
                var decodedPhoneNumber = JsonConvert.DeserializeObject<DecodedPhoneNumber>(data);
                if (string.IsNullOrEmpty(customer.PhoneNumber))
                {
                    customer.PhoneNumber = decodedPhoneNumber.phoneNumber;
                    customer.CountryCode = decodedPhoneNumber.countryCode;

                    await _customerRepository.UpdateProperyAsync(customer, nameof(customer.PhoneNumber), nameof(customer.CountryCode));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return Result.Fail(ResultCodes.SysError);
        }

        /// <summary>
        /// 小程序码
        /// </summary>
        /// <returns></returns>
        [HttpGet("qrcode")]
        public async Task<Result> QRCode()
        {
            var userId = HttpContext.GetUserId();
            var appId = Config.SenparcWeixinSetting.WxOpenAppId;
            var scene = $"id={userId}";
            var page = "home/index";
            try
            {

                Image backgroundImg = null;
                string sharePath = Path.Combine(_enviromenet.WebRootPath, "images/20210107152422.png");
                var qrCodeImgDemo = Path.Combine(_enviromenet.WebRootPath, "images/qrcode_example.png");
                if (System.IO.File.Exists(sharePath))
                {
                    backgroundImg = Image.Load(sharePath);
                }

                using (var stream = new MemoryStream())
                {
                    var wxResult = await WxAppApi.GetWxaCodeUnlimitAsync(appId, stream, scene, page);
                    if (wxResult.errcode == 0 && stream.Length > 1024)
                    {
                        Image qrCodeImg = Image.Load(stream);
                        qrCodeImg = qrCodeImg.Clone(e => e.Resize(211, 211));
                        backgroundImg.Mutate(e =>
                        {
                            e.DrawImage(qrCodeImg, new Point(270, 620), 1);
                        });
                        using (var ms = new MemoryStream())
                        {
                            backgroundImg.Save(ms, new JpegEncoder()
                            {
                                Quality = 50
                            });
                            string base64String = $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
                            return Result.Ok(base64String);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Result.Fail(ResultCodes.SysError, ex.Message);
            }
            return Result.Fail(ResultCodes.RequestParamError, "请求生成小程序码失败");
        }
    }
}