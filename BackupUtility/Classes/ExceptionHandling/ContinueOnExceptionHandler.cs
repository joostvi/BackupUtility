using System;

namespace ZCopy
{
    public class ContinueOnExceptionHandler : IExceptionHandler
    {
        private CommandHandler _commandHandler;

        public ContinueOnExceptionHandler(CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public void HandleException(string message, Exception ex)
        {
            string eventText = message;
            if ((eventText == "" || eventText == null))
                eventText = ex.Message;
            _commandHandler.RaiseThisEvent(eventText);
        }
    }
}
