using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
namespace Imagine.BookManager.Core.Entity
{
    public class ShoppingCart : Entity<Int64>
    {
        public string OrderRef { get; set; }
        public Guid UserId { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public DateTime Timestamp { get; set; }
        public ICollection<CartItem> CartItems { get; set; }

        public ShoppingCart()
        {
            Timestamp = DateTime.Now;
            CartItems = new List<CartItem>();
        }

    }
}
