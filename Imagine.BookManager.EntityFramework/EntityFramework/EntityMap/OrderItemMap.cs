using Imagine.BookManager.Core.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Imagine.BookManager.EntityFramework
{
    public class OrderItemMap : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemMap()
        {
            ToTable("OrderItem");
            Property(o => o.Price).IsRequired().HasPrecision(18, 2);
            Property(o => o.Quantity).IsRequired();
            Property(o => o.RemainCredit).IsRequired();
        }
    }
}
