'TODO request administator rights
'TODO Possibilty to skip folders
'TODO Possibilty to give file format *.bla
'TODO Now failing when path does not end on \ or /
<Security.Permissions.SecurityPermission(Security.Permissions.SecurityAction.Demand, Unrestricted:=True)>
Module ZCopyMain


    Sub Main(ByVal args() As String)

        Try
            Dim theCommands As New Commands(args)
            If theCommands.ShowHelp Then
                Console.WriteLine(Commands.Help)
            Else
                Console.WriteLine("Copy started.")
                Dim CommandHandler As New CommandHandler(theCommands)
                Dim Copier As New Copier(theCommands, CommandHandler)
                AddHandler Copier.ProcessInfoEvent, AddressOf ProcessInfo_ProcessInfoEvent
                AddHandler CommandHandler.ProcessInfoEvent, AddressOf ProcessInfo_ProcessInfoEvent
                Copier.Copy(AddressOf ConfirmationRequest)
                If theCommands.PauseWhenDone Then Console.WriteLine("Copy done.")
            End If
        Catch ex As IO.DirectoryNotFoundException
            Console.WriteLine(ex.Message)
        Catch ex As UnauthorizedAccessException
            Console.WriteLine(ex.Message)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.WriteLine(ex.StackTrace)
        End Try



        Console.ReadKey()
    End Sub

    Private Function ConfirmationRequest(ByVal theInfo As String) As Boolean
        Console.WriteLine("File {0} already exists. Do you want to override the file (Y/N)?", theInfo)
        Dim aKey As ConsoleKeyInfo = Console.ReadKey()
        Return UCase(aKey.KeyChar) = "Y"
    End Function

    Private Sub ProcessInfo_ProcessInfoEvent(ByVal theInfo As String)
        Console.WriteLine(theInfo)
    End Sub
End Module
