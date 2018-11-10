using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZCopy;

namespace ZCopyUnitTester
{
    [TestClass()]
    public class PathFormatterTester
    {
        [TestMethod()]
        public void TestWithDoubleQoutesAndSlash()
        {
            string input = @"c:\user\joost\""";
            string expected = @"c:\user\joost\";
            Assert.AreEqual(expected, PathFormatter.FormatPath(input));
        }

        [TestMethod()]
        public void TestWithDoubleQoutesNoSlash()
        {
            string input = @"c:\user\joost""";
            string expected = @"c:\user\joost";
            Assert.AreEqual(expected, PathFormatter.FormatPath(input));
        }
    }
}