using ZCopy.Interfaces;

namespace ZCopy.Interfaces
{
    public interface ICommandHandler : IFileReadableChecker, IFileIgnoreChecker, INeedToCopyChecker, IConfirmationChecker, IEventLogger, IExceptionHandler
    {
        void CopyFile(string aFile, string aTarget);
        bool CreateDirectory(string theTarget);
    }
}
