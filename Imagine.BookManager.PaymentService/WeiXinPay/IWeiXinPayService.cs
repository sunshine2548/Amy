using Imagine.BookManager.Dto.PayMent;

namespace Imagine.BookManager.PaymentService.WeiXinPay
{
    public interface IWeiXinPayService: IPaymentService
    {
        WeiXinOrderResult GetWeiXinPayQrCode(string orderRef, decimal amount);
    }
}