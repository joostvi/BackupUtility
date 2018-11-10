using System.Linq;
using System;
using System.IO;
using GenericClassLibrary.FileSystem;
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
            _exclusiveExt = exclusiveExt;
        }

        public bool IgnoreFile(string file)
        {
            try
            {
                if (_fileSystem.File.GetExtension(file).Length > 0)
                {
                    if (_exclusiveExt.Contains(_fileSystem.File.GetExtension(file).Substring(1)))
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
