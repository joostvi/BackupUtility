using System.Linq;
using System.Collections.Generic;
using System;

namespace ZCopy.Classes
{
    public class Commands
    {
        private class CommandStringComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                bool equal = false;

                if (x == null && y == null)
                    equal = true;
                else if (x == null)
                    equal = false;
                else if (y == null)
                    equal = false;
                else
                    equal = x.ToUpper() == y.ToUpper();
                return equal;
            }

            public int GetHashCode(string obj)
            {
                if (obj == null)
                    return "".GetHashCode();
                return obj.GetHashCode();
            }
        }

        private string FormatTarget(string thisTarget)
        {
            thisTarget = PathFormatter.FormatPath(thisTarget);
            if (!thisTarget.EndsWith("/") && !thisTarget.EndsWith(@"\"))
                thisTarget += @"\";
            return thisTarget;
        }

        public Commands(string[] args)
        {
            ShowHelp = false;
            Source = "";
            UpdatedOnly = false;
            RequestConfirm = true;
            SubFoldersAlso = false;
            SkipCopyErrors = false;
            PauseWhenDone = false;
            ExclusiveExt = new string[1];
            // If not arguments supplied or help is requested set showHelp to true and stop processing.
            if (args == null || args.Length < 2 || args.Contains("/?"))
                ShowHelp = true;
            else
            {
                // Check other parms 
                // 0 should be source
                Console.WriteLine("args(0)=" + args[0]);
                Source = PathFormatter.FormatPath(args[0]);
                Console.WriteLine("source=" + Source);
                // 1 should be targed
                Target = FormatTarget(args[1]);
                UpdatedOnly = args.Contains("/d", new CommandStringComparer());
                RequestConfirm = !args.Contains("/y", new CommandStringComparer());
                SubFoldersAlso = args.Contains("/s", new CommandStringComparer());
                SkipCopyErrors = args.Contains("/x", new CommandStringComparer());
                PauseWhenDone = args.Contains("/p", new CommandStringComparer());
                ReadCheckFirst = args.Contains("/r", new CommandStringComparer());

                foreach (string aCmd in args)
                {
                    if (aCmd.ToLower().StartsWith("/exclusiveext:"))
                    {
                        ExclusiveExt = aCmd.Split(':')[0].Split('+'); // string.Split(aCmd, ":")(1), "+");
                        break;
                    }
                }
            }
        }

        public static string Help()
        {
            string aStr;

            // TODO: Make language depending.
            Version ver = typeof(Commands).Assembly.GetName().Version;
            aStr = "Description of ZCopy version " + ver.ToString();
            aStr += "\r\n====================================================";
            aStr += "\r\nZCopy is an utility to copy files and directories. ";
            aStr += "\r\nThis tool is created as a helper because xcopy did not work for me!";
            aStr += "\r\nAs I wanted xcopy only to copy new or updated files and did failed for my Nas.";
            aStr += "\r\nAlso I wanted to do some practice in another style of programma as I'm processional used.";
            aStr += "\r\n\r\nParameters";
            aStr += "\r\n====================================================";
            aStr += "\r\nThis utility accepts the next parameters:";
            aStr += "\r\nThe source which to copy from this is mandetory and should be the first parameter.";
            aStr += "\r\nThe target which to copy to this is mandetory and should be second parameter.";
            aStr += "\r\n/d Copy only new or updated files.";
            aStr += "\r\n/y do not request for confirmation.";
            aStr += "\r\n/s copy subfolders also.";
            aStr += "\r\n/x skip errors. Program will continue with next directory or folder if an error occurs.";
            aStr += "\r\n/exclusiveExt: list separated by + of file extensions which should be skipped when performing the copy action.";
            aStr += "\r\n/? show this info file. When this parameter is given all others will be ignored.";
            aStr += "\r\n/p Pause when copy is ready.";
            aStr += "\r\n/r First check if we can read the file.";
            aStr += "\r\n\r\nSwitches may be in any order as long as they are not used as first or second parameter.";
            aStr += "\r\n\r\nExample:";
            aStr += "\r\n\r\n" + @"zcopy c:\tmp\ d:\tmp /d /y /s /x /exclusiveExt:bak+csv";
            return aStr;
        }

        private Commands(bool showHelp, string source, string target, bool updatedOnly, bool requestConfirm, bool subFoldersAlso, bool skipCopyErrors, string[] exclusiveExt, bool pauseWhenDone, bool doReadCheckFirst)
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

        public Commands Clone()
        {
            return new Commands(this.ShowHelp, this.Source, this.Target, this.UpdatedOnly, this.RequestConfirm, this.SubFoldersAlso, this.SkipCopyErrors, this.ExclusiveExt, this.PauseWhenDone, this.ReadCheckFirst);
        }
    }
}
