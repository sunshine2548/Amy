using Abp.Application.Services;
using Imagine.BookManager.Dto.PayMent;

namespace Imagine.BookManager.PayMentService
{
    public interface IPaymentAppService : IApplicationService
    {
        WeiXinOrderResult WeiXinConfigOrder(string orderRef);

        bool QueryOrderStutas(string orderRef);
    }
}