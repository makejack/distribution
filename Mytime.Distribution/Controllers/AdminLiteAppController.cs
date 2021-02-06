using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台小程序
    /// </summary>
    [Authorize(Roles = "Admin,Goods")]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/liteapp")]
    [Produces("application/json")]
    public class AdminLiteAppController : ControllerBase
    {
        private readonly IRepositoryByInt<LiteAppSetting> _repository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public AdminLiteAppController(IRepositoryByInt<LiteAppSetting> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> Get()
        {
            var setting = await _repository.Query().FirstOrDefaultAsync();
            if (setting == null)
            {
                setting = new LiteAppSetting();
            }

            return Result.Ok(setting);
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Save(AdminLiteAppSaveRequest request)
        {
            var setting = await _repository.Query().FirstOrDefaultAsync();
            if (setting == null)
            {
                setting = new LiteAppSetting
                {
                    CityMembershipRights = request.CityMembershipRights,
                    BranchMembershipRights = request.BranchMembershipRights,
                    Createat = DateTime.Now
                };
                await _repository.InsertAsync(setting);
            }
            else
            {
                setting.CityMembershipRights = request.CityMembershipRights;
                setting.BranchMembershipRights = request.BranchMembershipRights;

                await _repository.UpdateAsync(setting);
            }

            return Result.Ok();
        }
    }
}