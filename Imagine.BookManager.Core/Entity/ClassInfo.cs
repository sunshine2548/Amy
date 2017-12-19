using System;
using System.Collections.Generic;

namespace Imagine.BookManager.Core.Entity
{
    public sealed class ClassInfo : Abp.Domain.Entities.Entity
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int? ReminderInterva { get; set; }
        public List<Admin> Admins { get; set; }
        public int InstitutionId { get; set; }
        public ICollection<Student> Students { get; set; }

        public ClassInfo()
        {
            DateCreated = DateTime.Now;
            Admins = new List<Admin>();
            Students = new List<Student>();
        }

    }
}
