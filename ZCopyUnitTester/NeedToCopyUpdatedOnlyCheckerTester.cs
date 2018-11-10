using GenericClassLibraryTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ZCopy;
using ZCopyUnitTester.Mocks;

namespace ZCopyUnitTester
{
    [TestClass()]
    public class NeedToCopyUpdatedOnlyCheckerTester
    {
        [TestMethod()]
        public void NeedToCopy_Filenew()
        {
            FileMock file = new FileMock(null);
            FileSystemMock fileSystem = new FileSystemMock(file);
            NeedToCopyUpdatedOnlyChecker needToCopyUpdatedOnlyChecker = new NeedToCopyUpdatedOnlyChecker(null, fileSystem, null);


            Assert.IsTrue(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"));
        }

        [TestMethod()]
        public void NeedToCopy_NoConfirmationForExistingFile()
        {
            FileMock file = new FileMock(null);
            file.Files.Add("dummy");
            FileSystemMock fileSystem = new FileSystemMock(file);
            Mock<IConfirmationChecker> confirmation = MockBuilder.CreateIConfirmationChecker(false);

            NeedToCopyUpdatedOnlyChecker needToCopyUpdatedOnlyChecker = new NeedToCopyUpdatedOnlyChecker(null, fileSystem, confirmation.Object);

            Assert.IsFalse(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"));
            Assert.AreEqual(1, confirmation.Invocations.Count);
        }

        [TestMethod()]
        public void NeedToCopy_WithConfirmationForExistingFile_EqualFiles()
        {
            FileMock file = new FileMock(null);
            file.Files.Add("dummy");
            FileSystemMock fileSystem = new FileSystemMock(file);
            Mock<IConfirmationChecker> confirmation = MockBuilder.CreateIConfirmationChecker(true);
            Mock<IFileComparer> fileComparere =  MockBuilder.CreateIFileComparer(true);

            NeedToCopyUpdatedOnlyChecker needToCopyUpdatedOnlyChecker = new NeedToCopyUpdatedOnlyChecker(fileComparere.Object, fileSystem, confirmation.Object);

            Assert.IsFalse(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"));
            Assert.AreEqual(1, confirmation.Invocations.Count);
            Assert.AreEqual(1, fileComparere.Invocations.Count);
        }

        [TestMethod()]
        public void NeedToCopy_WithConfirmationForExistingFile_DifferentFiles()
        {
            FileMock file = new FileMock(null);
            file.Files.Add("dummy");
            FileSystemMock fileSystem = new FileSystemMock(file);
            Mock<IConfirmationChecker> confirmation = MockBuilder.CreateIConfirmationChecker(true);
            Mock<IFileComparer> fileComparere = MockBuilder.CreateIFileComparer(false);

            NeedToCopyUpdatedOnlyChecker needToCopyUpdatedOnlyChecker = new NeedToCopyUpdatedOnlyChecker(fileComparere.Object, fileSystem, confirmation.Object);
            
            Assert.IsTrue(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"));
            Assert.AreEqual(1, confirmation.Invocations.Count);
            Assert.AreEqual(1, fileComparere.Invocations.Count);
        }
    }
}
