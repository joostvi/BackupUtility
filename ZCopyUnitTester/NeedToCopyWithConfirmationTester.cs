using GenericClassLibraryTests.Mocks;
using Xunit;
using Moq;
using ZCopy.Classes.NeedToCopy;
using ZCopy.Interfaces;
using ZCopyUnitTester.Mocks;

namespace ZCopyUnitTester
{
    [Collection("NeedToCopyWithConfirmationTester")]
    public  class NeedToCopyWithConfirmationTester
    {
        [Fact()]
        public void NeedToCopy_Filenew()
        {
            FileMock file = new FileMock(null);
            FileSystemMock fileSystem = new FileSystemMock(file);
            NeedToCopyWithConfirmation needToCopyUpdatedOnlyChecker = new NeedToCopyWithConfirmation(fileSystem, null);
            
            Assert.True(needToCopyUpdatedOnlyChecker.NeedToCopy(null, "dummy", "dummy"));
        }

        [Fact()]
        public void NeedToCopy_ConfirmationForExistingFile()
        {
            FileMock file = new FileMock(null);
            file.Files.Add("dummy");
            FileSystemMock fileSystem = new FileSystemMock(file);
            Mock<IConfirmationChecker> confirmation = MockBuilder.CreateIConfirmationChecker(true);
            
            NeedToCopyWithConfirmation needToCopyUpdatedOnlyChecker = new NeedToCopyWithConfirmation(fileSystem, confirmation.Object);

            Assert.True(needToCopyUpdatedOnlyChecker.NeedToCopy(null, "dummy", "dummy"));
            Assert.Equal(1, confirmation.Invocations.Count);
        }

        [Fact()]
        public void NeedToCopy_NoConfirmationForExistingFile()
        {
            FileMock file = new FileMock(null);
            file.Files.Add("dummy");
            FileSystemMock fileSystem = new FileSystemMock(file);
            Mock<IConfirmationChecker> confirmation = MockBuilder.CreateIConfirmationChecker(false);

            NeedToCopyWithConfirmation needToCopyUpdatedOnlyChecker = new NeedToCopyWithConfirmation(fileSystem, confirmation.Object);

            Assert.False(needToCopyUpdatedOnlyChecker.NeedToCopy(null, "dummy", "dummy"));
            Assert.Equal(1, confirmation.Invocations.Count);
        }
    }
}
