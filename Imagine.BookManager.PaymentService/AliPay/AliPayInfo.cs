using Aop.Api;
using Imagine.BookManager.Common;

namespace Imagine.BookManager.PaymentService.AliPay
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
            AliPayNotifyUrl = ConfigHelper.GetValue("AliPayNotifyUrl");
            AliPayReturnUrl = ConfigHelper.GetValue("AliPayReturnUrl");
            AliPayAppId = ConfigHelper.GetValue("AliPayAppId");
            AliPayServerUrl = ConfigHelper.GetValue("AliPayServerUrl");
            AliPayPublicKey = ConfigHelper.GetFileContent("ApliPayPublicKey_Path");
            AliPayOuerKey = ConfigHelper.GetFileContent("ApliPayOuerKey_Path");
            AopClient = new DefaultAopClient(AliPayServerUrl, AliPayAppId, AliPayOuerKey, "json", "1.0", "RSA2", AliPayPublicKey, "utf-8", false);
        }
    }
}