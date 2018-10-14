Imports ZCopy

<TestClass()>
Public Class CommandTester

    <TestMethod()>
    Public Sub TestCreateWithSourceEndsOnQuoate()
        Dim args(1) As String
        Dim input As String = "c:\user\joost\"""
        Dim expected As String = "c:\user\joost\"

        args(0) = input
        args(1) = "somepath"

        Assert.AreEqual(expected, New Commands(args).Source)

    End Sub
End Class
