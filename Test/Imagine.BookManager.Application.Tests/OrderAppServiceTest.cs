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
        #region CreateOrder
        [Fact]
        public void CreateOrder_Return_Ture_If_Success()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
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
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            Order order = InitFakeEntity.GetFakeOrder();
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
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            for (int i = 0; i < 10; i++)
            {
                Order order = InitFakeEntity.GetFakeOrder();
                order.UserId = admin.UserId;
                order.OrderRef += i;
                UsingDbContext(ctx => ctx.Order.Add(order));
            }

            PaginationDataList<OrderDto> list = _orderAppService.GetAll(1, null);

            list.CurrentPage.ShouldBe(1);
            list.ListData.Count.ShouldBe(10);
            list.TotalPages.ShouldBe(1);
        }

        [Fact]
        public void GetOrderByOrderRef_Should_Return_Correct_Number_Of_Records()
        {
            Admin admin = UsingDbContext(ctx => ctx.Admin.Add(InitFakeEntity.GetFakeAdmin()));
            Order order = InitFakeEntity.GetFakeOrder();
            order.OrderRef = Util.CreateOrderRef();
            order.UserId = admin.UserId;
            var reult = UsingDbContext(ctx => ctx.Order.Add(order));
            OrderDto orderDto = _orderAppService.GetOrderByOrderRef(order.OrderRef);
            orderDto.OrderRef.ShouldBe(order.OrderRef);
        }

        [Fact]
        public void GetOrderByOrderRef_Should_Return_Null_If_OrderRef_Not_Exists()
        {
            OrderDto orderDto = _orderAppService.GetOrderByOrderRef("112222");
            orderDto.ShouldBe(null);
        }

        [Fact]
        public void UpdateOrderPaid_Return_Ture_If_Success()
        {
            Admin admin = UsingDbContext(ctx => ctx.Admin.Add(InitFakeEntity.GetFakeAdmin()));
            Order order = InitFakeEntity.GetFakeOrder();
            order.OrderRef = Util.CreateOrderRef();
            order.UserId = admin.UserId;
            UsingDbContext(ctx => ctx.Order.Add(order));
            bool result = _orderAppService.UpdateOrderPaid(order.OrderRef, true);

            Order orderResult = UsingDbContext(ctx => ctx.Order.FirstOrDefault(x => x.OrderRef == order.OrderRef));
            Assert.True(result);
            Assert.True(orderResult.Paid);
        }

        [Fact]
        public void UpdateOrderPaid_Return_False_If_Order_Not_Found()
        {
            bool result = _orderAppService.UpdateOrderPaid("1231123", true);
            Assert.False(result);
        }

        [Fact]
        public void GetUserOrders_Should_Return_Correct_Number_Of_Records()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            for (int i = 0; i < 10; i++)
            {
                Order order = InitFakeEntity.GetFakeOrder();
                order.UserId = admin.UserId;
                order.OrderRef += i;
                UsingDbContext(ctx => ctx.Order.Add(order));
            }

            var result = _orderAppService.GetUserOrders(admin.UserId, 1, null);

            result.CurrentPage.ShouldBe(1);
            result.TotalPages.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
        }


        [Fact]
        public void GetUserOrders_Should_Return_Correct_Number_Of_Records_Page_Not_Null()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            for (int i = 0; i < 10; i++)
            {
                Order order = InitFakeEntity.GetFakeOrder();
                order.UserId = admin.UserId;
                order.OrderRef += i;
                UsingDbContext(ctx => ctx.Order.Add(order));
            }
            var result = _orderAppService.GetUserOrders(admin.UserId, 1, 10);
            result.CurrentPage.ShouldBe(1);
            result.TotalPages.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
        }

        [Fact]
        public void GetUserOrders_Should_Return_Correct_Number_Of_Records_Index_is_Null()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            for (int i = 0; i < 10; i++)
            {
                Order order = InitFakeEntity.GetFakeOrder();
                order.UserId = admin.UserId;
                order.OrderRef += i;
                UsingDbContext(ctx => ctx.Order.Add(order));
            }

            var result = _orderAppService.GetUserOrders(admin.UserId, null, 10);

            result.CurrentPage.ShouldBe(1);
            result.TotalPages.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
        }

        [Fact]
        public void GetUserOrders_Should_Return_Correct_Number_Of_Records_Index_And_Page_is_Null()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            for (int i = 0; i < 10; i++)
            {
                Order order = InitFakeEntity.GetFakeOrder();
                order.UserId = admin.UserId;
                order.OrderRef += i;
                UsingDbContext(ctx => ctx.Order.Add(order));
            }

            var result = _orderAppService.GetUserOrders(admin.UserId, null, null);

            result.CurrentPage.ShouldBe(1);
            result.TotalPages.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
        }

        [Fact]
        public void GetInstitutionOrderList_Should_Return_Correct_Number_Of_Records()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            for (int i = 0; i < 10; i++)
            {
                Order order = InitFakeEntity.GetFakeOrder();
                order.UserId = admin.UserId;
                order.OrderRef += i;
                UsingDbContext(ctx => ctx.Order.Add(order));
            }

            var result = _orderAppService.GetInstitutionOrderList(institution.Id, 1, null);

            result.CurrentPage.ShouldBe(1);
            result.TotalPages.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
        }


        [Fact]
        public void GetInstitutionOrderList_Should_Return_Correct_Number_Of_Records_Page_Not_Null()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            for (int i = 0; i < 10; i++)
            {
                Order order = InitFakeEntity.GetFakeOrder();
                order.UserId = admin.UserId;
                order.OrderRef += i;
                UsingDbContext(ctx => ctx.Order.Add(order));
            }
            var result = _orderAppService.GetInstitutionOrderList(institution.Id, 1, 10);
            result.CurrentPage.ShouldBe(1);
            result.TotalPages.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
        }

        [Fact]
        public void GetInstitutionOrderList_Should_Return_Correct_Number_Of_Records_Index_is_Null()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            for (int i = 0; i < 10; i++)
            {
                Order order = InitFakeEntity.GetFakeOrder();
                order.UserId = admin.UserId;
                order.OrderRef += i;
                UsingDbContext(ctx => ctx.Order.Add(order));
            }

            var result = _orderAppService.GetInstitutionOrderList(institution.Id, null, 10);

            result.CurrentPage.ShouldBe(1);
            result.TotalPages.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
        }

        [Fact]
        public void GetInstitutionOrderList_Should_Return_Correct_Number_Of_Records_Index_And_Page_is_Null()
        {
            var institution = UsingDbContext(ctx => ctx.Institution.Add(InitFakeEntity.GetFakeInstitution()));
            var adminEntity = InitFakeEntity.GetFakeAdmin();
            adminEntity.InstitutionId = institution.Id;
            var admin = UsingDbContext(ctx => ctx.Admin.Add(adminEntity));
            for (int i = 0; i < 10; i++)
            {
                Order order = InitFakeEntity.GetFakeOrder();
                order.UserId = admin.UserId;
                order.OrderRef += i;
                UsingDbContext(ctx => ctx.Order.Add(order));
            }

            var result = _orderAppService.GetInstitutionOrderList(institution.Id, null, null);

            result.CurrentPage.ShouldBe(1);
            result.TotalPages.ShouldBe(1);
            result.ListData.Count.ShouldBe(10);
        }
    }
}
