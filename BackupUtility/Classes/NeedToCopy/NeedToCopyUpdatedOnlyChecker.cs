using System.IO;
using GenericClassLibrary.FileSystem;
using ZCopy.Interfaces;

namespace ZCopy.Classes.NeedToCopy
{
    public class NeedToCopyUpdatedOnlyChecker : INeedToCopyChecker
    {
        private readonly IFileComparer _fileComparer;
        private readonly IFileSystem _fileSystem;
        private readonly IConfirmationChecker _confirmationChecker;

        public NeedToCopyUpdatedOnlyChecker(IFileComparer fileComparer, IFileSystem fileSystem, IConfirmationChecker confirmationChecker)
        {
            _fileComparer = fileComparer;
            _confirmationChecker = confirmationChecker;
            _fileSystem = fileSystem;
        }

        public bool NeedToCopy(string aSource, string aTarget)
        {
            if (!_fileSystem.File.Exists(aTarget))
            {
                return true;
            }
            else if (_fileComparer.IsSameFile(new FileInfo(aSource), new FileInfo(aTarget)))
            {
                return false;
            }
            return _confirmationChecker.GetConfirmation(aTarget);
        }
    }
}
