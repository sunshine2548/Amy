using System.Collections.Generic;
namespace Imagine.BookManager.Core.Entity
{
    public class Set : Abp.Domain.Entities.Entity
    {
        public string SetName { get; set; }
        public string Synopsis { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<TeacherAllocation> TeacherAllocations { get; set; }

        public Set()
        {
            CartItems = new List<CartItem>();
            OrderItems = new List<OrderItem>();
            Books = new List<Book>();
            TeacherAllocations = new List<TeacherAllocation>();
        }
    }

    public enum SetAllotStatus
    {
        /// <summary>
        /// 已分配
        /// </summary>
        Allocated = 1,
        /// <summary>
        /// 未分配
        /// </summary>
        UnAllocated = 2,
        /// <summary>
        /// 额度不足
        /// </summary>
        CreditInadequate = 3
    }
}
