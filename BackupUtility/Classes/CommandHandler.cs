using Microsoft.VisualBasic;
using System;
using GenericClassLibrary.FileSystem;

namespace ZCopy
{
    public class CommandHandler : IFileReadableChecker, IFileIgnoreChecker, INeedToCopyChecker
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

            ConfirmationChecker confirmationChecker = new ConfirmationChecker(commands.RequestConfirm);
            if (commands.UpdatedOnly)
            {
                _needToCopyChecker = new NeedToCopyUpdatedOnlyChecker(new FileComparer(), new FileSystem(), confirmationChecker);
            }
            else
            {
                throw new NotImplementedException("command to copy always not implemented");
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
