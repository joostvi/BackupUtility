using ZCopy.Interfaces;

namespace ZCopy.Classes.FileIgnore
{
    public class IgnoreNoneChecker : IFileIgnoreChecker
    {
        public bool IgnoreFile(FolderMap baseMap, string file)
        {
            return false;
        }
    }
}
