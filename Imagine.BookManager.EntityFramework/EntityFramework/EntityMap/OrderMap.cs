using Imagine.BookManager.Core.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
namespace Imagine.BookManager.EntityFramework
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            ToTable("Order");
            HasKey(x => x.OrderRef);
            Property(o => o.OrderRef).IsRequired().HasMaxLength(64);
            Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(o => o.Subtotal).IsRequired().HasPrecision(18, 2);
            Property(o => o.Total).IsRequired().HasPrecision(18, 2);
            Property(o => o.Discount).IsRequired().HasPrecision(18, 2);
            Property(o => o.TotalQuantity).IsRequired();
            Property(o => o.Paid).IsRequired();

            HasMany(o => o.ShoppingCarts)
                .WithOptional()
                .HasForeignKey(s => s.OrderRef)
                .WillCascadeOnDelete(false);

            HasMany(o => o.OrderItems)
                .WithRequired()
                .HasForeignKey(o => o.OrderRef)
                .WillCascadeOnDelete(false);

            HasMany(o => o.Payments)
                .WithRequired()
                .HasForeignKey(o => o.OrderRef)
                .WillCascadeOnDelete(false);
        }
    }
}
