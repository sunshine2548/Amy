using Imagine.BookManager.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Core.Entity
{
    public class TeacherAllocation : Abp.Domain.Entities.Entity<Int64>
    {
        public long OrderItemId { get; set; }
        public int SetId { get; set; }
        public Guid TeacherId { get; set; }
        public int Credit { get; set; }
        public DateTime DateAllocated { get; set; }


        public ICollection<StudentAllocation> StudentAllocations { get; set; }
        public TeacherAllocation()
        {
            DateAllocated = DateTime.Now;
            StudentAllocations = new List<StudentAllocation>();
        }
    }
}
