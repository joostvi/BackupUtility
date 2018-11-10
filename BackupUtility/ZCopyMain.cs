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
                Commands theCommands = new Commands(args);
                if (theCommands.ShowHelp)
                    Console.WriteLine(Commands.Help());
                else
                {
                    Console.WriteLine("Copy started.");
                    CommandHandler CommandHandler = new CommandHandler(theCommands);
                    Copier Copier = new Copier(theCommands, CommandHandler);
                    Copier.ProcessInfoEvent += ProcessInfo_ProcessInfoEvent;
                    CommandHandler.ProcessInfoEvent += ProcessInfo_ProcessInfoEvent;
                    Copier.Copy(ConfirmationRequest);
                    if (theCommands.PauseWhenDone)
                        Console.WriteLine("Copy done.");
                }
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
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
            Console.WriteLine(theInfo);
        }
    }
}
