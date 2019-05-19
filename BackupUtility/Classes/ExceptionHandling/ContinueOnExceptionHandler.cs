using GenericClassLibrary.Logging;
using System;
using ZCopy.Interfaces;

namespace ZCopy.Classes.ExceptionHandling
{
    public class ContinueOnExceptionHandler : IExceptionHandler
    {
        private readonly IEventLogger _eventLogger;

        public ContinueOnExceptionHandler(IEventLogger commandHandler)
        {
            _eventLogger = commandHandler;
        }

        public void HandleException(string message, Exception ex)
        {
            string eventText = message;
            if ((eventText == "" || eventText == null))
            {
                eventText = ex.Message;
            }
            Logger.Error($"Exception (IGNORED): {ex.Message}");
            _eventLogger.LogEvent(eventText);
        }
    }
}
