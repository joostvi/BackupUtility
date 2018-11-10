namespace ZCopy.Interfaces
{
    public interface INeedToCopyChecker
    {
        bool NeedToCopy(string aSource, string aTarget);
    }
}
