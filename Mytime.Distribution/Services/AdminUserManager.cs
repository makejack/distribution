using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Events;
using Mytime.Distribution.Services.SmsContent;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 后台用户管理服务
    /// </summary>
    public class AdminUserManager : IAdminUserManager
    {
        private readonly IRepositoryByInt<AdminUser> _adminUserRepository;
        private readonly IMediator _mediator;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="adminUserRepository"></param>
        /// <param name="mediator"></param>
        public AdminUserManager(IRepositoryByInt<AdminUser> adminUserRepository,
                                IMediator mediator)
        {
            _adminUserRepository = adminUserRepository;
            _mediator = mediator;
        }

        /// <summary>
        /// 售后通知
        /// </summary>
        /// <returns></returns>
        public async Task AfterSaleNotify(INotify notify)
        {
            await SmsNotify(EmployeeRole.AfterSale, notify);
        }

        /// <summary>
        /// 开发人员通知
        /// </summary>
        /// <returns></returns>
        public async Task DevelopmentNotify()
        {
            var exceptionNotify = new ExceptionNotify();
            await SmsNotify(EmployeeRole.Development, exceptionNotify);
        }

        /// <summary>
        /// 账务通知
        /// </summary>
        /// <returns></returns>
        public async Task AccountingNotify(INotify notify)
        {
            await SmsNotify(EmployeeRole.Accounting, notify);
        }

        private async Task SmsNotify(EmployeeRole role, INotify notify)
        {
            var employeeTels = await _adminUserRepository.Query()
           .Where(e => e.Role == role)
           .Select(e => e.Tel)
           .ToListAsync();
            if (employeeTels.Count > 0)
            {
                var notifyEvent = new SmsNotifyEvent
                {
                    Tels = employeeTels,
                    Message = notify
                };
                await _mediator.Publish(notifyEvent);
            }
        }
    }
}