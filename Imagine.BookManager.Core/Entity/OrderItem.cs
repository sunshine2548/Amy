using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Core.Entity
{
    public class OrderItem: Abp.Domain.Entities.Entity<Int64>
    {
        public string OrderRef { get; set; }
        public int SetId { get; set; }
        public virtual Set Set { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public int RemainCredit { get; set; }
        public Guid UserId { get; set; }

    }
}
