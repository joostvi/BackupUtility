using GenericClassLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZCopy.Classes
{
    public static class CommandLineArgumentParser
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

        private static string FormatTarget(string thisTarget)
        {
            thisTarget = PathFormatter.FormatPath(thisTarget);
            if (!thisTarget.EndsWith("/") && !thisTarget.EndsWith(@"\"))
                thisTarget += @"\";
            return thisTarget;
        }

        private static Dictionary<EnumLogLevel, string> LogLevels()
        {
            var levels = Enum.GetValues(typeof(EnumLogLevel));
            Dictionary<EnumLogLevel, string> values = new Dictionary<EnumLogLevel, string>();
            foreach (int level in levels)
            {
                EnumLogLevel value = (EnumLogLevel)level;
                values.Add(value, value.ToString());
            }
            return values;
        }

        private static string LogLevelsToString()
        {
            var logLevelDict = LogLevels();
            string valueList = "";
            foreach (KeyValuePair<EnumLogLevel, string> value in logLevelDict)
            {
                valueList += $", {value.Value}";
            }
            return valueList.Substring(2);
        }

        private static EnumLogLevel GetLogLevel(string[] args)
        {
            var levels = LogLevels();
            foreach (KeyValuePair<EnumLogLevel, string> value in levels)
            {
                if (args.Contains("/loglevel:" + value.Value, new CommandStringComparer()))
                {
                    return value.Key;
                }
            }
            return EnumLogLevel.Info;
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
            aStr += "\r\n/loglevel:{value} Level of details logged. Default info. Possible values: " + LogLevelsToString();
            aStr += "\r\n\r\nSwitches may be in any order as long as they are not used as first or second parameter.";
            aStr += "\r\n\r\nExample:";
            aStr += "\r\n\r\n" + @"zcopy c:\tmp\ d:\tmp /d /y /s /x /exclusiveExt:bak+csv";
            return aStr;
        }

        public static Commands ParseArgs(string[] args)
        {
            bool showHelp = false;
            string source = "";
            bool updatedOnly = false;
            bool requestConfirm = true;
            bool subFoldersAlso = false;
            bool skipCopyErrors = false;
            bool pauseWhenDone = false;
            string[] exclusiveExt = new string[1];
            bool readCheckFirst = false;
            string target = string.Empty;
            EnumLogLevel logLevel = EnumLogLevel.None;
            // If not arguments supplied or help is requested set showHelp to true and stop processing.
            if (args == null || args.Length < 2 || args.Contains("/?"))
            {
                showHelp = true;
            }
            else
            {
                // Check other parms 
                // 0 should be source
                Logger.Info("args(0)=" + args[0]);
                source = PathFormatter.FormatPath(args[0]);
                Logger.Debug("source=" + source);
                // 1 should be targed
                target = FormatTarget(args[1]);
                updatedOnly = args.Contains("/d", new CommandStringComparer());
                requestConfirm = !args.Contains("/y", new CommandStringComparer());
                subFoldersAlso = args.Contains("/s", new CommandStringComparer());
                skipCopyErrors = args.Contains("/x", new CommandStringComparer());
                pauseWhenDone = args.Contains("/p", new CommandStringComparer());
                readCheckFirst = args.Contains("/r", new CommandStringComparer());
                logLevel = GetLogLevel(args);
                Logger.Info("Loglevel = " + logLevel.ToString());
                foreach (string aCmd in args)
                {
                    if (aCmd.ToLower().StartsWith("/exclusiveext:"))
                    {
                        exclusiveExt = aCmd.Split(':')[0].Split('+');
                        break;
                    }
                }
            }
            FolderMap folderMap = new FolderMap(source, target, updatedOnly, subFoldersAlso, exclusiveExt);
            List<FolderMap> folderMaps = new List<FolderMap>() { folderMap };
            return new Commands(showHelp, folderMaps, requestConfirm, skipCopyErrors, pauseWhenDone, readCheckFirst, logLevel);
        }
    }
}
