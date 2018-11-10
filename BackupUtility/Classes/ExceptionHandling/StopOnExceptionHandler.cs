using System;

namespace ZCopy
{
    public class StopOnExceptionHandler : IExceptionHandler
    {
        public void HandleException(string message, Exception ex)
        {
            throw ex;
        }
    }
}
