using System;
using ZCopy.Interfaces;

namespace ZCopy.Classes.ExceptionHandling
{
    public class StopOnExceptionHandler : IExceptionHandler
    {
        public void HandleException(string message, Exception ex)
        {
            throw ex;
        }
    }
}
