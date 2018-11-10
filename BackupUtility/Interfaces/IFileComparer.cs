using System.IO;

namespace ZCopy.Interfaces
{
    public interface IFileComparer
    {
        bool IsSameFile(FileInfo aFile, FileInfo aFile2);
        bool IsSameFile(string file1, string file2);
    }
}
