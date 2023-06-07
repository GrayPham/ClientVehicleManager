using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Connect.SocketClient
{
    /// <summary>
    /// Handle client in Server side
    /// </summary>
    public interface ITcpClientHandler : IDisposable
    {
        bool IsAlive { get; }

        /// <summary>
        /// Set socket user for this client
        /// </summary>
        TcpClient Socket { get; set; }

        /// <summary>
        /// 100ms ticker
        /// </summary>
        void InvokeTick();

        ILog Log { get; set; }

        void Shutdown();

        void Begin();
    }
}
