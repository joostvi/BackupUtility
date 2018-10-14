Imports System.IO

Public Interface IFileComparer
    Function IsSameFile(ByVal aFile As FileInfo, ByVal aFile2 As FileInfo) As Boolean
    Function IsSamefile(ByVal file1 As String, ByVal file2 As String) As Boolean
End Interface
