using Imagine.BookManager.Core.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Imagine.BookManager.EntityFramework
{
    public class PaymentMap : EntityTypeConfiguration<Payment>
    {
        public PaymentMap()
        {
            ToTable("Payment");
            Property(p => p.GatewayRef).HasMaxLength(64);
            Property(p => p.Amount).HasPrecision(18, 2);
        }
    }
}
