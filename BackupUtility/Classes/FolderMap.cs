namespace ZCopy.Classes
{
    public class FolderMap
    {
        public string Source { get; }

        public string Target { get; }

        public bool SubFoldersAlso { get; }

        public bool UpdatedOnly { get; }

        public string[] ExclusiveExt { get; }

        public FolderMap(string source, string target, bool updatedOnly, bool subFoldersAlso, string[] exclusiveExt)
        {
            Source = source;
            Target = target;
            UpdatedOnly = updatedOnly;
            SubFoldersAlso = subFoldersAlso;
            ExclusiveExt = exclusiveExt;
        }
        public FolderMap Clone()
        {
            return new FolderMap(Source, Target, UpdatedOnly, SubFoldersAlso, ExclusiveExt);
        }
    }
}
