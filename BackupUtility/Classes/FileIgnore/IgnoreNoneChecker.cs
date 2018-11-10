namespace ZCopy
{
    public class IgnoreNoneChecker : IFileIgnoreChecker
    {
        public bool IgnoreFile(string file)
        {
            return false;
        }
    }
}
