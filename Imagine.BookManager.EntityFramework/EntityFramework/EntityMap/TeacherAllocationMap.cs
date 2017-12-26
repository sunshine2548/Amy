using Imagine.BookManager.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.EntityFramework
{
    public class TeacherAllocationMap : EntityTypeConfiguration<TeacherAllocation>
    {
        public TeacherAllocationMap()
        {
            ToTable("TeacherAllocation");
            Property(e => e.OrderItemId)
                .IsRequired();

            Property(e => e.SetId)
                .IsRequired();

            Property(e => e.TeacherId)
                .IsRequired();

            Property(e => e.Credit)
                .IsRequired();

            //HasMany(e => e.StudentAllocations)
            //    .WithRequired()
            //    .HasForeignKey(e=>e.TeacherAllocationId)
            //    .WillCascadeOnDelete(false);
        }
    }
}
