Imports ZCopy

Public Class IgnoreNoneChecker
    Implements IFileIgnoreChecker

    Public Function IgnoreFile(file As String) As Boolean Implements IFileIgnoreChecker.IgnoreFile
        Return False
    End Function
End Class
