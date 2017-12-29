using Shouldly;
using Xunit;
using Imagine.BookManager.Common;

namespace Imagine.BookManager.Common.Tests
{
    public class GuardTest
    {
        [Fact]
        public void StringEmptyTest()
        {
            string result = Guard.EnsureParam(" ");
            result.ShouldBe(string.Empty);
        }

        [Fact]
        public void StringNullTest()
        {
            string result = Guard.EnsureParam(null);
            result.ShouldBe(string.Empty);
        }

        [Fact]
        public void StringTest()
        {
            string result = Guard.EnsureParam("a");
            result.ShouldBe("a");
        }
    }
}
