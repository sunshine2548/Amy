using Imagine.BookManager.Core.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
namespace Imagine.BookManager.EntityFramework
{
    public class StudentMap : EntityTypeConfiguration<Student>
    {
        public StudentMap()
        {
            ToTable("Student");
            HasKey(s => s.StudentId);
            Property(s => s.StudentId)
                            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(s => s.Password).IsRequired().HasMaxLength(23);
            Property(s => s.UserName).IsRequired().HasMaxLength(50);
            Property(s => s.GuardianName).HasMaxLength(50);
            Property(s => s.FullName).HasMaxLength(50);
            Property(s => s.Mobile).HasMaxLength(12);
            Property(s => s.Picture).HasMaxLength(50);
            HasIndex(s => s.UserName).IsUnique(true);

            //HasMany(e => e.StudentAllocations)
            //    .WithOptional()
            //    .HasForeignKey(e=>e.StudentId)
            //    .WillCascadeOnDelete(false);
        }
    }
}
