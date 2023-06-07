using Connect.Common;
using Connect.Common.Interface;
using System;

namespace Connect.Common.Interface
{
    public interface IPort : IL2LMessageUpdated, ITick
    {
        /// <summary>
        /// Fire with string when data is received
        /// </summary>
        event DataEventHandler DataReceived;

        /// <summary>
        /// Port name like COM1, COM20...
        /// </summary>
        String Port { get; set; }

        /// <summary>
        /// Write String to opened port
        /// </summary>
        /// <param name="message">ASCII message</param>
        void Write(String message);

        /// <summary>
        /// Write array to opened port
        /// </summary>
        /// <param name="data">Array of byte</param>
        void Write(Byte[] data);

        /// <summary>
        /// Write Responded
        /// </summary>
        event BooleanEventHandler WriteResponsed;

        /// <summary>
        /// Open Port
        /// </summary>
        void Open();

        /// <summary>
        /// Response when open is finish
        /// </summary>
        event BooleanEventHandler OpenResponsed;

        /// <summary>
        /// Close Port
        /// </summary>
        void Close();

        /// <summary>
        /// Response when close is finish
        /// </summary>
        event BooleanEventHandler CloseResponsed;

        /// <summary>
        /// true if port is opened
        /// </summary>
        Boolean IsOpened { get; }

        /// <summary>
        /// True if port is opened and false for Closed
        /// </summary>
        event BooleanEventHandler OpenStateChanged;
    }
}
