using System.IO;
using GenericClassLibrary.FileSystem;
using GenericClassLibrary.Logging;
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

        public bool NeedToCopy(FolderMap baseMap, string aSource, string aTarget)
        {
            Logger.Debug($"aSource={aSource}, aTarget={aTarget}");
            Logger.Debug($"TypeOf(_fileSystem)={_fileSystem.GetType()} ");
            if (!_fileSystem.File.Exists(aTarget))
            {
                Logger.Debug($"File {aTarget} not found! Need to copy.");
                return true;
            }
            else if (_fileComparer.IsSameFile(new FileInfo(aSource), new FileInfo(aTarget)))
            {
                Logger.Debug($"File {aTarget} equal to {aSource}! Skip file!");
                return false;
            }
            return _confirmationChecker.GetConfirmation(aTarget);
        }
    }
}
