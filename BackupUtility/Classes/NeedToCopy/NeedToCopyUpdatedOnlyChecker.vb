Imports System.IO
Imports GenericClassLibrary.FileSystem

Public Class NeedToCopyUpdatedOnlyChecker
    Implements INeedToCopyChecker

    Private ReadOnly _fileComparer As IFileComparer
    Private ReadOnly _fileSystem As IFileSystem
    Private ReadOnly _confirmationChecker As IConfirmationChecker


    Public Sub New(fileComparer As IFileComparer, fileSystem As IFileSystem, confirmationChecker As IConfirmationChecker)
        _fileComparer = fileComparer
        _confirmationChecker = confirmationChecker
        _fileSystem = fileSystem
    End Sub

    Public Function NeedToCopy(aSource As String, aTarget As String) As Boolean Implements INeedToCopyChecker.NeedToCopy
        Dim doCopy As Boolean
        If Not _fileSystem.File.Exists(aTarget) Then
            doCopy = True
        Else
            If _confirmationChecker.GetConfirmation(aTarget) Then
                doCopy = Not _fileComparer.IsSameFile(New FileInfo(aSource), New FileInfo(aTarget))
            End If
        End If
        Return doCopy
    End Function
End Class
