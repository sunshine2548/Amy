using Imagine.BookManager.Core.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
namespace Imagine.BookManager.EntityFramework
{
    public class ShoppingCartMap : EntityTypeConfiguration<ShoppingCart>
    {
        public ShoppingCartMap()
        {
            ToTable("ShoppingCart");
            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasIndex(s => s.UserId);
            Property(o => o.Total).IsRequired().HasPrecision(18, 2);
            Property(o => o.Discount).IsRequired().HasPrecision(18, 2);
            Property(o => o.TotalQuantity).IsRequired();

            HasMany(s => s.CartItems)
                .WithRequired()
                .HasForeignKey(c => c.CartId)
                .WillCascadeOnDelete(false);

        }
    }
}
