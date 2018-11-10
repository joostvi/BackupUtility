namespace ZCopy.Classes
{
    public class PathFormatter
    {
        public static string FormatPath(string thisPath)
        {
            if (thisPath.EndsWith("\""))
                thisPath = thisPath.Substring(0, thisPath.Length - 1);
            return thisPath;
        }
    }
}
