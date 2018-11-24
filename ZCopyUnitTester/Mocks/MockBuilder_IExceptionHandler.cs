using Moq;
using ZCopy.Interfaces;

namespace ZCopyUnitTester.Mocks
{
    public partial class MockBuilder
    {
        public static Mock<IExceptionHandler> CreateIExceptionHandler()
        {
            Mock<IExceptionHandler> eventLogger = new Mock<IExceptionHandler>();
            return eventLogger;
        }
    }
}
