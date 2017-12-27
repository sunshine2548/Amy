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

            HasRequired(e => e.AdminObj)
                .WithMany(e => e.TeacherAllocations)
                .HasForeignKey(e => e.TeacherId)
                .WillCascadeOnDelete(false);


            HasRequired(e => e.OrderItemObj)
                .WithMany(e => e.TeacherAllocations)
                .HasForeignKey(e => e.OrderItemId)
                .WillCascadeOnDelete(false);

            HasRequired(e => e.SetObj)
                .WithMany(e => e.TeacherAllocations)
                .HasForeignKey(e => e.SetId)
                .WillCascadeOnDelete(false);
        }
    }
}
