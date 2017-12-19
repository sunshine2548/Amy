using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagine.BookManager.Core.Entity;
using Imagine.BookManager.Dto.Order;
using Imagine.BookManager.ShoppingCartService;
using Shouldly;
using Xunit;

namespace Imagine.BookManager.Application.Tests
{
    public class ShoppingCartAppServiceTest : BookManagerTestBase
    {
        private readonly IShoppingCartAppService _shoppingCartAppService;

        public ShoppingCartAppServiceTest()
        {
            _shoppingCartAppService = Resolve<IShoppingCartAppService>();
        }

        [Fact]
        public void CreateShoppingCart_Return_true_If_Success()
        {
            //Arrange
            var userId = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.InstitutionId.HasValue)).UserId;
            var count1 = UsingDbContext(ctx => ctx.ShoppingCart.Count());
            var shoppingCart = InitFakeEntity.GetFakeShoppingCart();
            shoppingCart.UserId = userId;

            //Act
            var result = _shoppingCartAppService.CreateShoppingCart(shoppingCart);

            //Asset
            var count2 = UsingDbContext(ctx => ctx.ShoppingCart.Count());
            count2.ShouldBe(count1 + 1);
            result.ShouldBe(true);
        }

        [Fact]
        public void GetShoppingCartByUserId_Should_Return_Correct_Number_Of_Records()
        {
            var set1 = InitFakeEntity.GetFakeSet();
            Set setResult1 = UsingDbContext(ctx => ctx.Sets.Add(set1));
            var set2 = InitFakeEntity.GetFakeSet();
            Set setResult2 = UsingDbContext(ctx => ctx.Sets.Add(set2));

            var userId = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.InstitutionId.HasValue)).UserId;
            var shoppingCart = InitFakeEntity.GetFakeShoppingCart();
            shoppingCart.UserId = userId;
            CartItem cartItem = InitFakeEntity.GetFakeCartItem(setId: setResult1.Id, quantity: 2);
            CartItem cartItem2 = InitFakeEntity.GetFakeCartItem(setId: setResult2.Id, quantity: 3);
            shoppingCart.Discount = cartItem2.Discount + cartItem.Discount;
            shoppingCart.TotalQuantity = 5;
            shoppingCart.Total = cartItem2.Price + cartItem.Price;
            shoppingCart.CartItems.Add(cartItem2);
            shoppingCart.CartItems.Add(cartItem);
            var shopping = UsingDbContext(ctx => ctx.ShoppingCart.Add(shoppingCart));

            ShoppingCart shoppingCartGet = _shoppingCartAppService.GetShoppingCartByUserId(userId);

            Assert.True(shoppingCartGet.Id > 0);
            Assert.True(shoppingCartGet.CartItems.Count > 0);
        }

        [Fact]
        public async void GetShoppingCartByUserIdTask_Should_Return_Correct_Number_Of_Records()
        {
            var set1 = InitFakeEntity.GetFakeSet();
            Set setResult1 = UsingDbContext(ctx => ctx.Sets.Add(set1));
            var set2 = InitFakeEntity.GetFakeSet();
            Set setResult2 = UsingDbContext(ctx => ctx.Sets.Add(set2));

            var userId = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.InstitutionId.HasValue)).UserId;
            var shoppingCart = InitFakeEntity.GetFakeShoppingCart();
            shoppingCart.UserId = userId;
            CartItem cartItem = InitFakeEntity.GetFakeCartItem(setId: setResult1.Id, quantity: 2);
            CartItem cartItem2 = InitFakeEntity.GetFakeCartItem(setId: setResult2.Id, quantity: 3);
            shoppingCart.Discount = cartItem2.Discount + cartItem.Discount;
            shoppingCart.TotalQuantity = 5;
            shoppingCart.Total = cartItem2.Price + cartItem.Price;
            shoppingCart.CartItems.Add(cartItem2);
            shoppingCart.CartItems.Add(cartItem);
            var shopping = UsingDbContext(ctx => ctx.ShoppingCart.Add(shoppingCart));
            ShoppingCart shoppingCartGet = await _shoppingCartAppService.GetShoppingCartByUerIdAsync(userId);
            Assert.True(shoppingCartGet.Id > 0);
            Assert.True(shoppingCartGet.CartItems.Count > 0);
        }

        [Fact]
        public void UpdateShoppingCart_Return_True_Add_CartItem_Where_SetId_Not_In_ShoppingCart()
        {
            var set1 = InitFakeEntity.GetFakeSet();
            Set setResult1 = UsingDbContext(ctx => ctx.Sets.Add(set1));
            var set2 = InitFakeEntity.GetFakeSet();
            Set setResult2 = UsingDbContext(ctx => ctx.Sets.Add(set2));

            var userId = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.InstitutionId.HasValue)).UserId;
            var shoppingCart = InitFakeEntity.GetFakeShoppingCart();
            shoppingCart.UserId = userId;
            CartItem cartItem = InitFakeEntity.GetFakeCartItem(setId: setResult1.Id, quantity: 2);
            CartItem cartItem2 = InitFakeEntity.GetFakeCartItem(setId: setResult2.Id, quantity: 3);
            shoppingCart.Discount = cartItem.Discount;
            shoppingCart.TotalQuantity = 2;
            shoppingCart.Total = cartItem.Price;
            shoppingCart.CartItems.Add(cartItem);
            UsingDbContext((ctx) => ctx.ShoppingCart.Add(shoppingCart));

            var result = _shoppingCartAppService.UpdateShoppingCart(cartItem2, userId);
            var shoppingCart2 = UsingDbContext(ctx => ctx.ShoppingCart.Include(s => s.CartItems).First());

            Assert.True(result);
            shoppingCart2.TotalQuantity.ShouldBe(5);
            shoppingCart2.Total.ShouldBe(cartItem.Price + cartItem2.Price);
            shoppingCart2.Discount.ShouldBe(cartItem.Discount + cartItem2.Discount);
            shoppingCart2.CartItems.Count.ShouldBe(2);
        }

        [Fact]
        public void UpdateShoppingCart_Return_True_Add_CartItem_Where_SetId_In_ShoppingCart()
        {
            var set1 = InitFakeEntity.GetFakeSet();
            Set setResult1 = UsingDbContext(ctx => ctx.Sets.Add(set1));
            var userId = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.InstitutionId.HasValue)).UserId;
            var shoppingCart = InitFakeEntity.GetFakeShoppingCart();
            shoppingCart.UserId = userId;
            CartItem cartItem = InitFakeEntity.GetFakeCartItem(setId: setResult1.Id, quantity: 2);
            CartItem cartItem2 = InitFakeEntity.GetFakeCartItem(setId: setResult1.Id, quantity: 3, price: 200, discount: 180);
            shoppingCart.Discount = cartItem.Discount;
            shoppingCart.TotalQuantity = 2;
            shoppingCart.Total = cartItem.Price;
            shoppingCart.CartItems.Add(cartItem);
            UsingDbContext((ctx) => ctx.ShoppingCart.Add(shoppingCart));

            var result = _shoppingCartAppService.UpdateShoppingCart(cartItem2, userId);
            var shoppingCart2 = UsingDbContext(ctx => ctx.ShoppingCart.Include(s => s.CartItems).First());

            Assert.True(result);
            shoppingCart2.TotalQuantity.ShouldBe(3);
            shoppingCart2.Total.ShouldBe(cartItem2.Price);
            shoppingCart2.Discount.ShouldBe(cartItem2.Discount);
            shoppingCart2.CartItems.Count.ShouldBe(1);
        }

        [Fact]
        public void UpdateShoppingCart_Return_True_Sub_CartItem_Where_SetId_In_ShoppingCart()
        {
            var set1 = InitFakeEntity.GetFakeSet();
            Set setResult1 = UsingDbContext(ctx => ctx.Sets.Add(set1));
            var userId = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.InstitutionId.HasValue)).UserId;
            var shoppingCart = InitFakeEntity.GetFakeShoppingCart();
            shoppingCart.UserId = userId;
            CartItem cartItem = InitFakeEntity.GetFakeCartItem(setId: setResult1.Id, quantity: 5);
            CartItem cartItem2 = InitFakeEntity.GetFakeCartItem(setId: setResult1.Id, quantity: 3);
            shoppingCart.Discount = cartItem.Discount;
            shoppingCart.TotalQuantity = 5;
            shoppingCart.Total = cartItem.Price;
            shoppingCart.CartItems.Add(cartItem);
            UsingDbContext((ctx) => ctx.ShoppingCart.Add(shoppingCart));

            var result = _shoppingCartAppService.UpdateShoppingCart(cartItem2, userId);
            var shoppingCart2 = UsingDbContext(ctx => ctx.ShoppingCart.Include(s => s.CartItems).First());

            Assert.True(result);
            shoppingCart2.TotalQuantity.ShouldBe(3);
            shoppingCart2.Total.ShouldBe(cartItem2.Price);
            shoppingCart2.Discount.ShouldBe(cartItem2.Discount);
            shoppingCart2.CartItems.Count.ShouldBe(1);
        }

        [Fact]
        public void UpdateShoppingCart_Return_True_Remove_CartItem_Where_SetId_In_ShoppingCart()
        {
            var set1 = InitFakeEntity.GetFakeSet();
            Set setResult1 = UsingDbContext(ctx => ctx.Sets.Add(set1));
            var userId = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.InstitutionId.HasValue)).UserId;
            var shoppingCart = InitFakeEntity.GetFakeShoppingCart();
            shoppingCart.UserId = userId;
            CartItem cartItem = InitFakeEntity.GetFakeCartItem(setId: setResult1.Id, quantity: 5);
            CartItem cartItem2 = InitFakeEntity.GetFakeCartItem(setId: setResult1.Id, quantity: 0);
            shoppingCart.Discount = cartItem.Discount;
            shoppingCart.TotalQuantity = 5;
            shoppingCart.Total = cartItem.Price;
            shoppingCart.CartItems.Add(cartItem);
            UsingDbContext((ctx) => ctx.ShoppingCart.Add(shoppingCart));

            var result = _shoppingCartAppService.UpdateShoppingCart(cartItem2, userId);
            var shoppingCart2 = UsingDbContext(ctx => ctx.ShoppingCart.Include(s => s.CartItems).First());

            Assert.True(result);
            shoppingCart2.TotalQuantity.ShouldBe(0);
            shoppingCart2.Total.ShouldBe(0);
            shoppingCart2.Discount.ShouldBe(0);
            shoppingCart2.CartItems.Count.ShouldBe(0);
        }

        [Fact]
        public void UpdateShoppingCart_Return_True_Add_CartItem_Where_ShoppingCart_Is_Empty()
        {
            var set1 = InitFakeEntity.GetFakeSet();
            Set setResult1 = UsingDbContext(ctx => ctx.Sets.Add(set1));
            var userId = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.InstitutionId.HasValue)).UserId;
            CartItem cartItem = InitFakeEntity.GetFakeCartItem(setId: setResult1.Id, quantity: 5);
            var result = _shoppingCartAppService.UpdateShoppingCart(cartItem, userId);
            var shoppingCart2 = UsingDbContext(ctx => ctx.ShoppingCart.Include(s => s.CartItems).First());

            Assert.True(result);
            shoppingCart2.TotalQuantity.ShouldBe(cartItem.Quantity);
            shoppingCart2.Total.ShouldBe(cartItem.Price);
            shoppingCart2.Discount.ShouldBe(cartItem.Discount);
            shoppingCart2.CartItems.Count.ShouldBe(1);
        }

        [Fact]
        public void ConfirmShoppingCart_Return_Null_If_Not_Found()
        {
            var result = _shoppingCartAppService.ConfirmShoppingCart(123);
            result.ShouldBe(null);
        }

        [Fact]
        public void ConfirmShoppingCart_Should_Return_Correct_Number_Of_Records()
        {
            var set1 = InitFakeEntity.GetFakeSet();
            Set setResult1 = UsingDbContext(ctx => ctx.Sets.Add(set1));
            var set2 = InitFakeEntity.GetFakeSet();
            Set setResult2 = UsingDbContext(ctx => ctx.Sets.Add(set2));

            var userId = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.InstitutionId.HasValue)).UserId;
            var shoppingCart = InitFakeEntity.GetFakeShoppingCart();
            shoppingCart.UserId = userId;
            CartItem cartItem = InitFakeEntity.GetFakeCartItem(setId: setResult1.Id, quantity: 2);
            CartItem cartItem2 = InitFakeEntity.GetFakeCartItem(setId: setResult2.Id, quantity: 3);
            shoppingCart.Discount = cartItem2.Discount + cartItem.Discount;
            shoppingCart.TotalQuantity = 5;
            shoppingCart.Total = cartItem2.Price + cartItem.Price;
            shoppingCart.CartItems.Add(cartItem2);
            shoppingCart.CartItems.Add(cartItem);
            var shopping = UsingDbContext(ctx => ctx.ShoppingCart.Add(shoppingCart));

            OrderDto order = _shoppingCartAppService.ConfirmShoppingCart(shopping.Id);
            Assert.True(order.Id > 0);
            Assert.True(order.OrderItems.Count == shoppingCart.CartItems.Count);
        }

        [Fact]
        public void ConfirmShoppingCart_Should_Return_Null_If_CartItem_Is_Null()
        {
            var userId = UsingDbContext(ctx => ctx.Admin.FirstOrDefault(x => x.InstitutionId.HasValue)).UserId;
            var shoppingCart = InitFakeEntity.GetFakeShoppingCart();
            shoppingCart.UserId = userId;

            var shopping = UsingDbContext(ctx => ctx.ShoppingCart.Add(shoppingCart));

            OrderDto order = _shoppingCartAppService.ConfirmShoppingCart(shopping.Id);
            Assert.True(order == null);
        }
    }
}
