using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customerRelationRepository"></param>
        public CustomerRelationHandler(IRepositoryByInt<CustomerRelation> customerRelationRepository)
        {
            _customerRelationRepository = customerRelationRepository;
        }
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(CustomerRelationEvent notification, CancellationToken cancellationToken)
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
                await _customerRelationRepository.SaveAsync();

                transaction.Commit();
            }
        }
    }
}