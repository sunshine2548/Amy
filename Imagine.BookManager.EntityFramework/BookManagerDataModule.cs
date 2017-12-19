using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using Imagine.BookManager.EntityFramework;

namespace Imagine.BookManager
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(BookManagerCoreModule))]
    public class BookManagerDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<BookManagerDbContext>(null);
        }
    }
}
