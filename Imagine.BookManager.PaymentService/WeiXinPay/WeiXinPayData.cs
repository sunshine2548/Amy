using Imagine.BookManager.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Imagine.BookManager.PaymentService.WeiXinPay
{
    public class WeiXinPayData
    {
        public SortedDictionary<string, object> SortedDictionary { get; set; }

        public WeiXinPayData()
        {
            SortedDictionary = new SortedDictionary<string, object>();
        }

        public WeiXinPayData(string xmlParamter)
        {
            this.FromXml(xmlParamter);
        }

        public WeiXinPayData(string orderRef, decimal amount)
        {
            SortedDictionary[WeiXinPayInfo.OutTradeNo] = orderRef;
            SortedDictionary[WeiXinPayInfo.TotalFee] = amount * 100;
            SortedDictionary[WeiXinPayInfo.ProductId] = orderRef;
            SortedDictionary[WeiXinPayInfo.TimeStart] = DateTime.Now.ToString("yyyyMMddHHmmss");
            SortedDictionary[WeiXinPayInfo.TimeExpire] = DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss");
            SortedDictionary[WeiXinPayInfo.TradeType] = "NATIVE";
            SortedDictionary[WeiXinPayInfo.NotifyUrl] = WeiXinPayInfo.WeiXinPayNodifyUrl;
            SortedDictionary[WeiXinPayInfo.Appid] = WeiXinPayInfo.WeiXinPayAppId;
            SortedDictionary[WeiXinPayInfo.MchId] = WeiXinPayInfo.WeiXinPayMchid;
            SortedDictionary[WeiXinPayInfo.SpbillCreateIp] = WeiXinPayInfo.WeiXinPayLocalServerIp;
            SortedDictionary[WeiXinPayInfo.NonceStr] = DateTime.Now.Ticks;
            SortedDictionary[WeiXinPayInfo.Body] = ConfigHelper.PaymentBody;
            SortedDictionary[WeiXinPayInfo.Attach] = ConfigHelper.PaymentBody;
            SortedDictionary[WeiXinPayInfo.GoodsTag] = ConfigHelper.PaymentSubject;
        }

        public void SetDictionaryValue(string key, object value)
        {
            SortedDictionary[key] = value;
        }

        public Object GetDictionaryValue(string key)
        {
            SortedDictionary.TryGetValue(key, out object o);
            return o;
        }

        /// <summary>
        /// 将sortedDictionary转换成XML
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            if (SortedDictionary.Count == 0)
                return string.Empty;
            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (KeyValuePair<string, object> item in SortedDictionary)
            {
                sb.Append($"<{item.Key}>{item.Value}</{item.Key}");
            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        public string ToUrlParamter()
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, object> pair in SortedDictionary)
            {
                if (pair.Value == null)
                {
                    return string.Empty;
                }
                if (pair.Key != WeiXinPayInfo.Sign && pair.Value.ToString() != "")
                {
                    sb.Append($"{pair.Key}={pair.Value}&");
                }
            }
            return sb.ToString();
        }

        public void FromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new Exception("weixin request or response is null");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;
            XmlNodeList nodes = xmlNode.ChildNodes;
            SortedDictionary = new SortedDictionary<string, object>();
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                SortedDictionary[xe.Name] = xe.InnerText;
            }
            if (SortedDictionary[WeiXinPayInfo.ReturnCode].ToString() != "SUCCESS")
            {
                SortedDictionary = new SortedDictionary<string, object>();
            }
            if (CheckSign() == false)
                SortedDictionary = new SortedDictionary<string, object>();
        }

        public string MakeSign()
        {
            string str = ToUrlParamter();
            str += "key=" + WeiXinPayInfo.WeiXinPayKey;
            var md5 = Util.CreateMd5(str);
            return md5.ToUpper();
        }

        public bool CheckSign()
        {
            if (SortedDictionary.ContainsKey(WeiXinPayInfo.Sign))
            {
                return false;
            }
            if (SortedDictionary["sign"] == null || SortedDictionary[WeiXinPayInfo.Sign].ToString() == "")
            {
                return false;
            }
            string returnSign = SortedDictionary[WeiXinPayInfo.Sign].ToString();

            string newSign = MakeSign();

            if (newSign == returnSign)
            {
                return true;
            }
            return false;
        }
    }
}
