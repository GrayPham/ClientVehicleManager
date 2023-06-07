using Connect.Common;
using Connect.Common.Common;
using nsConnect.RemoteDataProvider.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.RemoteDataProvider.Interface
{
    public interface IRemoteDataHandler
    {
        int GetClientID();
        Boolean IsConnected { set; }

        // Unique signature of serice
        UInt32 Signature { get; }
        // Basic Send and Receive
        event GeneralEventHandler<DataContainer> RequestSend;
        void DataReceived(ushort functionCode, byte[] data, int frameID);

        void Reset();
        // Call after register to Handler pool (only can send or rec when this called)
        void Registered();
        void Registered(int clientID);
        void Registered(EClientType eClientType);
        void Registered(int clientID, EClientType eClientType);
        // Un-register can't send or rec any more
        void UnRegistered();
    }
}
