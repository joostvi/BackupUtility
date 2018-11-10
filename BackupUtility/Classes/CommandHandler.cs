using GenericClassLibrary.FileSystem;
using ZCopy.Classes.NeedToCopy;
using ZCopy.Classes.ExceptionHandling;
using ZCopy.Classes.FileIgnore;
using ZCopy.Classes.Readability;
using ZCopy.Interfaces;

namespace ZCopy.Classes 
{
    public class CommandHandler : IFileReadableChecker, IFileIgnoreChecker, INeedToCopyChecker, IConfirmationChecker
    {

        // Private ReadOnly _commands As Commands

        private readonly IFileReadableChecker _fileReadableChecker;
        private readonly IExceptionHandler ExceptionHandler;
        private readonly IFileIgnoreChecker _fileIgnoreChecker;
        private readonly INeedToCopyChecker _needToCopyChecker;

        public event ProcessInfoEventEventHandler ProcessInfoEvent;

        public delegate void ProcessInfoEventEventHandler(string theInfo);

        public void RaiseThisEvent(string theInfo)
        {
            ProcessInfoEvent?.Invoke(theInfo);
        }

        public delegate bool ConfirmationRequest(string theInfo);

        private readonly ConfirmationRequest _ConfirmationRequest;

        public bool GetConfirmation(string aTarget)
        {
            if (_ConfirmationRequest != null)
                return _ConfirmationRequest.Invoke(aTarget);
            return true;
        }

        public CommandHandler(Commands commands)
        {
            // _commands = commands

            IFileSystem fileSystem = new FileSystem();
            if (commands.SkipCopyErrors)
                ExceptionHandler = new ContinueOnExceptionHandler(this);
            else
                ExceptionHandler = new StopOnExceptionHandler();

            if (commands.ReadCheckFirst)
                _fileReadableChecker = new FullReadChecker(ExceptionHandler);
            else
                _fileReadableChecker = new BasicReadChecker();
            if (commands.ExclusiveExt.Length > 0)
                _fileIgnoreChecker = new IgnoreOnExtensionsChecker(commands.ExclusiveExt, fileSystem, ExceptionHandler);
            else
                _fileIgnoreChecker = new IgnoreNoneChecker();

            if (commands.UpdatedOnly)
            {
                _needToCopyChecker = new NeedToCopyUpdatedOnlyChecker(new FileComparer(), new FileSystem(), this);
            }
            else
            {
                _needToCopyChecker = new NeedToCopyWithConfirmation(new FileSystem(), this);
            }
        }

        public bool CanReadFile(string aFile)
        {
            return _fileReadableChecker.CanReadFile(aFile);
        }

        public bool IgnoreFile(string file)
        {
            return _fileIgnoreChecker.IgnoreFile(file);
        }

        public bool NeedToCopy(string aSource, string aTarget)
        {
            return _needToCopyChecker.NeedToCopy(aSource, aTarget);
        }
    }
}
