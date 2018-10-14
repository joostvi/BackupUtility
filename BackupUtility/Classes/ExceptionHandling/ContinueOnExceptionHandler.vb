Imports ZCopy

Public Class ContinueOnExceptionHandler
    Implements IExceptionHandler
    Private _commandHandler As CommandHandler

    Public Sub New(commandHandler As CommandHandler)
        _commandHandler = commandHandler
    End Sub

    Public Sub HandleException(message As String, ex As Exception) Implements IExceptionHandler.HandleException
        Dim eventText As String = message
        If (eventText = "" OrElse eventText Is Nothing) Then
            eventText = ex.Message
        End If
        _commandHandler.RaiseThisEvent(eventText)
    End Sub
End Class
