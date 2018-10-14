Public Class BasicReadChecker
    Implements IFileReadableChecker

    Public Function CanReadFile(ByVal aFile As String) As Boolean Implements IFileReadableChecker.CanReadFile
        'Try if we can read the file
        If aFile Is Nothing OrElse aFile = "" Then Return False
        Return True
    End Function

End Class
