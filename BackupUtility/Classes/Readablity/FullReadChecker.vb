Imports System.IO

Public Class FullReadChecker
    Implements IFileReadableChecker

    Private _exceptionHandler As IExceptionHandler

    Public Sub New(exceptionHandler As IExceptionHandler)
        _exceptionHandler = exceptionHandler
    End Sub

    Public Function CanReadFile(ByVal aFile As String) As Boolean Implements IFileReadableChecker.CanReadFile

        'Try if we can read the file
        Dim canRead As Boolean = False
        If aFile Is Nothing OrElse aFile = "" Then Return False

        Dim aStream As FileStream = Nothing
        Dim aBR As BinaryReader
        Try

            aStream = New FileStream(aFile, FileMode.Open, FileAccess.Read)
            aBR = New BinaryReader(aStream)
            Dim length As Long = aStream.Length
            'Hope this is faster and less error prone as readallbytes
            While length > 0
                Dim readBytes As Integer

                If length > Integer.MaxValue Then
                    readBytes = Integer.MaxValue
                Else
                    readBytes = CInt(length)
                End If
                aBR.ReadBytes(readBytes)
                length -= readBytes
            End While

            'Dim aByteArry() As Byte = File.ReadAllBytes(aFile)
            canRead = True
        Catch ex As UnauthorizedAccessException
            'If _commands.SkipCopyErrors Then
            '    RaiseEvent ProcessInfoEvent()
            'Else
            '    Throw ex
            'End If
            _exceptionHandler.HandleException("Failed to read " & aFile & "(" & ex.Message & ")", ex)
        Catch ex As IOException
            'If _commands.SkipCopyErrors Then
            '    RaiseEvent ProcessInfoEvent("Failed to read " & aFile & "(" & ex.Message & ")")
            'Else
            '    Throw ex
            'End If

            _exceptionHandler.HandleException("Failed to read " & aFile & "(" & ex.Message & ")", ex)
        Catch ex As OutOfMemoryException
            'not enough memory to read file hopefully we can copy it. 
            'TODO: might think about copy with other name or better check for read.
            canRead = True
        Finally
            If aStream IsNot Nothing Then
                aStream.Close()
                aStream.Dispose()
                aStream = Nothing
            End If
        End Try

        Return canRead
    End Function
End Class
