using GenericClassLibrary.Logging;

namespace ZCopy.Classes
{

    public class Commands
    {      
        public Commands(bool showHelp, string source, string target, bool updatedOnly, bool requestConfirm, bool subFoldersAlso, bool skipCopyErrors, string[] exclusiveExt, bool pauseWhenDone, bool doReadCheckFirst, EnumLogLevel logLevel)
        {
            ShowHelp = showHelp;
            Source = source;
            Target = target;
            UpdatedOnly = updatedOnly;
            RequestConfirm = requestConfirm;
            SubFoldersAlso = subFoldersAlso;
            SkipCopyErrors = skipCopyErrors;
            ExclusiveExt = exclusiveExt;
            PauseWhenDone = pauseWhenDone;
            ReadCheckFirst = doReadCheckFirst;
            LogLevel = logLevel;
        }

        public bool ShowHelp { get; }

        public string Source { get; }

        public string Target { get; }

        public bool UpdatedOnly { get; }

        public bool RequestConfirm { get; }

        public bool SubFoldersAlso { get; }

        public bool SkipCopyErrors { get; }

        public string[] ExclusiveExt { get; }

        public bool PauseWhenDone { get; }

        public bool ReadCheckFirst { get; }

        public EnumLogLevel LogLevel { get; }

        public Commands Clone()
        {
            return new Commands(this.ShowHelp, this.Source, this.Target, this.UpdatedOnly, this.RequestConfirm, this.SubFoldersAlso, this.SkipCopyErrors, this.ExclusiveExt, this.PauseWhenDone, this.ReadCheckFirst, this.LogLevel);
        }
    }
}
