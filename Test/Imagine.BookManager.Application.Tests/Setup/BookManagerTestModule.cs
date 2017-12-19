using Abp.Modules;
using Abp.TestBase;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
