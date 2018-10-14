Imports GenericClassLibrary.FileSystem
Imports ZCopy

Public Class CommandHandler
    Implements IFileReadableChecker, IFileIgnoreChecker

    Private ReadOnly _commands As Commands

    Private ReadOnly FileReadableChecker As IFileReadableChecker
    Private ReadOnly ExceptionHandler As IExceptionHandler
    Private ReadOnly _fileIgnoreChecker As IFileIgnoreChecker

    Public Event ProcessInfoEvent(ByVal theInfo As String)

    Public Sub RaiseThisEvent(ByVal theInfo As String)
        RaiseEvent ProcessInfoEvent(theInfo)
    End Sub

    Public Sub New(commands As Commands)
        _commands = commands

        Dim fileSystem As IFileSystem = New FileSystem()
        If _commands.SkipCopyErrors Then
            ExceptionHandler = New ContinueOnExceptionHandler(Me)
        Else
            ExceptionHandler = New StopOnExceptionHandler()
        End If

        If _commands.ReadCheckFirst Then
            FileReadableChecker = New FullReadChecker(ExceptionHandler)
        Else
            FileReadableChecker = New BasicReadChecker()
        End If
        If _commands.ExclusiveExt.Length > 0 Then
            _fileIgnoreChecker = New IgnoreOnExtensionsChecker(_commands.ExclusiveExt, fileSystem, ExceptionHandler)
        Else
            _fileIgnoreChecker = New IgnoreNoneChecker()
        End If
    End Sub

    Public Function CanReadFile(aFile As String) As Boolean Implements IFileReadableChecker.CanReadFile
        FileReadableChecker.CanReadFile(aFile)
    End Function

    Public Function IgnoreFile(file As String) As Boolean Implements IFileIgnoreChecker.IgnoreFile
        _fileIgnoreChecker.IgnoreFile(file)
    End Function
End Class
