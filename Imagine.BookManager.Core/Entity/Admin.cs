using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Core.Entity
{
    public class Admin : Abp.Domain.Entities.Entity
    {
        public Guid UserId { get; set; }
        public int? InstitutionId { get; set; }
        public string FullName { get; set; }
        public UserType UserType { get; set; }
        public bool Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDelete { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public List<ClassInfo> Classes { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public ICollection<TeacherAllocation> TeacherAllocations { get; set; }
        public Admin()
        {
            DateCreated = DateTime.Now;
            Classes = new List<ClassInfo>();
            Orders = new List<Order>();
            ShoppingCarts = new List<ShoppingCart>();
            TeacherAllocations = new List<TeacherAllocation>();
        }
    }

    public enum UserType
    {
        Admin = 1,
        Teacher = 2
    }
}
