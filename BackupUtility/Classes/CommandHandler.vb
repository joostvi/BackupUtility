Imports GenericClassLibrary.FileSystem

Public Class CommandHandler
    Implements IFileReadableChecker, IFileIgnoreChecker, INeedToCopyChecker

    'Private ReadOnly _commands As Commands

    Private ReadOnly _fileReadableChecker As IFileReadableChecker
    Private ReadOnly ExceptionHandler As IExceptionHandler
    Private ReadOnly _fileIgnoreChecker As IFileIgnoreChecker
    Private ReadOnly _needToCopyChecker As INeedToCopyChecker

    Public Event ProcessInfoEvent(ByVal theInfo As String)

    Public Sub RaiseThisEvent(ByVal theInfo As String)
        RaiseEvent ProcessInfoEvent(theInfo)
    End Sub

    Public Sub New(commands As Commands)
        '_commands = commands

        Dim fileSystem As IFileSystem = New FileSystem()
        If commands.SkipCopyErrors Then
            ExceptionHandler = New ContinueOnExceptionHandler(Me)
        Else
            ExceptionHandler = New StopOnExceptionHandler()
        End If

        If commands.ReadCheckFirst Then
            _fileReadableChecker = New FullReadChecker(ExceptionHandler)
        Else
            _fileReadableChecker = New BasicReadChecker()
        End If
        If commands.ExclusiveExt.Length > 0 Then
            _fileIgnoreChecker = New IgnoreOnExtensionsChecker(commands.ExclusiveExt, fileSystem, ExceptionHandler)
        Else
            _fileIgnoreChecker = New IgnoreNoneChecker()
        End If
    End Sub

    Public Function CanReadFile(aFile As String) As Boolean Implements IFileReadableChecker.CanReadFile
        _fileReadableChecker.CanReadFile(aFile)
    End Function

    Public Function IgnoreFile(file As String) As Boolean Implements IFileIgnoreChecker.IgnoreFile
        _fileIgnoreChecker.IgnoreFile(file)
    End Function

    Public Function NeedToCopy(aSource As String, aTarget As String) As Boolean Implements INeedToCopyChecker.NeedToCopy
        Throw New NotImplementedException()
    End Function
End Class
