using Imagine.BookManager.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Imagine.BookManager.PaymentService.WeiXinPay
{
    public class WeiXinPayData
    {
        private readonly SortedDictionary<string, object> _sortedDictionary = new SortedDictionary<string, object>();

        public WeiXinPayData()
        {

        }

        public WeiXinPayData(string xmlParamter)
        {
            this.FromXml(xmlParamter);
        }

        public WeiXinPayData(string orderRef, decimal payMoney)
        {
            _sortedDictionary["out_trade_no"] = orderRef;
            _sortedDictionary["total_fee"] = payMoney * 100;
            _sortedDictionary["product_id"] = orderRef;
            _sortedDictionary["time_start"] = DateTime.Now.ToString("yyyyMMddHHmmss");
            _sortedDictionary["time_expire"] = DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss");
            _sortedDictionary["trade_type"] = "NATIVE";
            _sortedDictionary["notify_url"] = WeiXinPayInfo.WeiXinPayNodifyUrl;
            _sortedDictionary["appid"] = WeiXinPayInfo.WeiXinPayAppId;
            _sortedDictionary["mch_id"] = WeiXinPayInfo.WeiXinPayMchid;
            _sortedDictionary["spbill_create_ip"] = WeiXinPayInfo.WeiXinPayLocalServerIp;
            _sortedDictionary["nonce_str"] = DateTime.Now.Ticks;
            _sortedDictionary["body"] = PaymentConfig.PaymentBody;
            _sortedDictionary["attach"] = PaymentConfig.PaymentBody;
            _sortedDictionary["goods_tag"] = PaymentConfig.PaymentSubject;
        }

        public SortedDictionary<string, object> SortedDictionary { get; }

        public void SetDictionaryValue(string key, object value)
        {
            _sortedDictionary[key] = value;
        }

        public Object GetDictionaryValue(string key)
        {
            _sortedDictionary.TryGetValue(key, out object o);
            return o;
        }

        /// <summary>
        /// 将sortedDictionary转换成XML
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            if (_sortedDictionary.Count == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (KeyValuePair<string, object> item in _sortedDictionary)
            {
                if (item.Value == null)
                {
                    return "";
                }
                sb.AppendFormat("<{0}>{1}</{0}>", item.Key, item.Value);
            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        public string ToUrlParamter()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in _sortedDictionary)
            {
                if (pair.Value == null)
                {
                    return string.Empty;
                }
                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }

        public void FromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                _sortedDictionary.Clear();
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                _sortedDictionary[xe.Name] = xe.InnerText;
            }
            if (_sortedDictionary.ContainsKey("return_code") == false)
            {
                _sortedDictionary.Clear();
                return;
            }
            try
            {
                if (_sortedDictionary["return_code"].ToString() != "SUCCESS")
                {
                    _sortedDictionary.Clear();
                    return;
                }
                if (CheckSign() == false)
                    _sortedDictionary.Clear();
            }
            catch (Exception)
            {
                _sortedDictionary.Clear();
            }
        }

        public string MakeSign()
        {
            string str = ToUrlParamter();
            str += "&key=" + WeiXinPayInfo.WeiXinPayKey;
            var md5 = Util.CreateMd5(str);
            return md5.ToUpper();
        }


        public bool CheckSign()
        {
            if (_sortedDictionary.ContainsKey("sign"))
            {
                return false;
            }
            if (_sortedDictionary["sign"] == null || _sortedDictionary["sign"].ToString() == "")
            {
                return false;
            }
            string returnSign = _sortedDictionary["sign"].ToString();

            string newSign = MakeSign();

            if (newSign == returnSign)
            {
                return true;
            }
            return false;
        }
    }
}
