using System.Reflection;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;

namespace Imagine.BookManager
{
    [DependsOn(typeof(AbpWebApiModule), typeof(BookManagerApplicationModule))]
    public class BookManagerWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(BookManagerApplicationModule).Assembly, "app")
                .Build();
        }
    }
}
