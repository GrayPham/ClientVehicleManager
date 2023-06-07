using Connect.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.RemoteDataProvider.Interface
{
    public interface IClientHandler
    {
        event GeneralEventHandler<string> ClientConnected;
        event GeneralEventHandler<String> ClientDisconnected;
        event GeneralEventHandler<string> ClientReConnected;
    }
}
