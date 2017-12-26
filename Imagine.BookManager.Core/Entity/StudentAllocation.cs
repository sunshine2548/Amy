using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Core.Entity
{
    public class StudentAllocation : Entity<Int64>
    {
        public long TeacherAllocationId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime StartDate { get; set; }

        //public TeacherAllocation TeacherAllocationInfo { get; set; }
        //public Student StudentInfo { get; set; }

        public StudentAllocation()
        {
            StartDate = DateTime.Now;
            //TeacherAllocationInfo = new TeacherAllocation();
            //StudentInfo = new Student();
        }
    }
}
