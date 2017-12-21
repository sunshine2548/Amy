using System.Reflection;
using Abp.Modules;
using Imagine.BookManager.Dto;

namespace Imagine.BookManager.PaymentService
{
    [DependsOn(typeof(BookManagerDtoModule))]
    public class BookManagerPaymentServiceModule: AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}