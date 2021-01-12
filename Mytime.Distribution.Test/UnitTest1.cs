using System.Collections.Specialized;
using System;
using Xunit;
using Mytime.Distribution;
using Mytime.Distribution.Services;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Domain.Entities;
using Moq;
using Mytime.Distribution.Models;
using Mytime.Distribution.Services.Models.Request;
using Mytime.Distribution.EFCore.Repositories;
using Mytime.Distribution.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Mytime.Distribution.Test
{
    public class UnitTest1
    {
        [Fact]
        public void PaymentReceived()
        {
            string connectionString = "Data Source=localhost;Initial Catalog=mytime_distribution;User ID=sa;Password=sa";
            var options = new DbContextOptionsBuilder<AppDatabase>().UseSqlServer(connectionString).Options;
            AppDatabase db = new AppDatabase(options);
            //Given
            // var dbMock = new Mock<AppDatabase>(options);
            // var a = orderMock.Object.FirstOrDefaultAsync(10027);
            // var order = a.Result;
            // var mock = new Mock<IRepositoryByInt<Orders>>();
            // mock.Setup(s => s.FirstOrDefaultAsync(10018)).ReturnsAsync(new Orders
            // {
            //     Createat = DateTime.Now,
            //     Id = 10018,
            //     OrderNo = "20201230793793187574448128",
            //     OrderStatus = Domain.Shared.OrderStatus.PendingPayment
            // });
            // var cMock = new Mock<IRepositoryByInt<Customer>>();
            // cMock.Setup(s => s.FirstOrDefaultAsync(1)).ReturnsAsync(new Customer
            // {
            //     Id = 10001,
            //     NickName = "sea",
            //     Role = Domain.Shared.PartnerRole.Default
            // });

            IRepositoryByInt<Orders> orderRes = new RepositoryByInt<Orders>(db);
            IRepositoryByInt<Customer> customerRes = new RepositoryByInt<Customer>(db);
            var order = orderRes.FirstOrDefaultAsync(1).Result;
            var customer = customerRes.FirstOrDefaultAsync(0).Result;
            PaymentReceivedRequest request = new PaymentReceivedRequest
            {
                OrderNo = "20201230793793187574448128",
                PaymentFee = 1100,
                PaymentMethod = Domain.Shared.PaymentMethods.Wechat,
                PaymentTime = DateTime.Now
            };
            //When

            //Then

            IOrderService orderService = new OrderService(orderRes, customerRes, null);
            orderService.PaymentReceived(request);
        }
    }
}
