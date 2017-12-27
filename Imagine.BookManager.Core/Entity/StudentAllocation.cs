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
        private int UsefulTime { get; } = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["UsefulTime"]);
        public long TeacherAllocationId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public StudentAllocation()
        {
            StartDate = DateTime.Now;
            ExpiryDate = StartDate.AddMonths(UsefulTime);
        }
    }
}
