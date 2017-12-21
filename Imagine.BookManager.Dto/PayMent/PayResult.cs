namespace Imagine.BookManager.Dto.PayMent
{
    public class PayResult
    {
        public bool IsSuccess { get; set; }

        public string OrderRef { get; set; }

        public string GatewayRef { get; set; }
    }
}