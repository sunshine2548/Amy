using Imagine.BookManager.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.EntityFramework
{
    public class StudentAllocationMap : EntityTypeConfiguration<StudentAllocation>
    {
        public StudentAllocationMap()
        {
            ToTable("StudentAllocation");
            //Property(e => e.TeacherAllocationId)
            //    .IsRequired();
            


           
        }
    }
}
