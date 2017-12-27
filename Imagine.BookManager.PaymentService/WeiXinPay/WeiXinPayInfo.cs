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

        public const string OutTradeNo = "out_trade_no";

        public const string TotalFee = "total_fee";

        public const string ProductId = "product_id";

        public const string TimeStart = "time_start";

        public const string TimeExpire = "time_expire";

        public const string TradeType = "trade_type";

        public const string NotifyUrl = "notify_url";

        public const string Appid = "appid";

        public const string MchId = "mch_id";

        public const string SpbillCreateIp = "spbill_create_ip";

        public const string NonceStr = "nonce_str";

        public const string Body = "body";

        public const string Attach = "attach";

        public const string GoodsTag = "goods_tag";

        public const string Sign = "sign";

        public const string TransactionId = "transaction_id";

        public const string ReturnCode = "return_code";

        public const string ResultCode = "result_code";

        public const string SignType = "sign_type";

        public const string CodeUrl = "code_url";

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