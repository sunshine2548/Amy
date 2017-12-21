using Abp.Modules;
using Abp.TestBase;

namespace Imagine.BookManager.Application.Tests
{
    [DependsOn(typeof(BookManagerApplicationModule),typeof(BookManagerDataModule),typeof(AbpTestBaseModule))]
    public class BookManagerTestModule: AbpModule
    {
        public override void PreInitialize()
        {
            base.PreInitialize();
        }
    }
}
