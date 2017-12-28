namespace Imagine.BookManager.PaymentService
{
    public interface IWebUtilService
    {
        string PostRequest(string url, string paramter, string contentType = "text/xml");
    }
}