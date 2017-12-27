using Imagine.BookManager.Common;

namespace Imagine.BookManager.PaymentService.WeiXinPay
{
    public class WeiXinPayInfo
    {
        public static readonly string WeiXinPayNodifyUrl;

        public static readonly string WeiXinPayKey;

        public static readonly string WeiXinPayServerUrl;

        public static readonly string WeiXinPayQueryOrderUrl;

        public static readonly string WeiXinPayAppId;

        public static readonly string WeiXinPayMchid;

        public static readonly string WeiXinPayLocalServerIp;

        static WeiXinPayInfo()
        {
            WeiXinPayNodifyUrl = ConfigHelper.GetValue("WeiXinPayNodifyUrl");
            WeiXinPayKey = ConfigHelper.GetValue("WeiXinPayKey");
            WeiXinPayServerUrl = ConfigHelper.GetValue("WeiXinPayUrl");
            WeiXinPayQueryOrderUrl = ConfigHelper.GetValue("WeiXinPayQueryOrderUrl");
            WeiXinPayAppId = ConfigHelper.GetValue("WeiXinPayAppId");
            WeiXinPayMchid = ConfigHelper.GetValue("WeiXinPayMCHID");
            WeiXinPayLocalServerIp = ConfigHelper.GetValue("WeiXinPayLocalServerIp");
        }
    }
}