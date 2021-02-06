using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Services;
using Mytime.Distribution.Services.SmsContent;
using Mytime.Distribution.Utils.Helpers;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 忘记密码
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/forget")]
    [Produces("application/json")]
    public class AdminForgetController : ControllerBase
    {
        private readonly IRepositoryByInt<AdminUser> _adminUserRepository;
        private readonly ISmsService _smsService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="adminUserRepository"></param>
        /// <param name="smsService"></param>
        /// <param name="mapper"></param>
        public AdminForgetController(IRepositoryByInt<AdminUser> adminUserRepository,
                                     ISmsService smsService,
                                     IMapper mapper)
        {
            _adminUserRepository = adminUserRepository;
            _smsService = smsService;
            _mapper = mapper;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> ResetPwd([FromBody] AdminForgetResetPwdRequest request)
        {
            var user = await _adminUserRepository.Query().FirstOrDefaultAsync(e => e.Tel == request.Tel);
            if (user == null) return Result.Fail(ResultCodes.UserNotExists);

            var code = HttpContext.Session.GetString(request.Tel);
            if (string.IsNullOrEmpty(code)) return Result.Fail(ResultCodes.RequestParamError, "验证码已过期或未获取验证码");
            if (!request.Code.Equals(code, System.StringComparison.InvariantCultureIgnoreCase))
                return Result.Fail(ResultCodes.RequestParamError, "验证码不正确");

            user.Pwd = request.Password.ToMD5Base64();

            await _adminUserRepository.UpdateAsync(user);

            HttpContext.Session.Remove(request.Tel);

            return Result.Ok();
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("verifycode")]
        public async Task<Result> VerifyCode([FromQuery] string tel)
        {
            var anyTel = await _adminUserRepository.Query().AnyAsync(e => e.Tel == tel);
            if (!anyTel) return Result.Fail(ResultCodes.UserNotExists);

            var code = GenerateHelper.VerifyCode(4);
            HttpContext.Session.SetString(tel, code);

            var notify = new ForgetPwdVerifyCodeNotify
            {
                Code = code
            };
            await _smsService.SendAsync(tel, notify);

            return Result.Ok();
        }
    }
}