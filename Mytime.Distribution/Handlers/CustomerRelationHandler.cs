using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Events;

namespace Mytime.Distribution.Handlers
{
    /// <summary>
    /// 用户关系处理
    /// </summary>
    public class CustomerRelationHandler : INotificationHandler<CustomerRelationEvent>
    {
        private readonly IRepositoryByInt<CustomerRelation> _customerRelationRepository;
        private readonly ILogger<CustomerRelationHandler> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customerRelationRepository"></param>
        /// <param name="logger"></param>
        public CustomerRelationHandler(IRepositoryByInt<CustomerRelation> customerRelationRepository,
                                       ILogger<CustomerRelationHandler> logger)
        {
            _customerRelationRepository = customerRelationRepository;
            _logger = logger;
        }
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(CustomerRelationEvent notification, CancellationToken cancellationToken)
        {
            if (notification.ParentId == notification.ChildrenId) return;
            try
            {
                var customerRelations = new List<CustomerRelation>(){
                    new CustomerRelation(notification.ParentId, notification.ChildrenId, 1)
                };

                var relations = await _customerRelationRepository.Query().Where(e => e.ChildrenId == notification.ParentId).ToListAsync();
                if (relations.Count > 0)
                {
                    foreach (var item in relations)
                    {
                        customerRelations.Add(new CustomerRelation(item.ParentId, notification.ChildrenId, item.Level + 1));
                    }
                }

                using (var transaction = _customerRelationRepository.BeginTransaction())
                {
                    foreach (var item in customerRelations)
                    {
                        _customerRelationRepository.Insert(item, false);
                    }

                    await _customerRelationRepository.SaveAsync();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}