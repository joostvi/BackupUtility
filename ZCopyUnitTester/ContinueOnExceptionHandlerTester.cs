using Xunit;
using Moq;
using System;
using ZCopy.Classes.ExceptionHandling;
using ZCopy.Interfaces;
using ZCopyUnitTester.Mocks;

namespace ZCopyUnitTester
{
    [Collection("ContinueOnExceptionHandlerTester")]
    public class ContinueOnExceptionHandlerTester
    {
        [Fact]
        public void HandleException_ExpectMessageToBeLogged()
        {
            const string message = "This is the message";
            const string exceptionMessage = "This is the exception";
            Mock<IEventLogger> eventLogger = MockBuilder.CreateIEventLogger(message);
            ContinueOnExceptionHandler handler = new ContinueOnExceptionHandler(eventLogger.Object);
            Exception ex = new Exception(exceptionMessage);
            handler.HandleException(message, ex);

            Assert.Equal(1, eventLogger.Invocations.Count);
            eventLogger.Verify(a => a.LogEvent(message));
        }

        [Fact]
        public void HandleException_ExpectExceptionToBeLogged_WhenNoMessage()
        {
            const string message = "This is the message";
            const string exceptionMessage = "This is the exception";
            Mock<IEventLogger> eventLogger = MockBuilder.CreateIEventLogger(message);
            ContinueOnExceptionHandler handler = new ContinueOnExceptionHandler(eventLogger.Object);
            Exception ex = new Exception(exceptionMessage);
            handler.HandleException(null, ex);

            Assert.Equal(1, eventLogger.Invocations.Count);
            eventLogger.Verify(a => a.LogEvent(exceptionMessage));
        }
    }
}
