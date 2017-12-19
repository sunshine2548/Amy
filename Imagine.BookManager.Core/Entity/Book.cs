using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imagine.BookManager.Core.Entity
{
    [Table("Book")]
    public class Book : Abp.Domain.Entities.Entity
    {
        [Required]
        [MaxLength(100)]
        public string BookName { get; set; }
        public int SetId { get; set; }

    }
}
