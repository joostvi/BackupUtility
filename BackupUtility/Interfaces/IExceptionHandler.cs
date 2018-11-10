using System;

namespace ZCopy
{
    public interface IExceptionHandler
    {
        void HandleException(string message, Exception ex);
    }
}
