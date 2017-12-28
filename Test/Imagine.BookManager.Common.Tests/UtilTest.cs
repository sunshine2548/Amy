using Shouldly;
using Xunit;

namespace Imagine.BookManager.Common.Tests
{
    public class UtilTest
    {
        [Fact]
        public void CreateMd5Test()
        {
            var md5 = "e10adc3949ba59abbe56e057f20f883e";
            var result = Util.CreateMd5("123456");
            result.ShouldBe(md5);
        }
    }
}