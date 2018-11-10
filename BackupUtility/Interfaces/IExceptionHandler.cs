using System;

namespace ZCopy.Interfaces
{
    public interface IExceptionHandler
    {
        void HandleException(string message, Exception ex);
    }
}
