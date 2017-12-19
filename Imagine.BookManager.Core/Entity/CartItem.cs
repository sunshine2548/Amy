using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imagine.BookManager.Core.Entity
{
    [Table("CartItem")]
    public class CartItem : Abp.Domain.Entities.Entity
    {
        public Int64 CartId { get; set; }
        public int SetId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public DateTime Timestamp { get; set; }
        public CartItem()
        {
            Timestamp = DateTime.Now;
        }
    }
}
