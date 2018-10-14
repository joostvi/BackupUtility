Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports ZCopy

<TestClass()>
Public Class PathFormatterTester

    <TestMethod()>
    Public Sub TestWithDoubleQoutesAndSlash()
        Dim input As String = "c:\user\joost\"""
        Dim expected As String = "c:\user\joost\"
        Assert.AreEqual(expected, PathFormatter.FormatPath(input))

    End Sub

    <TestMethod()>
    Public Sub TestWithDoubleQoutesNoSlash()
        Dim input As String = "c:\user\joost"""
        Dim expected As String = "c:\user\joost"
        Assert.AreEqual(expected, PathFormatter.FormatPath(input))

    End Sub

End Class