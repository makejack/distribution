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
using SixLabors.ImageSharp.PixelFormats;
using Mytime.Distribution.Utils.Helpers;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats.Png;
using System.Net.Http;

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

        private readonly IWebHostEnvironment _enviromenet;
        private readonly IHttpClientFactory _httpFactory;
        private readonly IRepositoryByInt<Customer> _customerRepository;
        private readonly ICustomerManager _customerManager;
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enviromenet"></param>
        /// <param name="httpFactory"></param>
        /// <param name="customerRepository"></param>
        /// <param name="customerManager"></param>
        /// <param name="mediator"></param>
        /// <param name="tokenService"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public WeChatController(IWebHostEnvironment enviromenet,
                                IHttpClientFactory httpFactory,
                                IRepositoryByInt<Customer> customerRepository,
                                ICustomerManager customerManager,
                                IMediator mediator,
                                ITokenService tokenService,
                                ILogger<WeChatController> logger,
                                IMapper mapper)
        {
            _enviromenet = enviromenet;
            _httpFactory = httpFactory;
            _customerRepository = customerRepository;
            _customerManager = customerManager;
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
            try
            {
                JsCode2JsonResult jsonResult = SnsApi.JsCode2Json(WechatService.WxOpenAppId, WechatService.WxOpenAppSecret, request.Code);
                if (jsonResult != null && jsonResult.errcode == ReturnCode.请求成功)
                {
                    var customer = await _customerRepository.Query()
                    .FirstOrDefaultAsync(e => e.OpenId == jsonResult.openid);
                    if (customer == null)
                    {
                        int? parentId = null;
                        var anyParent = await _customerRepository.Query().AnyAsync(e => e.Id == request.ParentId);
                        if (anyParent)
                        {
                            parentId = request.ParentId;
                        }

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
                                ParentId = request.ParentId,
                                ChildrenId = customer.Id
                            });
                        }
                    }
                    else
                    {
                        if (!customer.ParentId.HasValue && request.ParentId > 0)
                        {
                            var anyParent = await _customerRepository.Query().AnyAsync(e => e.Id == request.ParentId);
                            if (anyParent)
                            {
                                customer.ParentId = request.ParentId;
                                await _mediator.Publish(new CustomerRelationEvent
                                {
                                    ParentId = request.ParentId,
                                    ChildrenId = customer.Id
                                });
                            }
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
            var user = await _customerManager.GetUserAsync();
            var appId = WechatService.WxOpenAppId;
            var scene = $"id={user.Id}";
            var page = "pages/index/index";
            try
            {
                var myShareCodePath = Path.Combine("images", "sharecodes", user.Id + ".png");
                var physicalPath = Path.Combine(_enviromenet.WebRootPath, myShareCodePath);
                var httpPath = Request.GetHostUrl() + "/" + myShareCodePath.Replace(@"\", "/");
                if (System.IO.File.Exists(physicalPath))
                {
                    return Result.Ok(httpPath);
                }

                Image backgroundImg = null;
                string sharePath = Path.Combine(_enviromenet.WebRootPath, "images/20210107152422.png");
                // var qrCodeImgDemo = Path.Combine(_enviromenet.WebRootPath, "images/qrcode_example.png");
                if (System.IO.File.Exists(sharePath))
                {
                    backgroundImg = Image.Load(sharePath);
                }

                using (var stream = new MemoryStream())
                {
                    var wxResult = await WxAppApi.GetWxaCodeUnlimitAsync(appId, stream, scene, page);
                    if (wxResult.errcode == 0 && stream.Length > 1024)
                    {
                        Image qrCodeImg = Image.Load<Rgba32>(stream.GetBuffer());

                        if (!string.IsNullOrEmpty(user.AvatarUrl))
                        {
                            using (var httpClient = _httpFactory.CreateClient())
                            {
                                var response = await httpClient.GetAsync(user.AvatarUrl);
                                if (response.IsSuccessStatusCode)
                                {
                                    var avatarStream = await response.Content.ReadAsStreamAsync();
                                    var avatarPoint = 120;
                                    var avatarSize = 190;
                                    var avatarImg = Image.Load<Rgba32>(avatarStream).Clone(e => e.ConvertToAvatar(new Size(avatarSize, avatarSize), avatarSize / 2));
                                    qrCodeImg.Mutate(e =>
                                    {
                                        e.DrawImage(avatarImg, new Point(avatarPoint, avatarPoint), 1);
                                    });
                                }
                            }
                        }

                        var qrSize = 211;
                        var qrPoint = new Point(270, 620);
                        qrCodeImg = qrCodeImg.Clone(e => e.Resize(qrSize, qrSize));
                        backgroundImg.Mutate(e =>
                        {
                            e.DrawImage(qrCodeImg, qrPoint, 1);
                        });
                        using (var ms = new MemoryStream())
                        {
                            backgroundImg.Save(physicalPath, new PngEncoder());
                            // backgroundImg.SaveAsPng(ms);
                            // string base64String = $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
                            // return Result.Ok(base64String);
                        }
                        return Result.Ok(httpPath);
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