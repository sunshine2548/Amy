using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Core.Entity
{
    public class Student : Abp.Domain.Entities.Entity<Int64>
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Picture { get; set; }
        public string GuardianName { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public int? ClassId { get; set; }
        public bool IsDelete { get; set; }

        public Student()
        {
            DateCreated = DateTime.Now;
            IsDelete = false;
        }
    }
}
