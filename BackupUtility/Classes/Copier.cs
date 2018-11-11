﻿using System;
using GenericClassLibrary.FileSystem;
using System.IO;
using ZCopy.Interfaces;

namespace ZCopy.Classes
{
    public class Copier
    {
        private readonly Commands _commands;
        private readonly IFileSystem _fileSystem;
        private readonly ICommandHandler _commandHandler;

        public event ProcessInfoEventEventHandler ProcessInfoEvent;

        public delegate void ProcessInfoEventEventHandler(string theInfo);

        public Copier(Commands theCommands, ICommandHandler commandHandler)
        {
            _fileSystem = new FileSystem();
            // Just in case clown.
            _commands = theCommands.Clone();
            _commandHandler = commandHandler;
        }

        private void CopyFile(string aFile)
        {
            // Just an input check
            if (aFile == null || aFile == "")
                return;

            if (_commandHandler.IgnoreFile(aFile))
                return;

            string aTarget = aFile.Replace(_commands.Source, _commands.Target);

            bool doCopy = _commandHandler.NeedToCopy(_commands.Source, aTarget);

            if (doCopy)
            {
                ProcessInfoEvent?.Invoke("Copy File: " + aFile + " to " + aTarget);
                if (_commandHandler.CanReadFile(aFile))
                {
                    // Try if we can copy the file.
                    _commandHandler.CopyFile(aFile, aTarget);
                }
            }
        }

        private bool FolderExists(string aFolder)
        {
            // Check input
            if (_fileSystem.Directory.Exists(aFolder))
            {
                return true;
            }
            string msgText = "Source directory(" + aFolder + ") not found!";
            _commandHandler.HandleException(msgText, new DirectoryNotFoundException(msgText));
            return false;
        }

        private bool CreateTargetDirectory(string aFolder)
        {
            // OK copy files in this directory.
            // First check if target folder exists.
            string theTarget = aFolder.Replace(_commands.Source, _commands.Target);
            // if already exists we don't need to create it
            return _commandHandler.CreateDirectory(theTarget);
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
                    _commandHandler.HandleException($"Failed to read directories within directory {aFolder}", ex);
                }
                foreach (var aDirectory in directories)
                {
                    ThisDirectory(aDirectory);
                }
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

            if (!CreateTargetDirectory(aFolder))
                return;

            string[] theFiles;

            // Now copy files
            try
            {
                theFiles = _fileSystem.Directory.GetFiles(aFolder);
                foreach (string aFile in theFiles)
                {
                    CopyFile(aFile);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                _commandHandler.HandleException("Could not read directories of folder: " + aFolder + "(" + ex.Message + ")", ex);
            }
            ProcessSubfolders(aFolder);
        }

        public void Copy()
        {
            ThisDirectory(_commands.Source);
        }
    }
}
