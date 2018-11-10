using GenericClassLibraryTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ZCopy.Classes.NeedToCopy;
using ZCopy.Interfaces;
using ZCopyUnitTester.Mocks;

namespace ZCopyUnitTester
{
    [TestClass()]
    public  class NeedToCopyWithConfirmationTester
    {
        [TestMethod()]
        public void NeedToCopy_Filenew()
        {
            FileMock file = new FileMock(null);
            FileSystemMock fileSystem = new FileSystemMock(file);
            NeedToCopyWithConfirmation needToCopyUpdatedOnlyChecker = new NeedToCopyWithConfirmation(fileSystem, null);
            
            Assert.IsTrue(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"));
        }

        [TestMethod()]
        public void NeedToCopy_ConfirmationForExistingFile()
        {
            FileMock file = new FileMock(null);
            file.Files.Add("dummy");
            FileSystemMock fileSystem = new FileSystemMock(file);
            Mock<IConfirmationChecker> confirmation = MockBuilder.CreateIConfirmationChecker(true);
            
            NeedToCopyWithConfirmation needToCopyUpdatedOnlyChecker = new NeedToCopyWithConfirmation(fileSystem, confirmation.Object);

            Assert.IsTrue(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"));
            Assert.AreEqual(1, confirmation.Invocations.Count);
        }

        [TestMethod()]
        public void NeedToCopy_NoConfirmationForExistingFile()
        {
            FileMock file = new FileMock(null);
            file.Files.Add("dummy");
            FileSystemMock fileSystem = new FileSystemMock(file);
            Mock<IConfirmationChecker> confirmation = MockBuilder.CreateIConfirmationChecker(false);

            NeedToCopyWithConfirmation needToCopyUpdatedOnlyChecker = new NeedToCopyWithConfirmation(fileSystem, confirmation.Object);

            Assert.IsFalse(needToCopyUpdatedOnlyChecker.NeedToCopy("dummy", "dummy"));
            Assert.AreEqual(1, confirmation.Invocations.Count);
        }
    }
}
