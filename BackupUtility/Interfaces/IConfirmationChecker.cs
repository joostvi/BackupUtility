namespace ZCopy
{
    public interface IConfirmationChecker
    {
        bool GetConfirmation(string aTarget);
    }
}
