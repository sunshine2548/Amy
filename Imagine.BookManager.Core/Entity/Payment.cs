using System;

namespace Imagine.BookManager.Core.Entity
{
    public class Payment : Abp.Domain.Entities.Entity<Int64>
    {
        public string OrderRef { get; set; }
        public string GatewayRef { get; set; }
        public decimal Amount { get; set; }
        public PaymentGateWay PaymenGateway { get; set; }
        public bool Paid { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DatePaid { get; set; }

        public Payment()
        {
            DateCreated = DateTime.Now;
        }


    }

    public enum PaymentGateWay
    {
        AliPay = 1,
        WeiXinPay = 2
    }
}
