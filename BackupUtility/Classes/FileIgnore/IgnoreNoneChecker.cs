using ZCopy.Interfaces;

namespace ZCopy.Classes.FileIgnore
{
    public class IgnoreNoneChecker : IFileIgnoreChecker
    {
        public bool IgnoreFile(string file)
        {
            return false;
        }
    }
}
