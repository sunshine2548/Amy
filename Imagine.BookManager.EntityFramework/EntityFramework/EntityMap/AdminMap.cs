using Imagine.BookManager.Core.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imagine.BookManager.EntityFramework
{
    public class AdminMap : EntityTypeConfiguration<Admin>
    {
        public AdminMap()
        {
            ToTable("Admin");
            Property(a => a.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(a => a.UserId);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(a => a.FullName).HasMaxLength(30);
            Property(a => a.Email).HasMaxLength(50);
            Property(a => a.Password)
                .IsRequired()
                .HasMaxLength(23);

            Property(a => a.UserName)
                .IsRequired()
                .HasMaxLength(30);

            Property(a => a.Mobile)
                .HasMaxLength(12);

            Property(a => a.UserType)
                .IsRequired();

            Property(a => a.Gender)
                .IsRequired();

            HasMany(i => i.Orders)
                .WithRequired()
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false);

            HasMany(a => a.ShoppingCarts)
                .WithRequired()
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            HasMany(a => a.Classes).WithMany(c => c.Admins).Map(m =>
            {
                m.MapLeftKey("TeacherId");
                m.MapRightKey("ClassId");
                m.ToTable("TeachClass");
            });
        }
    }
}
