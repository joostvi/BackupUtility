using GenericClassLibraryTests.Mocks;
using Xunit;
using Moq;
using ZCopy.Classes.NeedToCopy;
using ZCopy.Interfaces;
using ZCopyUnitTester.Mocks;

namespace ZCopyUnitTester
{
    [Collection("NeedToCopyUpdatedOnlyCheckerTester")]
    public class NeedToCopyUpdatedOnlyCheckerTester
    {
        [Fact()]
        public void NeedToCopy_Filenew()
        {
            FileMock file = new FileMock(null);
            FileSystemMock fileSystem = new FileSystemMock(file);
            NeedToCopyUpdatedOnlyChecker needToCopyUpdatedOnlyChecker = new NeedToCopyUpdatedOnlyChecker(null, fileSystem, null);


            Assert.True(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"));
        }

        [Fact()]
        public void NeedToCopy_NoConfirmationForExistingFile()
        {
            FileMock file = new FileMock(null);
            file.Files.Add("dummy");
            FileSystemMock fileSystem = new FileSystemMock(file);
            Mock<IConfirmationChecker> confirmation = MockBuilder.CreateIConfirmationChecker(false);
            Mock<IFileComparer> fileComparere = MockBuilder.CreateIFileComparer(false);

            NeedToCopyUpdatedOnlyChecker needToCopyUpdatedOnlyChecker = new NeedToCopyUpdatedOnlyChecker(fileComparere.Object, fileSystem, confirmation.Object);

            Assert.False(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"));
            Assert.Equal(1, fileComparere.Invocations.Count);
            Assert.Equal(1, confirmation.Invocations.Count);
        }

        [Fact()]
        public void NeedToCopy_WithConfirmationForExistingFile_EqualFiles()
        {
            FileMock file = new FileMock(null);
            file.Files.Add("dummy");
            FileSystemMock fileSystem = new FileSystemMock(file);
            Mock<IConfirmationChecker> confirmation = MockBuilder.CreateIConfirmationChecker(true);
            Mock<IFileComparer> fileComparere =  MockBuilder.CreateIFileComparer(true);

            NeedToCopyUpdatedOnlyChecker needToCopyUpdatedOnlyChecker = new NeedToCopyUpdatedOnlyChecker(fileComparere.Object, fileSystem, confirmation.Object);

            Assert.False(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"));
            Assert.Equal(0, confirmation.Invocations.Count);
            Assert.Equal(1, fileComparere.Invocations.Count);
        }

        [Fact()]
        public void NeedToCopy_WithConfirmationForExistingFile_DifferentFiles()
        {
            FileMock file = new FileMock(null);
            file.Files.Add("dummy");
            FileSystemMock fileSystem = new FileSystemMock(file);
            Mock<IConfirmationChecker> confirmation = MockBuilder.CreateIConfirmationChecker(true);
            Mock<IFileComparer> fileComparere = MockBuilder.CreateIFileComparer(false);

            NeedToCopyUpdatedOnlyChecker needToCopyUpdatedOnlyChecker = new NeedToCopyUpdatedOnlyChecker(fileComparere.Object, fileSystem, confirmation.Object);
            
            Assert.True(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"));
            Assert.Equal(1, confirmation.Invocations.Count);
            Assert.Equal(1, fileComparere.Invocations.Count);
        }
    }
}
