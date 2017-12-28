using Abp.Domain.Repositories;
using Imagine.BookManager.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Imagine.BookManager.Common;
using Imagine.BookManager.Dto.Order;

namespace Imagine.BookManager.ShoppingCartService
{
    public class ShoppingCartAppService : BookManagerAppServiceBase, IShoppingCartAppService
    {
        private readonly IRepository<ShoppingCart, Int64> _shoppingCartRepository;

        public IRepository<CartItem> CartItemRepository { get; set; }

        public IRepository<Order, Int64> OrderRepository { get; set; }

        public ShoppingCartAppService(IRepository<ShoppingCart, Int64> shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public OrderDto ConfirmShoppingCart(Int64 shoppingCartId)
        {
            var shoppingCart = _shoppingCartRepository
                .GetAllIncluding(x => x.CartItems)
                .FirstOrDefault(x => x.Id == shoppingCartId);
            if (shoppingCart == null || shoppingCart.Id == 0)
            {
                return null;
            }
            if (shoppingCart.CartItems.Count == 0)
                return null;
            Order order = ObjectMapper.Map<Order>(shoppingCart);
            var orderRef = Util.CreateOrderRef();
            order.OrderRef = orderRef;
            shoppingCart.OrderRef = orderRef;
            order.OrderItems = ObjectMapper.Map<ICollection<OrderItem>>(shoppingCart.CartItems);
            foreach (var item in order.OrderItems)
            {
                item.UserId = order.UserId;
                item.OrderRef = orderRef;
            }
            var resultOrder = OrderRepository.Insert(order);
            _shoppingCartRepository.Update(shoppingCart);
            return ObjectMapper.Map<OrderDto>(resultOrder);
        }

        public bool CreateShoppingCart(ShoppingCart shoppingCart)
        {
            return _shoppingCartRepository.InsertAndGetId(shoppingCart) > 0;
        }

        public ShoppingCart GetShoppingCartByUserId(Guid userId)
        {
            return _shoppingCartRepository
                .GetAllIncluding(x => x.CartItems)
                .FirstOrDefault(x => x.UserId == userId && string.IsNullOrEmpty(x.OrderRef));
        }

        public Task<ShoppingCart> GetShoppingCartByUerIdAsync(Guid userId)
        {
            return _shoppingCartRepository.GetAllIncluding(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId && string.IsNullOrEmpty(x.OrderRef));
        }

        public bool UpdateShoppingCart(CartItem cartItem, Guid userId)
        {
            ShoppingCart shoppingCart = GetShoppingCartByUserId(userId) ?? new ShoppingCart
            {
                UserId = userId
            };
            var cartItemTemp = shoppingCart.CartItems.FirstOrDefault(x => x.SetId == cartItem.SetId);
            if (cartItemTemp != null && cartItemTemp.Id > 0)
            {
                if (cartItem.Quantity == 0)
                {
                    shoppingCart.TotalQuantity -= cartItemTemp.Quantity;
                    shoppingCart.Total -= cartItemTemp.Price;
                    shoppingCart.Discount -= cartItemTemp.Discount;
                    CartItemRepository.Delete(cartItemTemp);
                    shoppingCart.CartItems.Remove(cartItemTemp);
                }
                else
                {
                    shoppingCart.Total += (cartItem.Price - cartItemTemp.Price);
                    shoppingCart.Discount += (cartItem.Discount - cartItemTemp.Discount);
                    shoppingCart.TotalQuantity += (cartItem.Quantity - cartItemTemp.Quantity);
                    cartItemTemp.Price = cartItem.Price;
                    cartItemTemp.Quantity = cartItem.Quantity;
                    cartItemTemp.Discount = cartItem.Discount;
                    cartItemTemp.Timestamp = DateTime.Now;
                }
            }
            else
            {
                shoppingCart.CartItems.Add(cartItem);
                shoppingCart.Total += cartItem.Price;
                shoppingCart.Discount += cartItem.Discount;
                shoppingCart.TotalQuantity += cartItem.Quantity;
            }
            return _shoppingCartRepository.InsertOrUpdate(shoppingCart) != null;
        }
    }
}
