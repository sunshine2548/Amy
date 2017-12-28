using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace Imagine.BookManager.PaymentService
{
    public class WebUtilService : IWebUtilService
    {
        private readonly HttpMessageHandler _httpMessageHandler;

        public WebUtilService(HttpMessageHandler httpMessageHandler)
        {
            _httpMessageHandler = httpMessageHandler;
        }

        public string PostRequest(string url, string paramter, string contentType = "text/xml")
        {
            byte[] data = Encoding.UTF8.GetBytes(paramter);
            using (HttpClient client = new HttpClient(_httpMessageHandler))
            {
                client.MaxResponseContentBufferSize = 2048 * 1000;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                HttpContent content = new ByteArrayContent(data);
                var responseMessage = client.PostAsync(new Uri(url), content).Result;
                return responseMessage.Content.ReadAsStringAsync().Result;
            }
        }
    }
}