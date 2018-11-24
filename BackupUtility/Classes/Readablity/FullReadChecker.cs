using System;
using System.IO;
using ZCopy.Interfaces;

namespace ZCopy.Classes.Readability
{
    public class FullReadChecker : IFileReadableChecker
    {
        private readonly IExceptionHandler _exceptionHandler;

        public FullReadChecker(IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        public bool CanReadFile(string aFile)
        {

            // Try if we can read the file
            bool canRead = false;
            if (aFile == null || aFile == "")
                return false;

            FileStream aStream = null;
            BinaryReader aBR;
            try
            {
                aStream = new FileStream(aFile, FileMode.Open, FileAccess.Read);
                aBR = new BinaryReader(aStream);
                long length = aStream.Length;
                // Hope this is faster and less error prone as readallbytes
                while (length > 0)
                {
                    int readBytes;

                    if (length > int.MaxValue)
                        readBytes = int.MaxValue;
                    else
                        readBytes = Convert.ToInt32(length);
                    aBR.ReadBytes(readBytes);
                    length -= readBytes;
                }
                canRead = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                _exceptionHandler.HandleException("Failed to read " + aFile + "(" + ex.Message + ")", ex);
            }
            catch (IOException ex)
            {
                _exceptionHandler.HandleException("Failed to read " + aFile + "(" + ex.Message + ")", ex);
            }
            catch (OutOfMemoryException ex)
            {
                _exceptionHandler.HandleException("Failed to read " + aFile + "(" + ex.Message + ")", ex);
            }
            finally
            {
                if (aStream != null)
                {
                    aStream.Close();
                    aStream.Dispose();
                }
            }
            return canRead;
        }
    }
}
