using ZCopy.Classes;

namespace ZCopy.Interfaces
{
    public interface INeedToCopyChecker
    {
        bool NeedToCopy(FolderMap baseMap, string aSource, string aTarget);
    }
}
