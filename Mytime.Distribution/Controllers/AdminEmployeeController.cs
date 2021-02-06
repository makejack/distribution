using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Extensions;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Domain.Shared;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 后台员工管理
    /// </summary>
    [Authorize(Roles = "Admin")]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/employee")]
    [Produces("application/json")]
    public class AdminEmployeeController : ControllerBase
    {
        private readonly IRepositoryByInt<AdminUser> _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public AdminEmployeeController(IRepositoryByInt<AdminUser> repository,
                                       IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// 员工列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<Result> List([FromQuery] PaginationRequest request)
        {
            var queryable = _repository.Query();

            var totalRows = await queryable.CountAsync();
            var users = await queryable.OrderByDescending(e => e.Id)
            .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
            .ToListAsync();

            return Result.Ok(new PaginationResponse(request.Page, totalRows, _mapper.Map<List<AdminEmployeeResponse>>(users)));
        }

        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> Get(int id)
        {
            var user = await _repository.FirstOrDefaultAsync(id);
            if (user == null) return Result.Fail(ResultCodes.IdInvalid);

            return Result.Ok(_mapper.Map<AdminEmployeeResponse>(user));
        }

        /// <summary>
        /// 创建员工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Create([FromBody] AdminEmployeeCreateRequest request)
        {
            var anyName = await _repository.Query().AnyAsync(e => e.Name == request.Name || e.Tel == request.Tel);
            if (anyName) return Result.Fail(ResultCodes.UserExists, "用户名或手机号已存在");

            var user = new AdminUser
            {
                Name = request.Name,
                Createat = DateTime.Now,
                IsAdmin = false,
                NickName = request.NickName,
                Role = request.Role,
                Tel = request.Tel,
                Pwd = request.Pwd.ToMD5Base64()
            };
            await _repository.InsertAsync(user);

            return Result.Ok();
        }

        /// <summary>
        /// 编辑员工
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Result> Edit([FromBody] AdminEmployeeEditRequest request)
        {
            var user = await _repository.FirstOrDefaultAsync(request.Id);
            if (user == null) return Result.Fail(ResultCodes.IdInvalid);

            if (user.Tel != request.Tel)
            {
                var anyTel = await _repository.Query().AnyAsync(e => e.Tel == request.Tel);
                if (anyTel) return Result.Fail(ResultCodes.RequestParamError, "手机号已存在");
            }

            user.NickName = request.NickName;
            if (!user.IsAdmin)
            {
                user.Role = request.Role;
            }
            user.Tel = request.Tel;

            await _repository.UpdateAsync(user);

            return Result.Ok();
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var userId = HttpContext.GetUserId();
            var user = await _repository.FirstOrDefaultAsync(userId);

            var deleteUser = await _repository.FirstOrDefaultAsync(id);
            if (deleteUser == null) return Result.Fail(ResultCodes.IdInvalid);
            if (deleteUser.IsAdmin) return Result.Fail(ResultCodes.RequestParamError, "当前用户是管理员不允许删除");
            if (!user.IsAdmin && deleteUser.Role == EmployeeRole.Admin) Result.Fail(ResultCodes.RequestParamError, "当前用户是管理员不允许删除");

            await _repository.RemoveAsync(deleteUser);

            return Result.Ok();
        }
    }
}