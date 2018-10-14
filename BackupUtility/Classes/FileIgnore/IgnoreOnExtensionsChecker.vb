Imports System.IO
Imports GenericClassLibrary.FileSystem

Public Class IgnoreOnExtensionsChecker
    Implements IFileIgnoreChecker

    Private ReadOnly _fileSystem As IFileSystem
    Private ReadOnly _exclusiveExt As String()
    Private ReadOnly _exceptionHandler As IExceptionHandler

    Public Sub New(exclusiveExt As String(), fileSystem As IFileSystem, exceptionHandler As IExceptionHandler)
        _fileSystem = fileSystem
        _exceptionHandler = exceptionHandler
        _exclusiveExt = exclusiveExt
    End Sub

    Public Function IgnoreFile(file As String) As Boolean Implements IFileIgnoreChecker.IgnoreFile
        Try
            If _fileSystem.File.GetExtension(file).Length > 0 Then
                If _exclusiveExt.Contains(_fileSystem.File.GetExtension(file).Substring(1)) Then Return True
            End If
        Catch ex As UnauthorizedAccessException
            _exceptionHandler.HandleException("Copy File: " & file & " failed(" & ex.Message & ")", ex)
        Catch ex As IOException
            _exceptionHandler.HandleException("Copy File: " & file & " failed(" & ex.Message & ")", ex)
        End Try
        Return False
    End Function
End Class
