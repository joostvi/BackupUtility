using ZCopy.Classes;

namespace ZCopy.Interfaces
{
    public interface IFileIgnoreChecker
    {
        bool IgnoreFile(FolderMap baseMap, string file);
    }
}
