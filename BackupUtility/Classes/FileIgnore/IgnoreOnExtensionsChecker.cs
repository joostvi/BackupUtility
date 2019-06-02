using GenericClassLibrary.FileSystem;
using System;
using System.IO;
using System.Linq;
using ZCopy.Interfaces;

namespace ZCopy.Classes.FileIgnore
{
    public class IgnoreOnExtensionsChecker : IFileIgnoreChecker
    {
        private readonly IFileSystem _fileSystem;
        private readonly string[] _exclusiveExt;
        private readonly IExceptionHandler _exceptionHandler;

        public IgnoreOnExtensionsChecker(string[] exclusiveExt, IFileSystem fileSystem, IExceptionHandler exceptionHandler)
        {
            _fileSystem = fileSystem;
            _exceptionHandler = exceptionHandler;
            if (exclusiveExt != null)
            {
                _exclusiveExt = exclusiveExt.Select(a => a.ToLower()).ToArray();
            }
            else
            {
                _exclusiveExt = new string[0];
            }
        }

        public bool IgnoreFile(FolderMap baseMap, string file)
        {
            try
            {
                if (_exclusiveExt.Length == 0)
                {
                    return false;
                }
                string fileExtension = _fileSystem.File.GetExtension(file).ToLower();
                if (fileExtension.Length > 0 && _exclusiveExt.Contains(fileExtension.Substring(1)))
                {
                    return true;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                _exceptionHandler.HandleException("Copy File: " + file + " failed(" + ex.Message + ")", ex);
            }
            catch (IOException ex)
            {
                _exceptionHandler.HandleException("Copy File: " + file + " failed(" + ex.Message + ")", ex);
            }
            return false;
        }
    }
}
