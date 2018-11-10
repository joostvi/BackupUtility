Imports GenericClassLibrary.FileSystem
Imports System.IO

Public Class Copier

    Private _commands As Commands
    Private _ConfirmationRequest As ConfirmationRequest
    Private _fileSystem As IFileSystem
    Private _fileComparer As IFileComparer
    Private _copierComponents As CommandHandler

    'TODO: maybe we should use a class as parameter
    Public Delegate Function ConfirmationRequest(ByVal theInfo As String) As Boolean

    Public Event ProcessInfoEvent(ByVal theInfo As String)

    Public Sub New(ByVal theCommands As Commands, ByVal commandHandler As CommandHandler)
        _fileSystem = New FileSystem()
        _fileComparer = New FileComparer()
        'Just in case clown.
        _commands = theCommands.Clone
        _copierComponents = commandHandler
    End Sub

    Private Sub CopyFile(ByVal aFile As String)
        'Just an input check
        If aFile Is Nothing OrElse aFile = "" Then Return

        If _copierComponents.IgnoreFile(aFile) Then Return

        Dim aTarget As String = aFile.Replace(_commands.Source, _commands.Target)
        Dim doCopy As Boolean = False

        If _commands.UpdatedOnly Then
            If Not _fileSystem.File.Exists(aTarget) Then
                doCopy = True
            Else
                If ConfirmTheRequest(aTarget) Then
                    doCopy = Not _fileComparer.IsSameFile(New FileInfo(aFile), New FileInfo(aTarget))
                End If
            End If
        Else
            If ConfirmTheRequest(aTarget) Then
                doCopy = True
            End If
        End If

        If doCopy Then
            RaiseEvent ProcessInfoEvent("Copy File: " & aFile & " to " & aTarget)
            If _copierComponents.CanReadFile(aFile) Then
                'Try if we can copy the file.
                Try
                    _fileSystem.File.Copy(aFile, aTarget, True)
                Catch ex As UnauthorizedAccessException
                    If _commands.SkipCopyErrors Then
                        RaiseEvent ProcessInfoEvent("Failed to copy " & aFile & " to " & aTarget & " failed(" & ex.Message & ")")
                    Else
                        Throw ex
                    End If
                Catch ex As IOException
                    If _commands.SkipCopyErrors Then
                        RaiseEvent ProcessInfoEvent("Failed to copy " & aFile & " to " & aTarget & " failed(" & ex.Message & ")")
                    Else
                        Throw ex
                    End If
                End Try
            End If
        End If
    End Sub

    Private Function ConfirmTheRequest(aTarget As String) As Boolean
        If _commands.RequestConfirm AndAlso _ConfirmationRequest IsNot Nothing Then
            Return _ConfirmationRequest(aTarget)
        End If
        Return True
    End Function

    Private Function FolderExists(ByVal aFolder As String) As Boolean
        Dim exists As Boolean = False
        'Check input
        If Not _fileSystem.Directory.Exists(aFolder) Then
            Dim msgText As String = "Source directory(" & aFolder & ") not found!"
            If Not _commands.SkipCopyErrors Then
                Throw New DirectoryNotFoundException(msgText)
            Else
                'Directory not found and we don't want errors.
                RaiseEvent ProcessInfoEvent(msgText)
            End If
        Else
            exists = True
        End If
        Return exists
    End Function

    Private Function CreateTarget(ByVal aFolder As String) As Boolean
        Dim result As Boolean = False
        'OK copy files in this directory.
        'First check if target folder exists.
        Dim theTarget As String = aFolder.Replace(_commands.Source, _commands.Target)
        'if already exists we don't need to create it
        If Not _fileSystem.Directory.Exists(theTarget) Then
            'Target folder does not exist Create it
            RaiseEvent ProcessInfoEvent("Create directory: " & theTarget)
            Try
                _fileSystem.Directory.Create(theTarget)
                result = True
            Catch ex As DirectoryNotFoundException
                If Not _commands.SkipCopyErrors Then
                    'Don't want to skip errors.
                    Throw ex
                End If
                result = False
            End Try
        Else
            result = True
        End If
        Return result
    End Function

    Private Sub ProcessSubfolders(ByVal aFolder As String)

        'When we want the subfolders also we need to loop thru the sub directories.
        If _commands.SubFoldersAlso Then
            Dim directories(0) As String
            Try
                directories = _fileSystem.Directory.GetDirectories(aFolder)
            Catch ex As UnauthorizedAccessException
                If Not _commands.SkipCopyErrors Then
                    'Don't want to skip errors.
                    Throw ex
                End If
            End Try

            For Each aDirectory In directories
                ThisDirectory(aDirectory)
            Next
        End If
    End Sub

    Private Sub ThisDirectory(ByVal aFolder As String)

        If Not FolderExists(aFolder) Then Return

        'TODO make exclusion of folders optional
        If System.Text.RegularExpressions.Regex.IsMatch(aFolder, "(Temporary Internet Files)$") Then Return

        RaiseEvent ProcessInfoEvent("Process directory: " & aFolder)

        If Not CreateTarget(aFolder) Then Return

        Dim theFiles(0) As String

        'Now copy files
        Try
            theFiles = _fileSystem.Directory.GetFiles(aFolder)
        Catch ex As UnauthorizedAccessException
            If Not _commands.SkipCopyErrors Then
                'Don't want to skip errors.
                Throw ex
            Else
                RaiseEvent ProcessInfoEvent("Could not read directories of folder: " & aFolder & "(" & ex.Message & ")")
            End If
        End Try

        For Each aFile As String In theFiles
            CopyFile(aFile)
        Next
        ProcessSubfolders(aFolder)
    End Sub

    Public Sub Copy(ByVal aConfirmationRequest As ConfirmationRequest)
        _ConfirmationRequest = aConfirmationRequest
        ThisDirectory(_commands.Source)
    End Sub
End Class
