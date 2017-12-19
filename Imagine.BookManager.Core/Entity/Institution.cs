using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Core.Entity
{
    public class Institution : Abp.Domain.Entities.Entity
    {
        public string Name { get; set; }
        public string Tel { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public ICollection<Admin> Admins { get; set; }
        public ICollection<ClassInfo> ClassInfos { get; set; }
        public Institution()
        {
            Admins = new List<Admin>();
            ClassInfos = new List<ClassInfo>();
        }
    }
}
