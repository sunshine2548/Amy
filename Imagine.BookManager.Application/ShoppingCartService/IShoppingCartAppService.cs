using Abp.Application.Services;
using Imagine.BookManager.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagine.BookManager.Dto.Order;

namespace Imagine.BookManager.ShoppingCartService
{
    public interface IShoppingCartAppService: IApplicationService
    {
        bool CreateShoppingCart(ShoppingCart shoppingCart);

        bool UpdateShoppingCart(CartItem cartItem,Guid userId);
        OrderDto ConfirmShoppingCart(Int64 shoppingCartId);
        ShoppingCart GetShoppingCartByUserId(Guid userId);
        Task<ShoppingCart> GetShoppingCartByUerIdAsync(Guid userId);
    }
}
