using System.IO;
using GenericClassLibrary.FileSystem;

namespace ZCopy
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
            bool doCopy = false;
            if (!_fileSystem.File.Exists(aTarget))
            {
                doCopy = true;
            }
            else if (_confirmationChecker.GetConfirmation(aTarget))
            {
                doCopy = !_fileComparer.IsSameFile(new FileInfo(aSource), new FileInfo(aTarget));
            }
            return doCopy;
        }
    }
}
