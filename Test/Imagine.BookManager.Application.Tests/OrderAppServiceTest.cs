using System;
using System.Linq;
using Abp.UI;
using Imagine.BookManager.Common;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Order;
using Imagine.BookManager.OrderService;
using Shouldly;
using Xunit;

namespace Imagine.BookManager.Application.Tests
{
    public class OrderAppServiceTest : BookManagerTestBase
    {
        private readonly IOrderAppService _orderAppService;

        public OrderAppServiceTest()
        {
            _orderAppService = Resolve<IOrderAppService>();
        }

        Institution GetFakeInstitution()
        {
            return new Institution
            {
                Address = "上海",
                District = "上海市浦东新区",
                Name = "testOrder",
                Tel = "123456"
            };
        }

        Admin GetFakeAdmin()
        {
            return new Admin
            {
                DateCreated = DateTime.Now,
                Email = "brian2@imaginelearning.cn",
                FullName = "brian",
                Gender = true,
                IsDelete = false,
                Mobile = "18817617807",
                Password = "123456",
                UserName = "testOrder",
                UserType = UserType.Teacher
            };
        }

        Order GetFakeOrder()
        {
            return new Order
            {
                DeliveryCharge = 1000,
                Discount = 1000,
                OrderRef = "1234567",
                Subtotal = 1000,
                Timestamp = DateTime.Now,
                Total = 10000,
                TotalQuantity = 100
            };
        }



        #region CreateOrder
        [Fact]
        public void CreateOrder_Return_Ture_If_Success()
        {

            var institution = UsingDbContext(ctx => ctx.Institution.Add(GetFakeInstitution()));
            var adminEntity = GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            OrderDto order = new OrderDto()
            {
                DeliveryCharge = 1000,
                Discount = 1000,
                OrderRef = "1234567",
                Subtotal = 1000,
                Timestamp = DateTime.Now,
                Total = 10000,
                TotalQuantity = 100,
                UserId = admin.UserId
            };

            var result = _orderAppService.CreateOrder(order);
            var orderResult = UsingDbContext(ctx => ctx.Order.First());
            Assert.True(result);
            Assert.True(orderResult.Id > 0);
        }

        [Fact]
        public void CreateOrder_Throw_Exception_If_Orderref_Exists()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(GetFakeInstitution()));
            var adminEntity = GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            Order order = GetFakeOrder();
            order.UserId = admin.UserId;
            UsingDbContext(ctx => ctx.Order.Add(order));
            OrderDto createOrder = new OrderDto()
            {
                DeliveryCharge = 1000,
                Discount = 1000,
                OrderRef = "1234567",
                Subtotal = 1000,
                Timestamp = DateTime.Now,
                Total = 10000,
                TotalQuantity = 100,
                UserId = admin.UserId
            };

            UserFriendlyException ex = Should.Throw<UserFriendlyException>(
                () => _orderAppService.CreateOrder(createOrder)
            );

            Assert.True(ex != null);
        }
        #endregion

        [Fact]
        public void GetAll_Should_Should_Return_Correct_Number_Of_Records()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(GetFakeInstitution()));
            var adminEntity = GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            for (int i = 0; i < 10; i++)
            {
                Order order = GetFakeOrder();
                order.UserId = admin.UserId;
                order.OrderRef += i;
                UsingDbContext(ctx => ctx.Order.Add(order));
            }

            PaginationDataList<OrderDto> list = _orderAppService.GetAll(1, null);

            list.CurrentPage.ShouldBe(1);
            list.ListData.Count.ShouldBe(10);
            list.TotalPages.ShouldBe(1);

        }

    }
}
