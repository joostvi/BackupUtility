using System;
using System.IO;

namespace ZCopy
{
    public class FullReadChecker : IFileReadableChecker
    {
        private IExceptionHandler _exceptionHandler;

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
                        readBytes = System.Convert.ToInt32(length);
                    aBR.ReadBytes(readBytes);
                    length -= readBytes;
                }

                // Dim aByteArry() As Byte = File.ReadAllBytes(aFile)
                canRead = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                // If _commands.SkipCopyErrors Then
                // RaiseEvent ProcessInfoEvent()
                // Else
                // Throw ex
                // End If
                _exceptionHandler.HandleException("Failed to read " + aFile + "(" + ex.Message + ")", ex);
            }
            catch (IOException ex)
            {
                // If _commands.SkipCopyErrors Then
                // RaiseEvent ProcessInfoEvent("Failed to read " & aFile & "(" & ex.Message & ")")
                // Else
                // Throw ex
                // End If

                _exceptionHandler.HandleException("Failed to read " + aFile + "(" + ex.Message + ")", ex);
            }
            catch (OutOfMemoryException ex)
            {
                // not enough memory to read file hopefully we can copy it. 
                // TODO: might think about copy with other name or better check for read.
                canRead = true;
            }
            finally
            {
                if (aStream != null)
                {
                    aStream.Close();
                    aStream.Dispose();
                    aStream = null;
                }
            }

            return canRead;
        }
    }
}
