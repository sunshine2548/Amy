using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Core.Entity
{
    public class Order : Abp.Domain.Entities.Entity<Int64>
    {
        public string OrderRef { get; set; }
        public Guid UserId { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }
        public DateTime Timestamp { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Payment> Payments { get; set; }

        public Order()
        {
            Timestamp = DateTime.Now;
            ShoppingCarts = new List<ShoppingCart>();
            OrderItems = new List<OrderItem>();
            Payments = new List<Payment>();
        }
    }
}
