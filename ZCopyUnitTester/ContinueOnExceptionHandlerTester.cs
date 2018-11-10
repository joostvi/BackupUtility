using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCopy.Classes.ExceptionHandling;
using ZCopy.Interfaces;
using ZCopyUnitTester.Mocks;

namespace ZCopyUnitTester
{
    [TestClass()]
    public class ContinueOnExceptionHandlerTester
    {
        [TestMethod]
        public void HandleException_ExpectMessageToBeLogged()
        {
            const string message = "This is the message";
            const string exceptionMessage = "This is the exception";
            Mock<IEventLogger> eventLogger = MockBuilder.CreateIEventLogger(message);
            ContinueOnExceptionHandler handler = new ContinueOnExceptionHandler(eventLogger.Object);
            Exception ex = new Exception(exceptionMessage);
            handler.HandleException(message, ex);

            Assert.AreEqual(1, eventLogger.Invocations.Count);
            eventLogger.Verify(a => a.LogEvent(message));
        }

        [TestMethod]
        public void HandleException_ExpectExceptionToBeLogged_WhenNoMessage()
        {
            const string message = "This is the message";
            const string exceptionMessage = "This is the exception";
            Mock<IEventLogger> eventLogger = MockBuilder.CreateIEventLogger(message);
            ContinueOnExceptionHandler handler = new ContinueOnExceptionHandler(eventLogger.Object);
            Exception ex = new Exception(exceptionMessage);
            handler.HandleException(null, ex);

            Assert.AreEqual(1, eventLogger.Invocations.Count);
            eventLogger.Verify(a => a.LogEvent(exceptionMessage));
        }
    }
}
