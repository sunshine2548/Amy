using Imagine.BookManager.Dto.PayMent;

namespace Imagine.BookManager.PaymentService
{
    public interface IPaymentService
    {
        PayResult GetOrderPaidStatus(string xmlParamter);

    }
}