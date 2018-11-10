Imports GenericClassLibraryTests.Mocks
Imports Moq
Imports ZCopy

<TestClass()>
Public Class NeedToCopyUpdatedOnlyCheckerTester
    <TestMethod()>
    Public Sub NeedToCopy_FileNew()
        Dim file As New FileMock(Nothing)
        Dim fileSystem As New FileSystemMock(file)
        Dim needToCopyUpdatedOnlyChecker As New NeedToCopyUpdatedOnlyChecker(Nothing, fileSystem, Nothing)

        Assert.IsTrue(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"))
    End Sub

    <TestMethod()>
    Public Sub NeedToCopy_NoConfirmationForExistingFile()
        Dim file As New FileMock(Nothing)
        file.Files.Add("dummy")
        Dim fileSystem As New FileSystemMock(file)
        Dim confirmation As Mock(Of IConfirmationChecker) = MockBuilder.CreateIConfirmationChecker(False)

        Dim needToCopyUpdatedOnlyChecker As New NeedToCopyUpdatedOnlyChecker(Nothing, fileSystem, confirmation.Object)

        Assert.IsFalse(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"))
        Assert.AreEqual(1, confirmation.Invocations.Count)
    End Sub

    <TestMethod()>
    Public Sub NeedToCopy_WithConfirmationForExistingFile_EqualFiles()
        Dim file As New FileMock(Nothing)
        file.Files.Add("dummy")
        Dim fileSystem As New FileSystemMock(file)
        Dim confirmation As Mock(Of IConfirmationChecker) = MockBuilder.CreateIConfirmationChecker(True)
        Dim fileComparere As Mock(Of IFileComparer) = MockBuilder.CreateIFileComparer(True)

        Dim needToCopyUpdatedOnlyChecker As New NeedToCopyUpdatedOnlyChecker(fileComparere.Object, fileSystem, confirmation.Object)

        Assert.IsFalse(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"))
        Assert.AreEqual(1, confirmation.Invocations.Count)
        Assert.AreEqual(1, fileComparere.Invocations.Count)
    End Sub

    <TestMethod()>
    Public Sub NeedToCopy_WithConfirmationForExistingFile_DifferentFiles()
        Dim file As New FileMock(Nothing)
        file.Files.Add("dummy")
        Dim fileSystem As New FileSystemMock(file)
        Dim confirmation As Mock(Of IConfirmationChecker) = MockBuilder.CreateIConfirmationChecker(True)
        Dim fileComparere As Mock(Of IFileComparer) = MockBuilder.CreateIFileComparer(False)

        Dim needToCopyUpdatedOnlyChecker As New NeedToCopyUpdatedOnlyChecker(fileComparere.Object, fileSystem, confirmation.Object)

        Assert.IsTrue(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"))
        Assert.AreEqual(1, confirmation.Invocations.Count)
        Assert.AreEqual(1, fileComparere.Invocations.Count)
    End Sub
End Class
