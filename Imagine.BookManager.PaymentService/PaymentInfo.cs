using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagine.BookManager.Common;

namespace Imagine.BookManager.PaymentService
{
    public class PaymentInfo
    {
        public static readonly string PaymentBody;

        public static readonly string PaymentSubject;

        static PaymentInfo()
        {
            PaymentBody = ConfigHelper.GetValue("PaymentBody");
            PaymentSubject = ConfigHelper.GetValue("PaymentSubject");
        }
    }
}
