using GenericClassLibrary.FileSystem;
using GenericClassLibrary.Logging;
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
        private readonly IFileSystem _fileSystem;
        private readonly Commands _commands;

        public event ProcessInfoEventEventHandler ProcessInfoEvent;

        public delegate void ProcessInfoEventEventHandler(object sender, ProcessInfoEventArgs eventArgs);

        public delegate bool ConfirmationRequest(string theInfo);

        public event ConfirmationRequest ConfirmationRequestHandler;

        public bool GetConfirmation(string aTarget)
        {
            if (_commands.RequestConfirm && ConfirmationRequestHandler != null)
                return ConfirmationRequestHandler.Invoke(aTarget);
            return true;
        }

        public CommandHandler(Commands commands)
        {
            _commands = commands;
            _fileSystem = new FileSystem();
            if (commands.SkipCopyErrors)
                _exceptionHandler = new ContinueOnExceptionHandler(this);
            else
                _exceptionHandler = new StopOnExceptionHandler();

            if (commands.ReadCheckFirst)
                _fileReadableChecker = new FullReadChecker(_exceptionHandler);
            else
                _fileReadableChecker = new BasicReadChecker();
        }

        public bool CanReadFile(string aFile)
        {
            return _fileReadableChecker.CanReadFile(aFile);
        }

        public bool IgnoreFile(FolderMap baseMap, string file)
        {
            IFileIgnoreChecker fileIgnoreChecker;
            if (baseMap.ExclusiveExt.Length > 0)
                fileIgnoreChecker = new IgnoreOnExtensionsChecker(baseMap.ExclusiveExt, _fileSystem, _exceptionHandler);
            else
                fileIgnoreChecker = new IgnoreNoneChecker();
            return fileIgnoreChecker.IgnoreFile(baseMap, file);
        }

        public bool NeedToCopy(FolderMap baseMap, string aSource, string aTarget)
        {
            Logger.Debug($"baseMap.UpdatedOnly = {baseMap.UpdatedOnly}");
            INeedToCopyChecker needToCopyChecker;
            if (baseMap.UpdatedOnly)
            {
                needToCopyChecker = new NeedToCopyUpdatedOnlyChecker(new FileComparer(), new FileSystem(), this);
            }
            else
            {
                needToCopyChecker = new NeedToCopyWithConfirmation(new FileSystem(), this);
            }
            return needToCopyChecker.NeedToCopy(baseMap, aSource, aTarget);
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
            ProcessInfoEvent?.Invoke(this, new ProcessInfoEventArgs("Create directory: " + theTarget));
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
