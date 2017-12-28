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
            if (sortedDictionary.ContainsKey(Common.WeiXinPayInfo.Sign) == false)
            {
                return payResult;
            }
            if (sortedDictionary.ContainsKey(Common.WeiXinPayInfo.TransactionId) == false)
            {
                return payResult;
            }
            WeiXinPayData checkData = new WeiXinPayData();
            checkData.SetDictionaryValue(Common.WeiXinPayInfo.TransactionId, sortedDictionary[Common.WeiXinPayInfo.TransactionId]);
            checkData.SetDictionaryValue(Common.WeiXinPayInfo.Appid, WeiXinPayInfo.WeiXinPayAppId);
            checkData.SetDictionaryValue(Common.WeiXinPayInfo.MchId, WeiXinPayInfo.WeiXinPayMchid);
            checkData.SetDictionaryValue(Common.WeiXinPayInfo.NonceStr, DateTime.Now.Ticks);
            checkData.SetDictionaryValue(Common.WeiXinPayInfo.SignType, "MD5");
            checkData.SetDictionaryValue(Common.WeiXinPayInfo.Sign, checkData.MakeSign());
            string responseXmlPostRequest = _iWebUtilService.PostRequest(
                WeiXinPayInfo.WeiXinPayServerUrl,
                checkData.ToXml());
            WeiXinPayData response = new WeiXinPayData(responseXmlPostRequest);
            string returnCode = response.GetDictionaryValue(Common.WeiXinPayInfo.ReturnCode)?.ToString() ?? "";
            string resultCode = response.GetDictionaryValue(Common.WeiXinPayInfo.ResultCode)?.ToString() ?? "";
            if (returnCode == "SUCCESS" && resultCode == "SUCCESS")
            {
                payResult.IsSuccess = true;
                payResult.OrderRef = response.GetDictionaryValue(Common.WeiXinPayInfo.OutTradeNo).ToString();
                payResult.GatewayRef = response.GetDictionaryValue(Common.WeiXinPayInfo.TransactionId).ToString();
            }
            return payResult;
        }

        public WeiXinOrderResult GetWeiXinPayQrCode(string orderRef, decimal amount)
        {
            WeiXinPayData weiXinPayData = new WeiXinPayData(orderRef, amount);
            weiXinPayData.SetDictionaryValue(Common.WeiXinPayInfo.Sign, weiXinPayData.MakeSign());
            string httpXml = weiXinPayData.ToXml();
            string responseXmlPostRequest = _iWebUtilService.PostRequest(WeiXinPayInfo.WeiXinPayServerUrl, httpXml);
            WeiXinPayData responseData = new WeiXinPayData(responseXmlPostRequest);
            var result = responseData.GetDictionaryValue(Common.WeiXinPayInfo.CodeUrl);
            WeiXinOrderResult orderResult = new WeiXinOrderResult()
            {
                OrderRef = orderRef,
                OrderQrCode = result?.ToString() ?? ""
            };
            return orderResult;
        }
    }
}