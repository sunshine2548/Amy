using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imagine.BookManager.Core.Entity
{
    public class Book : Abp.Domain.Entities.Entity
    {
        public string BookName { get; set; }
        public int SetId { get; set; }

    }
}
