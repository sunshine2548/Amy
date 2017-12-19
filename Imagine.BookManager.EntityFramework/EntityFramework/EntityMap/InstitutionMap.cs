using Imagine.BookManager.Core.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Imagine.BookManager.EntityFramework
{
    public class InstitutionMap : EntityTypeConfiguration<Institution>
    {
        public InstitutionMap()
        {
            ToTable("Institution");
            HasMany(i => i.Admins)
                .WithOptional()
                .HasForeignKey(a => a.InstitutionId)
                .WillCascadeOnDelete(false);

            Property(i => i.Name).HasMaxLength(100).IsRequired();
            Property(i => i.Tel).HasMaxLength(20);
            Property(i => i.Address).HasMaxLength(200);
            Property(i => i.District).HasMaxLength(100);
            HasIndex(i => i.Name).IsUnique();

            HasMany(i => i.ClassInfos)
                .WithOptional().HasForeignKey(c => c.InstitutionId);
        }
    }
}
