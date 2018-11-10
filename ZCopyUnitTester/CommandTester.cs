using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZCopy;

namespace ZCopyUnitTester
{
    [TestClass()]
    public class CommandTester
    {
        [TestMethod()]
        public void TestCreateWithSourceEndsOnQuoate()
        {
            string[] args = new string[2];
            string input = @"c:\user\joost\""";
            string expected = @"c:\user\joost\";

            args[0] = input;
            args[1] = "somepath";

            Assert.AreEqual(expected, new Commands(args).Source);

        }
    }
}