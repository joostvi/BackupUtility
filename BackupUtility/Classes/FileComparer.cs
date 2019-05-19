using GenericClassLibrary.Logging;
using System.IO;
using ZCopy.Interfaces;

namespace ZCopy.Classes
{
    public class FileComparer : IFileComparer
    {
        public bool IsSameFile(FileInfo aFile, FileInfo aFile2)
        {
            bool sameFile = false;

            if (aFile == null && aFile2 == null)
            {
                sameFile = true;
            }
            else if (aFile == null || aFile2 == null)
            {
                // if one is nothing then the other is not.
                sameFile = false;
            }
            else
            {    // we have to look into the file details
                 // Cannot use creation time as this will be changed during backup.
                 // TimeSpan diffResult = aFile.LastWriteTimeUtc.Subtract(aFile2.LastWriteTimeUtc);
                //var hashCode1 = aFile.GetHashCode();
                //var hashCode2 = aFile2.GetHashCode();
                Logger.Debug($"FileComparer: length 1: {aFile.Length}, length 2: {aFile2.Length}, name 1: {aFile.Name}, name 2: {aFile2.Name}");
                if (aFile.Length == aFile2.Length && aFile.Name == aFile2.Name)
                {
                    sameFile = true;
                }
            }
            return sameFile;
        }


        // This method accepts two strings the represent two files to 
        // compare. A return value of 0 indicates that the contents of the files
        // are the same. A return value of any other value indicates that the 
        // files are not the same.
        public bool IsSameFile(string file1, string file2)
        {
            // ' Determine if the same file was referenced two times.
            if ((file1 == file2))
                // Return true to indicate that the files are the same.
                return true;

            // Open the two files.
            FileStream fs1 = new FileStream(file1, FileMode.Open);
            FileStream fs2 = new FileStream(file2, FileMode.Open);

            bool sameFile = false;
            // Check the file sizes. If they are not the same, the files are not the same.
            if (fs1.Length == fs2.Length)
            {
                int file1byte;
                int file2byte;
                // Read and compare a byte from each file until either a
                // non-matching set of bytes is found or until the end of
                // file1 is reached.
                do
                {
                    // Read one byte from each file.
                    file1byte = fs1.ReadByte();
                    file2byte = fs2.ReadByte();
                }
                while (((file1byte == file2byte) && file1byte != -1 && file2byte != -1));

                sameFile = (file1byte == file2byte);
            }
            // Close the files.
            fs1.Close();
            fs2.Close();
            fs1.Dispose();
            fs2.Dispose();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return sameFile;
        }
    }
}
