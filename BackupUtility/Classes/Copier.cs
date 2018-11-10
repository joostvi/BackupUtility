using Microsoft.VisualBasic;
using System;
using GenericClassLibrary.FileSystem;
using System.IO;

namespace ZCopy
{
    public class Copier
    {
        private Commands _commands;
        private ConfirmationRequest _ConfirmationRequest;
        private IFileSystem _fileSystem;
        private IFileComparer _fileComparer;
        private CommandHandler _copierComponents;

        // TODO: maybe we should use a class as parameter
        public delegate bool ConfirmationRequest(string theInfo);

        public event ProcessInfoEventEventHandler ProcessInfoEvent;

        public delegate void ProcessInfoEventEventHandler(string theInfo);

        public Copier(Commands theCommands, CommandHandler commandHandler)
        {
            _fileSystem = new FileSystem();
            _fileComparer = new FileComparer();
            // Just in case clown.
            _commands = theCommands.Clone();
            _copierComponents = commandHandler;
        }

        private void CopyFile(string aFile)
        {
            // Just an input check
            if (aFile == null || aFile == "")
                return;

            if (_copierComponents.IgnoreFile(aFile))
                return;

            string aTarget = aFile.Replace(_commands.Source, _commands.Target);
            bool doCopy = false;

            if (_commands.UpdatedOnly)
            {
                if (!_fileSystem.File.Exists(aTarget))
                    doCopy = true;
                else if (ConfirmTheRequest(aTarget))
                    doCopy = !_fileComparer.IsSameFile(new FileInfo(aFile), new FileInfo(aTarget));
            }
            else if (ConfirmTheRequest(aTarget))
                doCopy = true;

            if (doCopy)
            {
                ProcessInfoEvent?.Invoke("Copy File: " + aFile + " to " + aTarget);
                if (_copierComponents.CanReadFile(aFile))
                {
                    // Try if we can copy the file.
                    try
                    {
                        _fileSystem.File.Copy(aFile, aTarget, true);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        if (_commands.SkipCopyErrors)
                            ProcessInfoEvent?.Invoke("Failed to copy " + aFile + " to " + aTarget + " failed(" + ex.Message + ")");
                        else
                            throw ex;
                    }
                    catch (IOException ex)
                    {
                        if (_commands.SkipCopyErrors)
                            ProcessInfoEvent?.Invoke("Failed to copy " + aFile + " to " + aTarget + " failed(" + ex.Message + ")");
                        else
                            throw ex;
                    }
                }
            }
        }

        private bool ConfirmTheRequest(string aTarget)
        {
            if (_commands.RequestConfirm && _ConfirmationRequest != null)
                return _ConfirmationRequest(aTarget);
            return true;
        }

        private bool FolderExists(string aFolder)
        {
            bool exists = false;
            // Check input
            if (!_fileSystem.Directory.Exists(aFolder))
            {
                string msgText = "Source directory(" + aFolder + ") not found!";
                if (!_commands.SkipCopyErrors)
                    throw new DirectoryNotFoundException(msgText);
                else
                    // Directory not found and we don't want errors.
                    ProcessInfoEvent?.Invoke(msgText);
            }
            else
                exists = true;
            return exists;
        }

        private bool CreateTarget(string aFolder)
        {
            bool result = false;
            // OK copy files in this directory.
            // First check if target folder exists.
            string theTarget = aFolder.Replace(_commands.Source, _commands.Target);
            // if already exists we don't need to create it
            if (!_fileSystem.Directory.Exists(theTarget))
            {
                // Target folder does not exist Create it
                ProcessInfoEvent?.Invoke("Create directory: " + theTarget);
                try
                {
                    _fileSystem.Directory.Create(theTarget);
                    result = true;
                }
                catch (DirectoryNotFoundException ex)
                {
                    if (!_commands.SkipCopyErrors)
                        // Don't want to skip errors.
                        throw ex;
                    result = false;
                }
            }
            else
                result = true;
            return result;
        }

        private void ProcessSubfolders(string aFolder)
        {

            // When we want the subfolders also we need to loop thru the sub directories.
            if (_commands.SubFoldersAlso)
            {
                string[] directories = new string[1];
                try
                {
                    directories = _fileSystem.Directory.GetDirectories(aFolder);
                }
                catch (UnauthorizedAccessException ex)
                {
                    if (!_commands.SkipCopyErrors)
                        // Don't want to skip errors.
                        throw ex;
                }

                foreach (var aDirectory in directories)
                    ThisDirectory(aDirectory);
            }
        }

        private void ThisDirectory(string aFolder)
        {
            if (!FolderExists(aFolder))
                return;

            // TODO make exclusion of folders optional
            if (System.Text.RegularExpressions.Regex.IsMatch(aFolder, "(Temporary Internet Files)$"))
                return;

            ProcessInfoEvent?.Invoke("Process directory: " + aFolder);

            if (!CreateTarget(aFolder))
                return;

            string[] theFiles = new string[1];

            // Now copy files
            try
            {
                theFiles = _fileSystem.Directory.GetFiles(aFolder);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (!_commands.SkipCopyErrors)
                    // Don't want to skip errors.
                    throw ex;
                else
                    ProcessInfoEvent?.Invoke("Could not read directories of folder: " + aFolder + "(" + ex.Message + ")");
            }

            foreach (string aFile in theFiles)
                CopyFile(aFile);
            ProcessSubfolders(aFolder);
        }

        public void Copy(ConfirmationRequest aConfirmationRequest)
        {
            _ConfirmationRequest = aConfirmationRequest;
            ThisDirectory(_commands.Source);
        }
    }
}
