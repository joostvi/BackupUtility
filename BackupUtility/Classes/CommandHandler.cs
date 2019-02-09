using GenericClassLibrary.FileSystem;
using System;
using System.IO;
using ZCopy.Classes.ExceptionHandling;
using ZCopy.Classes.FileIgnore;
using ZCopy.Classes.NeedToCopy;
using ZCopy.Classes.Readability;
using ZCopy.Interfaces;

namespace ZCopy.Classes
{
    public class CommandHandler : ICommandHandler
    {

        private readonly IFileReadableChecker _fileReadableChecker;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IFileIgnoreChecker _fileIgnoreChecker;
        private readonly INeedToCopyChecker _needToCopyChecker;
        private readonly IFileSystem _fileSystem;

        public event ProcessInfoEventEventHandler ProcessInfoEvent;

        public delegate void ProcessInfoEventEventHandler(object sender, ProcessInfoEventArgs eventArgs);

        public delegate bool ConfirmationRequest(string theInfo);

        public event ConfirmationRequest ConfirmationRequestHandler;

        public bool GetConfirmation(string aTarget)
        {
            if (ConfirmationRequestHandler != null)
                return ConfirmationRequestHandler.Invoke(aTarget);
            return true;
        }

        public CommandHandler(Commands commands)
        {
            // _commands = commands

            _fileSystem = new FileSystem();
            if (commands.SkipCopyErrors)
                _exceptionHandler = new ContinueOnExceptionHandler(this);
            else
                _exceptionHandler = new StopOnExceptionHandler();

            if (commands.ReadCheckFirst)
                _fileReadableChecker = new FullReadChecker(_exceptionHandler);
            else
                _fileReadableChecker = new BasicReadChecker();
            if (commands.ExclusiveExt.Length > 0)
                _fileIgnoreChecker = new IgnoreOnExtensionsChecker(commands.ExclusiveExt, _fileSystem, _exceptionHandler);
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

        public void LogEvent(string message)
        {
            ProcessInfoEvent?.Invoke(this, new ProcessInfoEventArgs(message));
        }

        public void CopyFile(string aFile, string aTarget)
        {
            try
            {
                _fileSystem.File.Copy(aFile, aTarget, true);
            }
            catch (UnauthorizedAccessException ex)
            {
                _exceptionHandler.HandleException("Failed to copy " + aFile + " to " + aTarget + " failed(" + ex.Message + ")", ex);
            }
            catch (IOException ex)
            {
                _exceptionHandler.HandleException("Failed to copy " + aFile + " to " + aTarget + " failed(" + ex.Message + ")", ex);
            }
        }

        public bool CreateDirectory(string theTarget)
        {
            if (_fileSystem.Directory.Exists(theTarget))
            {
                return true;
            }

            bool result = false;
            // Target folder does not exist Create it
            ProcessInfoEvent?.Invoke(this, new ProcessInfoEventArgs( "Create directory: " + theTarget));
            try
            {
                _fileSystem.Directory.Create(theTarget);
                result = true;
            }
            catch (DirectoryNotFoundException ex)
            {
                _exceptionHandler.HandleException("Failed to create directory " + theTarget + "(" + ex.Message + ")", ex);
                result = false;
            }
            return result;
        }

        public void HandleException(string message, Exception ex)
        {
            _exceptionHandler.HandleException(message, ex);
        }
    }
}
