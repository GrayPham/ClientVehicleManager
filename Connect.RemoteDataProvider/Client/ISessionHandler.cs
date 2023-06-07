using Connect.Common;
using Connect.Common.Common;
using Connect.Common.Interface;
using Connect.RemoteDataProvider.Interface;
using System;

namespace nsConnect.RemoteDataProvider.Client
{
    public class DataContainer
    {
        public int FrameNumber { get; set; }
        public UInt16 FunctionCode { get; set; }
        public byte[] Data { get; set; }

        public DataContainer(UInt16 func, byte[] data, int frameNumber)
        {
            FunctionCode = func;
            Data = data;
            FrameNumber = frameNumber;
        }
    }

    public interface ISessionHandler
    {
        void Connected(IPort port);
        void Disconnected();
        void Register(IRemoteDataHandler handler);
        void Close();
        event GeneralEventHandler<ConnectionFailedReason> SessionError;
    }
}
