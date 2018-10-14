Imports ZCopy

Public Class StopOnExceptionHandler
    Implements IExceptionHandler

    Public Sub HandleException(message As String, ex As Exception) Implements IExceptionHandler.HandleException
        Throw ex
    End Sub
End Class
