using Moq;
using ZCopy;

 namespace ZCopyUnitTester.Mocks
{
    public partial class MockBuilder
    {
        public static Mock<IConfirmationChecker> CreateIConfirmationChecker(bool expectedReturn)
        {
            Mock<IConfirmationChecker> confirmation = new Mock<IConfirmationChecker>();
            confirmation.Setup(x => x.GetConfirmation(It.IsAny<string>())).Returns(expectedReturn);
            return confirmation;
        }
    }
}
