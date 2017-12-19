using Abp.TestBase;
using Castle.MicroKernel.Registration;
using Effort;
using Imagine.BookManager.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagine.BookManager.Migrations;

namespace Imagine.BookManager.Application.Tests
{
    public abstract class BookManagerTestBase : AbpIntegratedTestBase<BookManagerTestModule>
    {
        private DbConnection _hostDb;

        protected BookManagerTestBase()
        {
            UsingDbContext(context =>
            {
                new InitDbContext(context).Create();
            });
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();
            UseSingleDatabase();
        }


        private void UseSingleDatabase()
        {
            _hostDb = DbConnectionFactory.CreateTransient();

            LocalIocManager.IocContainer
                .Register(Component.For<DbConnection>().UsingFactoryMethod(() => _hostDb).LifestyleSingleton());
        }

        protected void UsingDbContext(Action<BookManagerDbContext> action)
        {
            using (var context = LocalIocManager.Resolve<BookManagerDbContext>())
            {
                action(context);
                context.SaveChanges();
            }
        }

        protected async Task UsingDbContextAsync(Func<BookManagerDbContext, Task> action)
        {
            using (var context = LocalIocManager.Resolve<BookManagerDbContext>())
            {
                await action(context);
                await context.SaveChangesAsync();
            }
        }

        protected T UsingDbContext<T>(Func<BookManagerDbContext, T> func)
        {
            T result;
            using (var context = LocalIocManager.Resolve<BookManagerDbContext>())
            {
                result = func(context);
                context.SaveChanges();
            }
            return result;
        }

        protected async Task<T> UsingDbContextAsync<T>(Func<BookManagerDbContext, Task<T>> func)
        {
            T result;
            using (var context = LocalIocManager.Resolve<BookManagerDbContext>())
            {
                result = await func(context);
                await context.SaveChangesAsync();
            }
            return result;
        }
    }
}
