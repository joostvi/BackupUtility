namespace ZCopy
{
    public class BasicReadChecker : IFileReadableChecker
    {
        public bool CanReadFile(string aFile)
        {
            // Try if we can read the file
            if (aFile == null || aFile == "")
                return false;
            return true;
        }
    }
}
