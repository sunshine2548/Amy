using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Imagine.BookManager.Dto.PayMent;

namespace Imagine.BookManager.PaymentService.AliPay
{
    public class AliPayService : IAliPayService
    {
        public string GetAliPayTradeOrderResponse(string orderRef, decimal amount)
        {
            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            request.SetReturnUrl(AliPayInfo.AliPayReturnUrl);
            request.SetNotifyUrl(AliPayInfo.AliPayNotifyUrl);
            request.BizContent = "{" +
                                 "    \"body\":\"" + PaymentInfo.PaymentBody + "\"," +
                                 "    \"subject\":\"" + PaymentInfo.PaymentSubject + "\"," +
                                 "    \"out_trade_no\":\"" + orderRef + "\"," +
                                 "    \"total_amount\":" + amount + "," +
                                 "    \"product_code\":\"FAST_INSTANT_TRADE_PAY\"" +
                                 "  }";
            AlipayTradePagePayResponse response = AliPayInfo.AopClient.pageExecute(request);
            return response.Body;
        }

        public PayResult GetOrderPaidStatus(string xmlParamter)
        {
            Dictionary<string, string> aliPayResponse=new Dictionary<string, string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlParamter);
            XmlNode xmlNode = xmlDoc.FirstChild;
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                aliPayResponse[xe.Name] = xe.InnerText;
            }
            var result = new PayResult();
            if (aliPayResponse.Count == 0)
            {
                return result;
            }
            if (aliPayResponse.ContainsKey(AliPayInfo.TradeNo) == false)
                return result;
            if (aliPayResponse.ContainsKey(AliPayInfo.OutTradeNo) == false)
                return result;
            bool isPaid = string.CompareOrdinal(aliPayResponse[AliPayInfo.TradeStatus], AliPayInfo.TradeSuccess) == 0 ||
                         string.CompareOrdinal(aliPayResponse[AliPayInfo.TradeStatus], AliPayInfo.TradeFinished) == 0;
            result.IsSuccess = isPaid;
            result.OrderRef = aliPayResponse[AliPayInfo.OutTradeNo];
            result.GatewayRef = aliPayResponse[AliPayInfo.TradeNo];
            result.IsSuccess =
                AlipaySignature.RSACheckV1(aliPayResponse, AliPayInfo.AliPayPublicKey, "utf-8", "RSA2", false);
            return result;
        }
    }
}