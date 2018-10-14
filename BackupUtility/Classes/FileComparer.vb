Imports System.IO
Imports ZCopy

Public Class FileComparer
    Implements IFileComparer

    Public Function IsSameFile(ByVal aFile As FileInfo, ByVal aFile2 As FileInfo) As Boolean Implements IFileComparer.IsSameFile
        Dim sameFile As Boolean

        If aFile Is Nothing AndAlso aFile2 Is Nothing Then
            sameFile = True
        ElseIf (aFile Is Nothing AndAlso aFile2 IsNot Nothing) _
           OrElse (aFile IsNot Nothing AndAlso aFile2 Is Nothing) Then
            'if one is nothing then the other is not.
            sameFile = False
        Else
            'we have to look into the file details
            'Cannot use creation time as this will be changed during backup.
            If DateDiff(DateInterval.Second, aFile.LastWriteTimeUtc, aFile2.LastWriteTimeUtc) = 0 _
                AndAlso aFile.Length = aFile2.Length _
                AndAlso aFile.Name = aFile2.Name Then
                sameFile = True
            End If
        End If

        Return sameFile
    End Function


    'This method accepts two strings the represent two files to 
    ' compare. A return value of 0 indicates that the contents of the files
    ' are the same. A return value of any other value indicates that the 
    ' files are not the same.
    Public Function IsSamefile(ByVal file1 As String, ByVal file2 As String) As Boolean Implements IFileComparer.IsSamefile
        Dim file1byte As Integer
        Dim file2byte As Integer
        Dim fs1 As FileStream
        Dim fs2 As FileStream

        '' Determine if the same file was referenced two times.
        If (file1 = file2) Then
            ' Return true to indicate that the files are the same.
            Return True
        End If

        ' Open the two files.
        fs1 = New FileStream(file1, FileMode.Open)
        fs2 = New FileStream(file2, FileMode.Open)

        'Check the file sizes. If they are not the same, the files 
        ' are not the same.
        If fs1.Length <> fs2.Length Then
            ' Close the file
            fs1.Close()
            fs2.Close()
            fs1.Dispose()
            fs2.Dispose()
            ' Return false to indicate files are different
            Return False
        End If

        ' Read and compare a byte from each file until either a
        ' non-matching set of bytes is found or until the end of
        ' file1 is reached.
        Do

            ' Read one byte from each file.
            file1byte = fs1.ReadByte()
            file2byte = fs2.ReadByte()
        Loop While ((file1byte = file2byte) AndAlso (file1byte <> -1) AndAlso file2byte <> -1)

        ' Close the files.
        fs1.Close()
        fs2.Close()
        fs1.Dispose()
        fs2.Dispose()

        ' Return the success of the comparison. "file1byte" is 
        ' equal to "file2byte" at this point only if the files are 
        ' the same.
        Return ((file1byte - file2byte) = 0)
    End Function

End Class
