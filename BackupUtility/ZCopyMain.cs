﻿using GenericClassLibrary.Logging;
using System;
using ZCopy.Classes;

namespace ZCopy
{
    [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand, Unrestricted = true)]
    static class ZCopyMain
    {
        public static void Main(string[] args)
        {
            try
            {
                Logger.Level = EnumLogLevel.Info;
                Logger.AddLogger(new ConsoleLogger());
                Commands theCommands = new Commands(args);
                if (theCommands.ShowHelp)
                    Logger.Info(Commands.Help());
                else
                {
                    Logger.Info("Copy started.");
                    CommandHandler CommandHandler = new CommandHandler(theCommands);
                    Copier Copier = new Copier(theCommands, CommandHandler);
                    Copier.ProcessInfoEvent += ProcessInfo_ProcessInfoEvent;
                    CommandHandler.ProcessInfoEvent += ProcessInfo_ProcessInfoEvent;
                    CommandHandler.ConfirmationRequestHandler += ConfirmationRequest;
                    Copier.Copy();
                    if (theCommands.PauseWhenDone)
                        Logger.Info("Copy done.");
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

            Console.ReadKey();
        }

        private static bool ConfirmationRequest(string theInfo)
        {
            Console.WriteLine("File {0} already exists. Do you want to override the file (Y/N)?", theInfo);
            ConsoleKeyInfo aKey = Console.ReadKey();
            return aKey.KeyChar == 'Y' || aKey.KeyChar == 'y';
        }

        private static void ProcessInfo_ProcessInfoEvent(string theInfo)
        {
            Logger.Info(theInfo);
        }
    }
}
