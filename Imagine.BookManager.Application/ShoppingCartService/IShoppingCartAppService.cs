using Abp.Application.Services;
using Imagine.BookManager.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagine.BookManager.Dto.Order;
using System.Web.Http;

namespace Imagine.BookManager.ShoppingCartService
{
    public interface IShoppingCartAppService: IApplicationService
    {
        [HttpPost]
        bool CreateShoppingCart(ShoppingCart shoppingCart);

        [HttpPost]
        bool UpdateShoppingCart(CartItem cartItem,Guid userId);
        [HttpGet]
        OrderDto ConfirmShoppingCart(Int64 shoppingCartId);
        [HttpGet]
        ShoppingCart GetShoppingCartByUserId(Guid userId);
        [HttpGet]
        Task<ShoppingCart> GetShoppingCartByUerIdAsync(Guid userId);
    }
}
