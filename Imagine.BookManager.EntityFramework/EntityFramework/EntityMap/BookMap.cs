using Imagine.BookManager.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.EntityFramework
{
    public class BookMap : EntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            ToTable("Book");
            Property(e => e.BookName).IsRequired().HasMaxLength(100);
        }
    }
}
