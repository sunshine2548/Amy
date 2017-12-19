using Imagine.BookManager.Core.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Imagine.BookManager.EntityFramework
{
    public class ClassInfoMap : EntityTypeConfiguration<ClassInfo>
    {
        public ClassInfoMap()
        {
            ToTable("Class");
            Property(c => c.Name).IsRequired().HasMaxLength(50);
            HasMany(c => c.Students)
                .WithOptional()
                .HasForeignKey(c => c.ClassId)
                .WillCascadeOnDelete(false);
        }
    }
}
