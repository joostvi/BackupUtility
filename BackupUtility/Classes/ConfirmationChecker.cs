namespace ZCopy
{
    public class ConfirmationChecker : IConfirmationChecker
    {
        public delegate bool ConfirmationRequest(string theInfo);

        private readonly ConfirmationRequest _ConfirmationRequest;
        private readonly bool _requestConfirm;

        public ConfirmationChecker(bool requestConfirm)
        {
            _requestConfirm = requestConfirm;
        }

        public bool GetConfirmation(string aTarget)
        {
            if (_requestConfirm && _ConfirmationRequest != null)
                return _ConfirmationRequest(aTarget);
            return true;
        }
    }
}
