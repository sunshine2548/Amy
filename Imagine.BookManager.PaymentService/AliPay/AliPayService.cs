using Aop.Api.Request;
using Aop.Api.Response;

namespace Imagine.BookManager.PaymentService.AliPay
{
    public class AliPayService : IAliPayService
    {
        public string GetAliPayTradeOrderResponse(string orderRef, decimal payMoney)
        {
            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            request.SetReturnUrl(AliPayInfo.AliPayReturnUrl);
            request.SetNotifyUrl(AliPayInfo.AliPayNotifyUrl);
            request.BizContent = "{" +
                                 "    \"body\":\"" + PaymentConfig.PaymentBody + "\"," +
                                 "    \"subject\":\"" + PaymentConfig.PaymentSubject + "\"," +
                                 "    \"out_trade_no\":\"" + orderRef + "\"," +
                                 "    \"total_amount\":" + payMoney + "," +
                                 "    \"product_code\":\"FAST_INSTANT_TRADE_PAY\"" +
                                 "  }";
            AlipayTradePagePayResponse response = AliPayInfo.AopClient.pageExecute(request);
            return response.Body;
        }
    }
}