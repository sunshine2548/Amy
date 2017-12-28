namespace Imagine.BookManager.PaymentService.AliPay
{
    public interface IAliPayService: IPaymentService
    {
        string GetAliPayTradeOrderResponse(string orderRef, decimal amount);
    }
}