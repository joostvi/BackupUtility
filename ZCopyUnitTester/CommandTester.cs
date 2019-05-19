using Xunit;
using ZCopy.Classes;

namespace ZCopyUnitTester
{
    [Collection("CommandTester")]
    public class CommandTester
    {
        [Fact]
        public void TestCreateWithSourceEndsOnQuote()
        {
            string[] args = new string[2];
            string input = @"c:\user\joost\""";
            string expected = @"c:\user\joost\";

            args[0] = input;
            args[1] = "somepath";

            Assert.Equal(expected, new Commands(args).Source);

        }

        [Theory]
        [InlineData("/y", false)]
        [InlineData("", true)]
        public void TestCreateRequestConfrm(string value, bool expected)
        {
            string[] args;
            if (value != "")
            {
                args = new string[3];
                args[0] = "dummy";
                args[1] = "somepath";
                args[2] = value;
            }
            else
            {
                args = new string[2];
                args[0] = "dummy";
                args[1] = "somepath";
            }         

            Assert.Equal(expected, new Commands(args).RequestConfirm);

        }
    }
}