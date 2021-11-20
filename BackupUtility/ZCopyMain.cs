using GenericClassLibrary.Logging;
using System;
using ZCopy.Classes;

namespace ZCopy
{
    static class ZCopyMain
    {
        public static void Main(string[] args)
        {
            try
            {
                Logger.Level = EnumLogLevel.Info;
                Logger.AddLogger(new ConsoleLogger());

                Commands theCommands = CommandLineArgumentParser.ParseArgs(args);
                if (theCommands.ShowHelp)
                    Logger.Info(CommandLineArgumentParser.Help());
                else
                {
                    Logger.Level = theCommands.LogLevel;
                    Logger.Info("Copy started.");
                    CommandHandler CommandHandler = new CommandHandler(theCommands);
                    Copier Copier = new Copier(theCommands, CommandHandler);
                    Copier.ProcessInfoEvent += ProcessInfo_ProcessInfoEvent;
                    CommandHandler.ProcessInfoEvent += ProcessInfo_ProcessInfoEvent;
                    CommandHandler.ConfirmationRequestHandler += ConfirmationRequest;
                    Copier.Copy();
                    Logger.Info("Copy done.");
                    if (theCommands.PauseWhenDone)
                    {
                        Console.ReadKey();
                    }                        
                }
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                Logger.Error(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error("Failed with an unexpected exception.", ex);
            }
           
        }

        private static bool ConfirmationRequest(string theInfo)
        {
            Console.WriteLine("File {0} already exists. Do you want to override the file (Y/N)?", theInfo);
            ConsoleKeyInfo aKey = Console.ReadKey();
            return aKey.KeyChar == 'Y' || aKey.KeyChar == 'y';
        }

        private static void ProcessInfo_ProcessInfoEvent(object sender, ProcessInfoEventArgs eventArgs)
        {
            Logger.Info(eventArgs.Info);
        }
    }
}
