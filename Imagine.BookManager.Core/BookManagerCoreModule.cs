using System.Reflection;
using Abp.Modules;

namespace Imagine.BookManager
{
    public class BookManagerCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
