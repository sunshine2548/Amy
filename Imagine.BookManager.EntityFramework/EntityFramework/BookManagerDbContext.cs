using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Abp.Domain.Entities;
using Abp.EntityFramework;
using Imagine.BookManager.Core.Entity;

namespace Imagine.BookManager.EntityFramework
{
    public class BookManagerDbContext : AbpDbContext
    {
        //TODO: Define an IDbSet for each Entity...

        //Example:
        //public virtual IDbSet<User> Users { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public BookManagerDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in BookManagerDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of BookManagerDbContext since ABP automatically handles it.
         */
        public BookManagerDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public BookManagerDbContext(DbConnection existingConnection)
         : base(existingConnection, false)
        {

        }

        public BookManagerDbContext(DbConnection existingConnection, bool contextOwnsConnection)
         : base(existingConnection, contextOwnsConnection)
        {

        }

        public virtual IDbSet<Admin> Admin { get; set; }
        public virtual IDbSet<Institution> Institution { get; set; }
        public virtual IDbSet<Order> Order { get; set; }
        public virtual IDbSet<ClassInfo> ClassInfo { get; set; }
        public virtual IDbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual IDbSet<CartItem> CarItem { get; set; }
        public virtual IDbSet<OrderItem> OrderItem { get; set; }
        public virtual IDbSet<Set> Sets { get; set; }
        public virtual IDbSet<Book> Book { get; set; }
        public virtual IDbSet<Payment> Payment { get; set; }
        public virtual IDbSet<Student> Student { get; set; }
        public virtual IDbSet<TeacherAllocation> TeacherAllocation { get; set; }
        public virtual IDbSet<StudentAllocation> StudentAllocation { get; set; }

        public DbEntityEntry<T> GetDbEntity<T>(T t) where T: Entity
        {
            return Entry(t);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new InstitutionMap());
            modelBuilder.Configurations.Add(new AdminMap());
            modelBuilder.Configurations.Add(new ClassInfoMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new ShoppingCartMap());
            modelBuilder.Configurations.Add(new OrderItemMap());
            modelBuilder.Configurations.Add(new SetMap());
            modelBuilder.Configurations.Add(new BookMap());
            modelBuilder.Configurations.Add(new PaymentMap());
            modelBuilder.Configurations.Add(new StudentMap());
            modelBuilder.Configurations.Add(new TeacherAllocationMap());
            modelBuilder.Configurations.Add(new StudentAllocationMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
