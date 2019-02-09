using Xunit;
using ZCopy.Classes;

namespace ZCopyUnitTester
{
    [Collection("CommandTester")]
    public class CommandTester
    {
        [Fact]
        public void TestCreateWithSourceEndsOnQuoate()
        {
            string[] args = new string[2];
            string input = @"c:\user\joost\""";
            string expected = @"c:\user\joost\";

            args[0] = input;
            args[1] = "somepath";

            Assert.Equal(expected, new Commands(args).Source);

        }
    }
}