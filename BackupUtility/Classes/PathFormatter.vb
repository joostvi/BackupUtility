Public Class PathFormatter

    Public Shared Function FormatPath(ByVal thisPath As String) As String
        If thisPath.EndsWith("""") Then
            thisPath = thisPath.Substring(0, thisPath.Length - 1)
        End If
        Return thisPath
    End Function

End Class
