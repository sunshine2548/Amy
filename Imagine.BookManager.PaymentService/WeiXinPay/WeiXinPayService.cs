using System.Collections.Generic;
using Imagine.BookManager.Common;
using System;
using Imagine.BookManager.Dto.PayMent;

namespace Imagine.BookManager.PaymentService.WeiXinPay
{
    public class WeiXinPayService : IWeiXinPayService
    {
        private readonly IWebUtilService _iWebUtilService;

        public WeiXinPayService(IWebUtilService iWebUtilService)
        {
            _iWebUtilService = iWebUtilService;
        }

        public PayResult GetOrderPaidStatus(string xmlParamter)
        {
            WeiXinPayData responseData = new WeiXinPayData(xmlParamter);
            SortedDictionary<string, object> sortedDictionary = responseData.SortedDictionary;
            PayResult payResult = new PayResult();
            if (sortedDictionary.Count == 0)
                return payResult;
            if (sortedDictionary.ContainsKey(WeiXinPayInfo.Sign) == false)
            {
                return payResult;
            }
            if (sortedDictionary.ContainsKey(WeiXinPayInfo.TransactionId) == false)
            {
                return payResult;
            }
            WeiXinPayData checkData = new WeiXinPayData();
            checkData.SetDictionaryValue(WeiXinPayInfo.TransactionId, sortedDictionary[WeiXinPayInfo.TransactionId]);
            checkData.SetDictionaryValue(WeiXinPayInfo.Appid, WeiXinPayInfo.WeiXinPayAppId);
            checkData.SetDictionaryValue(WeiXinPayInfo.MchId, WeiXinPayInfo.WeiXinPayMchid);
            checkData.SetDictionaryValue(WeiXinPayInfo.NonceStr, DateTime.Now.Ticks);
            checkData.SetDictionaryValue(WeiXinPayInfo.SignType, "MD5");
            checkData.SetDictionaryValue(WeiXinPayInfo.Sign, checkData.MakeSign());
            string responseXmlPostRequest = _iWebUtilService.PostRequest(
                WeiXinPayInfo.WeiXinPayServerUrl,
                checkData.ToXml());
            WeiXinPayData response = new WeiXinPayData(responseXmlPostRequest);
            string returnCode = response.GetDictionaryValue(WeiXinPayInfo.ReturnCode)?.ToString() ?? "";
            string resultCode = response.GetDictionaryValue(WeiXinPayInfo.ResultCode)?.ToString() ?? "";
            if (returnCode == "SUCCESS" && resultCode == "SUCCESS")
            {
                payResult.IsSuccess = true;
                payResult.OrderRef = response.GetDictionaryValue(WeiXinPayInfo.OutTradeNo).ToString();
                payResult.GatewayRef = response.GetDictionaryValue(WeiXinPayInfo.TransactionId).ToString();
            }
            return payResult;
        }

        public WeiXinOrderResult GetWeiXinPayQrCode(string orderRef, decimal amount)
        {
            WeiXinPayData weiXinPayData = new WeiXinPayData(orderRef, amount);
            weiXinPayData.SetDictionaryValue(WeiXinPayInfo.Sign, weiXinPayData.MakeSign());
            string httpXml = weiXinPayData.ToXml();
            string responseXmlPostRequest = _iWebUtilService.PostRequest(WeiXinPayInfo.WeiXinPayServerUrl, httpXml);
            WeiXinPayData responseData = new WeiXinPayData(responseXmlPostRequest);
            var result = responseData.GetDictionaryValue(WeiXinPayInfo.CodeUrl);
            WeiXinOrderResult orderResult = new WeiXinOrderResult()
            {
                OrderRef = orderRef,
                OrderQrCode = result?.ToString() ?? ""
            };
            return orderResult;
        }
    }
}