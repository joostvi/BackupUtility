﻿using System.IO;
using GenericClassLibrary.FileSystem.Compare;
using Moq;

namespace ZCopyUnitTester.Mocks
{
    public partial  class MockBuilder
	{
		public static Mock<IFileComparer> CreateIFileComparer(bool isSameFile )
		{    
			Mock<IFileComparer> comparer = new Mock<IFileComparer>();
			comparer.Setup(x => x.IsSameFile(It.IsAny<string>(), It.IsAny<string>())).Returns(isSameFile);
			comparer.Setup(x => x.IsSameFile(It.IsAny<FileInfo>(), It.IsAny<FileInfo>())).Returns(isSameFile);
			return comparer;
		}
	}
}