using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCopy.Interfaces;

namespace ZCopy.Classes.Confirmation
{
    public class ConfirmationGetter : IConfirmationHandler
    {
        public delegate bool ConfirmationRequest(string theInfo);

        public event ConfirmationRequest ConfirmationRequestHandler;

        public bool GetConfirmation(string aTarget)
        {
            return ConfirmationRequestHandler.Invoke(aTarget);
        }
    }
}
