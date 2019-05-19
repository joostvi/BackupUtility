using GenericClassLibrary.FileSystem;
using GenericClassLibrary.Logging;
using ZCopy.Interfaces;

namespace ZCopy.Classes.NeedToCopy
{
    public class NeedToCopyWithConfirmation : INeedToCopyChecker
    {
        private readonly IFileSystem _fileSystem;
        private readonly IConfirmationChecker _confirmationChecker;

        public NeedToCopyWithConfirmation(IFileSystem fileSystem, IConfirmationChecker confirmationChecker)
        {
            _fileSystem = fileSystem;
            _confirmationChecker = confirmationChecker;
        }
        public bool NeedToCopy(string aSource, string aTarget)
        {
            if (!_fileSystem.File.Exists(aTarget))
            {
                Logger.Debug($"NeedToCopyWithConfirmation: File {aTarget} not found! Need to copy.");
                return true;
            }
            return _confirmationChecker.GetConfirmation(aTarget);
        }
    }
}
