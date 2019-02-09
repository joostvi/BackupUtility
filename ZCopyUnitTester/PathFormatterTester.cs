using Xunit;
using ZCopy.Classes;

namespace ZCopyUnitTester
{
    [Collection("PathFormatterTester")]
    public class PathFormatterTester
    {
        [Fact()]
        public void TestWithDoubleQoutesAndSlash()
        {
            string input = @"c:\user\joost\""";
            string expected = @"c:\user\joost\";
            Assert.Equal(expected, PathFormatter.FormatPath(input));
        }

        [Fact()]
        public void TestWithDoubleQoutesNoSlash()
        {
            string input = @"c:\user\joost""";
            string expected = @"c:\user\joost";
            Assert.Equal(expected, PathFormatter.FormatPath(input));
        }
    }
}