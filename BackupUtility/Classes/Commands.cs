using GenericClassLibrary.Logging;
using System.Collections.Generic;

namespace ZCopy.Classes
{
    public class Commands
    {
        public Commands(bool showHelp, List<FolderMap> folders, bool requestConfirm, bool skipCopyErrors, bool pauseWhenDone, bool doReadCheckFirst, EnumLogLevel logLevel)
        {
            ShowHelp = showHelp;
            RequestConfirm = requestConfirm;
            SkipCopyErrors = skipCopyErrors;
            PauseWhenDone = pauseWhenDone;
            ReadCheckFirst = doReadCheckFirst;
            LogLevel = logLevel;
            Folders = folders;
        }

        public List<FolderMap> Folders { get; }
        public bool ShowHelp { get; }

        public bool RequestConfirm { get; }

        public bool SkipCopyErrors { get; }

        public bool PauseWhenDone { get; }

        public bool ReadCheckFirst { get; }

        public EnumLogLevel LogLevel { get; }

        public Commands Clone()
        {
            List<FolderMap> folders = new List<FolderMap>();
            foreach(FolderMap folder in Folders)
            {
                folders.Add(folder.Clone());
            }
            return new Commands(this.ShowHelp, folders, this.RequestConfirm, this.SkipCopyErrors, this.PauseWhenDone, this.ReadCheckFirst, this.LogLevel);
        }
    }
}
