using System.Reflection;
using Abp.Modules;

namespace Imagine.BookManager.PaymentService.Tests.StepUp
{
    [DependsOn(typeof(BookManagerPaymentServiceModule))]
    public class BookManagerTestModule: AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}