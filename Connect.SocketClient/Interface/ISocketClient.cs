using Connect.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.SocketClient
{
    public interface ISocketClient
    {
        event GeneralEventHandler<ITcpClientHandler> Connected;
        event GeneralEventHandler<String> Disconnected;
        event GeneralEventHandler<ITcpClientHandler> ReConnected;
        Boolean Connect();
        Boolean ReConnect();
        void ReConnectAsync();
        void Disconnect();
        void UpdateIP(string ip);
        void UpdatePort(int port);
    }
}
