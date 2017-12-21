using Imagine.BookManager.Dto.PayMent;

namespace Imagine.BookManager.PaymentService.WeiXinPay
{
    public interface IWeiXinPayService
    {
        WeiXinOrderResult GetWeiXinPayQrCode(string orderRef, decimal payMoney);

        PayResult GetOrderPaidStatus(string xmlParamter);
    }
}