using GenericClassLibrary.FileSystem;
using GenericClassLibraryTests.Mocks;
using Xunit;
using ZCopy.Classes.FileIgnore;
using ZCopy.Interfaces;
using ZCopyUnitTester.Mocks;

namespace ZCopyUnitTester
{
    [Collection("IgnoreOnExtensionsCheckerTester")]
    public class IgnoreOnExtensionsCheckerTester
    {
        [Fact]
        public void TestIgnoreFile_ExclusiveEx_IsNull()
        {
            string[] exclusiveEx = null;
            IgnoreOnExtensionsChecker checker = CreateIgnoreOnExtensionsChecker(exclusiveEx);
            Assert.False(checker.IgnoreFile("blabla.txt"));
        }

        [Fact]
        public void TestIgnoreFile_ExclusiveEx_IsEmpty()
        {
            string[] exclusiveEx = new string[0];
            IgnoreOnExtensionsChecker checker = CreateIgnoreOnExtensionsChecker(exclusiveEx);
            Assert.False(checker.IgnoreFile("blabla.txt"));
        }

        [Fact]
        public void TestIgnoreFile_ExclusiveEx_ContainsOtherValues()
        {
            string[] exclusiveEx = { "png", "jpg" };
            IgnoreOnExtensionsChecker checker = CreateIgnoreOnExtensionsChecker(exclusiveEx);
            Assert.False(checker.IgnoreFile("blabla.txt"));
        }

        [Fact]
        public void TestIgnoreFile_ExclusiveEx_ExtensionIsInIgnoreList()
        {
            string[] exclusiveEx = { "png", "jpg", "txt" };
            IgnoreOnExtensionsChecker checker = CreateIgnoreOnExtensionsChecker(exclusiveEx);
            Assert.True(checker.IgnoreFile("blabla.txt"));
        }

        [Fact]
        public void TestIgnoreFile_ExclusiveEx_ExtensionIsInIgnoreList_Uppercase()
        {
            string[] exclusiveEx = { "PNG", "JPG", "TXT" };
            IgnoreOnExtensionsChecker checker = CreateIgnoreOnExtensionsChecker(exclusiveEx);
            Assert.True(checker.IgnoreFile("blabla.txt"));
        }

        [Fact]
        public void TestIgnoreFile_ExclusiveEx_FileHasNoExtension()
        {
            string[] exclusiveEx = { "PNG", "JPG", "TXT" };
            IgnoreOnExtensionsChecker checker = CreateIgnoreOnExtensionsChecker(exclusiveEx);
            Assert.False(checker.IgnoreFile("blabla"));
        }

        private IgnoreOnExtensionsChecker CreateIgnoreOnExtensionsChecker(string[] exclusiveEx)
        {
            IFile file = new FileMock(new DirectoryMock());
            IFileSystem fileSystem = new FileSystemMock();
            IExceptionHandler exceptionHandler = MockBuilder.CreateIExceptionHandler().Object;
            return new IgnoreOnExtensionsChecker(exclusiveEx, fileSystem, exceptionHandler);
        }
    }
}
