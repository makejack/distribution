using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Models;

namespace Mytime.Distribution.Services
{
    /// <summary>
    /// 装货服务
    /// </summary>
    public class ShipmentService : IShipmentService
    {
        private readonly IRepositoryByInt<Shipment> _shipmentRepository;
        private readonly IRepositoryByInt<CustomerRelation> _customerRelationRepository;
        private readonly ICustomerManager _customerManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shipmentRepository"></param>
        /// <param name="customerRelationRepository"></param>
        /// <param name="customerManager"></param>
        public ShipmentService(IRepositoryByInt<Shipment> shipmentRepository,
        IRepositoryByInt<CustomerRelation> customerRelationRepository,
                               ICustomerManager customerManager)
        {
            _shipmentRepository = shipmentRepository;
            _customerRelationRepository = customerRelationRepository;
            _customerManager = customerManager;
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public async Task<Result> Confirm(int shipmentId)
        {
            var shipment = await _shipmentRepository.Query()
           .Include(e => e.ShipmentOrderItems).ThenInclude(e => e.OrderItem).ThenInclude(e => e.CommissionHistory)
           .FirstOrDefaultAsync(e => e.Id == shipmentId);
            if (shipment == null) return Result.Fail(ResultCodes.IdInvalid);

            if (shipment.ShippingStatus == ShippingStatus.Complete) return Result.Fail(ResultCodes.RequestParamError, "当前订单已完成确认收货");
            if (shipment.ShippingStatus != ShippingStatus.Shipped) return Result.Fail(ResultCodes.RequestParamError, "当前订单状态不允许确认收货");

            shipment.ShippingStatus = ShippingStatus.Complete;
            shipment.CompleteTime = DateTime.Now;

            var orderItems = shipment.ShipmentOrderItems.Select(e => e.OrderItem);
            var totalCommission = 0;
            foreach (var item in orderItems)
            {
                var RefundState = new[] { RefundStatus.ApplyFaild, RefundStatus.Default };
                if (!RefundState.Contains(item.RefundStatus)) continue;
                item.ShippingStatus = ShippingStatus.Complete;
                item.CompleteTime = DateTime.Now;
                item.WarrantyDeadline = DateTime.Now.AddYears(1);

                if (item.CommissionHistory != null)
                {
                    item.CommissionHistory.Status = CommissionStatus.Complete;
                    totalCommission += item.CommissionHistory.Commission;
                }
            }

            _shipmentRepository.Update(shipment, false);

            using (var transaction = _shipmentRepository.BeginTransaction())
            {
                await _shipmentRepository.SaveAsync();

                if (totalCommission > 0)
                {
                    var parentUser = await _customerRelationRepository.Query().FirstOrDefaultAsync(e => e.ChildrenId == shipment.CustomerId && e.Level == 1);
                    if (parentUser != null)
                    {
                        await _customerManager.UpdateAssets(parentUser.ParentId, -totalCommission, totalCommission);
                    }
                }

                transaction.Commit();
            }

            return Result.Ok();
        }

    }
}