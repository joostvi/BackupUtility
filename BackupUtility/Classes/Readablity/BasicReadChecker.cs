using ZCopy.Interfaces;

namespace ZCopy.Classes.Readability
{
    public class BasicReadChecker : IFileReadableChecker
    {
        public bool CanReadFile(string aFile)
        {
            // Try if we can read the file
            if (string.IsNullOrEmpty(aFile))
                return false;
            return true;
        }
    }
}
