using Imagine.BookManager.Core.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Imagine.BookManager.EntityFramework
{
    public class SetMap : EntityTypeConfiguration<Set>
    {
        public SetMap()
        {
            ToTable("Set");
            Property(s => s.SetName).IsRequired().HasMaxLength(50);
            Property(s => s.Synopsis).IsRequired().HasMaxLength(1000);
            Property(s => s.ImageUrl).IsRequired().HasMaxLength(100);
            Property(s => s.Price).IsRequired().HasPrecision(18, 2);

            HasMany(s => s.CartItems)
                .WithRequired()
                .HasForeignKey(c => c.SetId)
                .WillCascadeOnDelete(false);

            HasMany(s => s.OrderItems)
                .WithRequired()
                .HasForeignKey(o => o.SetId)
                .WillCascadeOnDelete(false);

            HasMany(s => s.Books)
                .WithRequired()
                .HasForeignKey(b => b.SetId)
                .WillCascadeOnDelete(false);

            HasMany(s => s.TeacherAllocations)
                .WithOptional()
                .HasForeignKey(s => s.SetId)
                .WillCascadeOnDelete(false);
        }
    }
}
