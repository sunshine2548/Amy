using Aop.Api;

namespace Imagine.BookManager.PaymentService
{
    public class AliPayInfo
    {
        public static readonly string AliPayNotifyUrl;

        public static readonly string AliPayReturnUrl;

        public static readonly string AliPayAppId;

        public static readonly string AliPayPublicKey;

        public static readonly string AliPayOuerKey;

        public static readonly string AliPayServerUrl;

        public static readonly IAopClient AopClient;

        static AliPayInfo()
        {
            AliPayNotifyUrl = PaymentConfig.GetValue("AliPayNotifyUrl");
            AliPayReturnUrl = PaymentConfig.GetValue("AliPayReturnUrl");
            AliPayAppId = PaymentConfig.GetValue("AliPayAppId");
            AliPayServerUrl = PaymentConfig.GetValue("AliPayServerUrl");
            AliPayPublicKey = PaymentConfig.GetFileContent("ApliPayPublicKey_Path");
            AliPayOuerKey = PaymentConfig.GetFileContent("ApliPayOuerKey_Path");
            AopClient = new DefaultAopClient(AliPayServerUrl, AliPayAppId, AliPayOuerKey, "json", "1.0", "RSA2", AliPayPublicKey, "utf-8", false);
        }
    }
}