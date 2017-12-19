using System.Data.Entity.Migrations;

namespace Imagine.BookManager.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<BookManager.EntityFramework.BookManagerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BookManager";
        }

        protected override void Seed(BookManager.EntityFramework.BookManagerDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
        }
    }
}
