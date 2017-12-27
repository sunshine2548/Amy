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
            WeiXinPayNodifyUrl = PaymentConfig.GetValue("WeiXinPayNodifyUrl");
            WeiXinPayKey = PaymentConfig.GetValue("WeiXinPayKey");
            WeiXinPayServerUrl = PaymentConfig.GetValue("WeiXinPayUrl");
            WeiXinPayQueryOrderUrl = PaymentConfig.GetValue("WeiXinPayQueryOrderUrl");
            WeiXinPayAppId = PaymentConfig.GetValue("WeiXinPayAppId");
            WeiXinPayMchid = PaymentConfig.GetValue("WeiXinPayMCHID");
            WeiXinPayLocalServerIp = PaymentConfig.GetValue("WeiXinPayLocalServerIp");
        }
    }
}