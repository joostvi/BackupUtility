Imports Moq
Imports ZCopy

Partial Public Class MockBuilder

    Public Shared Function CreateIConfirmationChecker(expectedReturn As Boolean) As Mock(Of IConfirmationChecker)
        Dim confirmation As New Mock(Of IConfirmationChecker)()
        confirmation.Setup(Function(x) x.GetConfirmation(It.IsAny(Of String))).Returns(expectedReturn)
        Return confirmation
    End Function
End Class
