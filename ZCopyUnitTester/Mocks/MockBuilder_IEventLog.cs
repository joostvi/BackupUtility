using Moq;
using ZCopy.Interfaces;

namespace ZCopyUnitTester.Mocks
{
    public partial class MockBuilder
    {
        public static Mock<IEventLogger> CreateIEventLogger(string expectedMessage)
        {
            Mock<IEventLogger> eventLogger = new Mock<IEventLogger>();
            return eventLogger;
        }
    }
}
