Public Class ConfirmationChecker
    Implements IConfirmationChecker

    Public Delegate Function ConfirmationRequest(ByVal theInfo As String) As Boolean

    Private ReadOnly _ConfirmationRequest As ConfirmationRequest
    Private ReadOnly _requestConfirm As Boolean

    Public Sub New(requestConfirm As Boolean)
        _requestConfirm = requestConfirm
    End Sub

    Public Function GetConfirmation(aTarget As String) As Boolean Implements IConfirmationChecker.GetConfirmation
        If _requestConfirm AndAlso _ConfirmationRequest IsNot Nothing Then
            Return _ConfirmationRequest(aTarget)
        End If
        Return True
    End Function
End Class
