Imports System.IO
Imports Moq
Imports ZCopy

Partial Public Class MockBuilder

    Public Shared Function CreateIFileComparer(isSameFile As Boolean) As Mock(Of IFileComparer)
        Dim comparer As New Mock(Of IFileComparer)()
        comparer.Setup(Function(x) x.IsSamefile(It.IsAny(Of String), It.IsAny(Of String))).Returns(isSameFile)
        comparer.Setup(Function(x) x.IsSameFile(It.IsAny(Of FileInfo), It.IsAny(Of FileInfo))).Returns(isSameFile)
        Return comparer
    End Function

End Class
