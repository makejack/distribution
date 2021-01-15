using System.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Shared;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Request;
using Mytime.Distribution.Models.V1.Response;
using System.Collections.Generic;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/admin/home")]
    [Produces("application/json")]
    public class AdminHomeController : ControllerBase
    {
        private readonly IRepositoryByInt<Orders> _orderRepository;
        private readonly IRepositoryByInt<Shipment> _shipmentRepository;
        private readonly IRepositoryByInt<Customer> _customerRepository;
        private readonly IRepositoryByInt<CustomerRelation> _customerRelationRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderRepository"></param>
        /// <param name="shipmentRepository"></param>
        /// <param name="customerRepository"></param>
        /// <param name="customerRelationRepository"></param>
        /// <param name="mapper"></param>
        public AdminHomeController(IRepositoryByInt<Orders> orderRepository,
                                   IRepositoryByInt<Shipment> shipmentRepository,
                                   IRepositoryByInt<Customer> customerRepository,
                                   IRepositoryByInt<CustomerRelation> customerRelationRepository,
                                   IMapper mapper)
        {
            _orderRepository = orderRepository;
            _shipmentRepository = shipmentRepository;
            _customerRepository = customerRepository;
            _customerRelationRepository = customerRelationRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 数据统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> DataStatistics()
        {
            // 总销售额
            var totalSales = await _orderRepository.Query().Where(e => e.OrderStatus >= OrderStatus.PaymentReceived).SumAsync(e => e.PaymentFee);
            // 总订单数
            var totalOrders = await _orderRepository.Query().Where(e => e.OrderStatus >= OrderStatus.PaymentReceived).CountAsync();
            // 待发货
            var pendingShipmentCount = await _shipmentRepository.Query().Where(e => e.ShippingStatus == ShippingStatus.PendingShipment).CountAsync();
            // 总合伙人数
            var totalCustomer = await _customerRepository.Query().Where(e => e.Role > PartnerRole.Default).CountAsync();

            return Result.Ok(new AdminHomeDataStatisticsResponse
            {
                TotalCustomer = totalCustomer,
                PendingShipmentCount = pendingShipmentCount,
                TotalOrders = totalOrders,
                TotalSales = totalSales
            });
        }

        /// <summary>
        /// 销售额
        /// </summary>
        /// <returns></returns>
        [HttpGet("sales")]
        public async Task<Result> Sales([FromQuery] AdminHomeSalesRequest request)
        {
            var flag = Enum.TryParse(typeof(DateTypes), request.Type, true, out var result);
            if (!flag) return Result.Fail(ResultCodes.RequestParamError, "Type 参数不正确");

            var startDate = DateTime.Now.Date;
            var endDate = startDate.AddDays(1);
            switch (result)
            {
                case DateTypes.Day:
                    var dayResult = await _orderRepository.Query()
                        .Where(e => e.OrderStatus >= OrderStatus.PaymentReceived && e.Createat >= startDate && e.Createat < endDate)
                        .GroupBy(e => e.Createat.Hour)
                        .Select(e => new AdminHomeSalesResponse<int>
                        {
                            Key = e.Key,
                            Amount = e.Sum(s => s.PaymentFee),
                            Quantity = e.Count()
                        }).ToListAsync();

                    for (int i = 1; i <= 24; i++)
                    {
                        var first = dayResult.FirstOrDefault(e => e.Key == i);
                        if (first == null)
                        {
                            dayResult.Add(new AdminHomeSalesResponse<int>
                            {
                                Key = i,
                                Amount = 0,
                                Quantity = 0,
                            });
                        }
                    }

                    return Result.Ok(dayResult.OrderBy(e => e.Key));
                case DateTypes.Week:
                    // var dayWeek = (int)startDate.DayOfWeek;

                    int week = startDate.DayOfWeek - DayOfWeek.Monday;
                    if (week == -1) week = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。 
                    TimeSpan ts = new TimeSpan(week, 0, 0, 0);
                    startDate = startDate.Subtract(ts);

                    // week = startDate.DayOfWeek - DayOfWeek.Sunday;
                    // if (week != 0) week = 7 - week;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。 
                    // ts = new TimeSpan(week, 0, 0, 0);

                    endDate = startDate.AddDays(8);

                    var weekResult = await _orderRepository.Query()
                        .Where(e => e.OrderStatus >= OrderStatus.PaymentReceived && e.Createat >= startDate && e.Createat < endDate)
                        .GroupBy(e => e.Createat.Date)
                        .Select(e => new AdminHomeSalesResponse<DateTime>
                        {
                            Key = e.Key,
                            Amount = e.Sum(s => s.PaymentFee),
                            Quantity = e.Count()
                        }).ToListAsync();

                    for (int i = 0; i < 7; i++)
                    {
                        var first = weekResult.FirstOrDefault(e => e.Key == startDate);
                        if (first == null)
                        {
                            weekResult.Add(new AdminHomeSalesResponse<DateTime>
                            {
                                Key = startDate,
                                Amount = 0,
                                Quantity = 0
                            });
                        }
                        startDate = startDate.AddDays(1);
                    }

                    return Result.Ok(weekResult.OrderBy(e => e.Key));
                case DateTypes.Month:
                    startDate = startDate.AddDays(-startDate.Day + 1);

                    var monthResult = await _orderRepository.Query()
                        .Where(e => e.OrderStatus >= OrderStatus.PaymentReceived && e.Createat >= startDate && e.Createat < endDate)
                        .GroupBy(e => e.Createat.Date)
                        .Select(e => new AdminHomeSalesResponse<DateTime>
                        {
                            Key = e.Key,
                            Amount = e.Sum(s => s.PaymentFee),
                            Quantity = e.Count()
                        }).ToListAsync();

                    var totalDay = (endDate - startDate).TotalDays;
                    for (int i = 0; i < totalDay; i++)
                    {
                        var first = monthResult.FirstOrDefault(e => e.Key == startDate);
                        if (first == null)
                        {
                            monthResult.Add(new AdminHomeSalesResponse<DateTime>
                            {
                                Key = startDate,
                                Amount = 0,
                                Quantity = 0
                            });
                        }
                        startDate = startDate.AddDays(1);
                    }

                    return Result.Ok(monthResult.OrderBy(e => e.Key));
                case DateTypes.Year:

                    startDate = startDate.AddDays(-startDate.DayOfYear + 1);

                    var yearResult = await _orderRepository.Query()
                        .Where(e => e.OrderStatus >= OrderStatus.PaymentReceived && e.Createat >= startDate && e.Createat < endDate)
                        .GroupBy(e => e.Createat.Month)
                        .Select(e => new AdminHomeSalesResponse<int>
                        {
                            Key = e.Key,
                            Amount = e.Sum(s => s.PaymentFee),
                            Quantity = e.Count()
                        }).ToListAsync();

                    for (int i = 1; i <= 12; i++)
                    {
                        var first = yearResult.FirstOrDefault(e => e.Key == i);
                        if (first == null)
                        {
                            yearResult.Add(new AdminHomeSalesResponse<int>
                            {
                                Key = i,
                                Amount = 0,
                                Quantity = 0
                            });
                        }
                    }
                    return Result.Ok(yearResult.OrderBy(e => e.Key));
                case DateTypes.Customize:

                    startDate = request.StartDate.Value;
                    endDate = request.EndDate.Value;

                    var customizeResult = await _orderRepository.Query()
                        .Where(e => e.OrderStatus >= OrderStatus.PaymentReceived && e.Createat >= startDate && e.Createat < endDate)
                        .GroupBy(e => e.Createat.Date)
                        .Select(e => new AdminHomeSalesResponse<DateTime>
                        {
                            Key = e.Key,
                            Amount = e.Sum(s => s.PaymentFee),
                            Quantity = e.Count()
                        }).ToListAsync();

                    return Result.Ok(customizeResult.OrderBy(e => e.Key));
            }

            return Result.Fail(ResultCodes.RequestParamError);
        }

        /// <summary>
        /// 团队销售
        /// </summary>
        /// <returns></returns>
        [HttpGet("teamsales")]
        public async Task<Result> TeamSales()
        {
            var childAmouns = await _customerRelationRepository.Query()
            .Where(e => e.Parent.ParentId == null)
            .Select(e => new
            {
                ParentId = e.ParentId,
                Amount = e.Children.Orders.Where(e => e.OrderStatus >= OrderStatus.PaymentReceived).Sum(e => e.TotalWithDiscount)
            }).ToListAsync();

            var childStatistics = childAmouns.GroupBy(e => e.ParentId).Select(e => new
            {
                Id = e.Key,
                ChildQuantity = e.Count(),
                TotalAmount = e.Sum(e => e.Amount)
            }).OrderByDescending(e => e.TotalAmount).Take(10);

            var parentIds = childStatistics.Select(e => e.Id);
            var customers = await _customerRepository.Query().Where(e => parentIds.Contains(e.Id)).ToListAsync();

            var responses = new List<AdminHomeTeamSalesResponse>();
            foreach (var item in customers)
            {
                var first = childStatistics.FirstOrDefault(e => e.Id == item.Id);

                responses.Add(new AdminHomeTeamSalesResponse
                {
                    Id = first.Id,
                    AvatarUrl = item.AvatarUrl,
                    NickName = item.NickName,
                    TeamQuantity = first.ChildQuantity,
                    TeamAmount = first.TotalAmount
                });
            }
            return Result.Ok(responses);
        }

    }
}