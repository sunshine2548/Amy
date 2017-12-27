namespace Imagine.BookManager.PaymentService.AliPay
{
    public interface IAliPayService
    {
        string GetAliPayTradeOrderResponse(string orderRef, decimal payMoney);
    }
}