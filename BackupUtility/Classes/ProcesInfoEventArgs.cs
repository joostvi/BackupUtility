using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCopy.Classes
{
    /// <summary>
    /// Event data of process info event.
    /// </summary>
    public class ProcessInfoEventArgs : EventArgs
    {
        /// <summary>
        /// Info about process state.
        /// </summary>
        public string Info { get; }

        /// <summary>
        /// The info about process state which should be communicated
        /// </summary>
        /// <param name="info"></param>
        public ProcessInfoEventArgs(string info) : base ()
        {
            Info = info;
        }
    }
}
