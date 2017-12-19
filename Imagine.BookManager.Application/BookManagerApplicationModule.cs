using Abp.AutoMapper;
using Abp.Modules;
using Imagine.BookManager.Dto;
using System.Reflection;

namespace Imagine.BookManager
{
    [DependsOn(typeof(BookManagerDtoModule), typeof(AbpAutoMapperModule))]
    public class BookManagerApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
