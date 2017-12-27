using Imagine.BookManager.PaymentService.Tests.StepUp;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Xunit;

namespace Imagine.BookManager.PaymentService.Tests
{
    public class WebUtilServiceTest : BookManagerTestModule
    {
        private readonly IWebUtilService _webUtilService;
        public WebUtilServiceTest()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://localost/api/user/*")
                .Respond("text/plain", "test");
            _webUtilService = new WebUtilService(mockHttp);
        }

        [Fact]
        public void PostRequest_Should_Return_Correct_Number_Of_Records()
        {
            var result = _webUtilService.PostRequest("http://localost/api/user/1", "a");
            Assert.Equal("test", result);
        }


    }
}
