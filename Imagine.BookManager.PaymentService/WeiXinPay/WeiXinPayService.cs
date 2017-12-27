using System.Collections.Generic;
using Imagine.BookManager.Common;
using System;
using Imagine.BookManager.Dto.PayMent;

namespace Imagine.BookManager.PaymentService.WeiXinPay
{
    public class WeiXinPayService : IWeiXinPayService
    {
        public PayResult GetOrderPaidStatus(string xmlParamter)
        {
            WeiXinPayData responseData = new WeiXinPayData(xmlParamter);
            SortedDictionary<string, object> sortedDictionary = responseData.SortedDictionary;
            PayResult payResult = new PayResult();
            if (sortedDictionary.Count == 0)
                return payResult;
            if (sortedDictionary.ContainsKey("sign") == false)
            {
                return payResult;
            }
            if (sortedDictionary.ContainsKey("transaction_id") == false)
            {
                return payResult;
            }
            WeiXinPayData checkData = new WeiXinPayData();
            checkData.SetDictionaryValue("transaction_id", sortedDictionary["transaction_id"]);
            checkData.SetDictionaryValue("appid", WeiXinPayInfo.WeiXinPayAppId);
            checkData.SetDictionaryValue("mch_id", WeiXinPayInfo.WeiXinPayMchid);
            checkData.SetDictionaryValue("nonce_str", DateTime.Now.Ticks);
            checkData.SetDictionaryValue("sign_type", "MD5");
            checkData.SetDictionaryValue("sign", checkData.MakeSign());
            string responseXmlPostRequest = WebUtil.PostRequest(
                WeiXinPayInfo.WeiXinPayServerUrl,
                checkData.ToXml());
            WeiXinPayData response = new WeiXinPayData(responseXmlPostRequest);
            string returnCode = response.GetDictionaryValue("return_code")?.ToString() ?? "";
            string resultCode = response.GetDictionaryValue("result_code")?.ToString() ?? "";
            if (returnCode == "SUCCESS" && resultCode == "SUCCESS")
            {
                payResult.IsSuccess = true;
                payResult.OrderRef = response.GetDictionaryValue("out_trade_no").ToString();
                payResult.GatewayRef = response.GetDictionaryValue("transaction_id").ToString();
            }
            return payResult;
        }

        public WeiXinOrderResult GetWeiXinPayQrCode(string orderRef, decimal payMoney)
        {
            WeiXinPayData weiXinPayData = new WeiXinPayData(orderRef, payMoney);
            weiXinPayData.SetDictionaryValue("sign", weiXinPayData.MakeSign());
            string httpXml = weiXinPayData.ToXml();
            string responseXmlPostRequest = WebUtil.PostRequest(WeiXinPayInfo.WeiXinPayServerUrl, httpXml);
            WeiXinPayData responseData = new WeiXinPayData(responseXmlPostRequest);
            var result = responseData.GetDictionaryValue("code_url");
            WeiXinOrderResult orderResult = new WeiXinOrderResult()
            {
                OrderRef = orderRef,
                OrderQrCode = result?.ToString() ?? ""
            };
            return orderResult;
        }
    }
}