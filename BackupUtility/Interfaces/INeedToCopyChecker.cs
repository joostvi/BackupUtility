namespace ZCopy
{
    public interface INeedToCopyChecker
    {
        bool NeedToCopy(string aSource, string aTarget);
    }
}
