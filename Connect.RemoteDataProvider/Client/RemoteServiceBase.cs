using Connect.Common;
using Connect.Common.Common;
using Connect.Common.Interface;
using Connect.RemoteDataProvider.Interface;
using System;
using System.Timers;

namespace nsConnect.RemoteDataProvider.Client
{
    public abstract class RemoteServiceBase : IRemoteDataHandler
    {
        #region Private/Protected
        //**--------------------------------------------------------------------------------

        protected ILog Log;
        protected Timer TimerOut;
        protected bool IsReady;

        protected object SendDataLock = new object();
        protected int FrameID;

        //**--------------------------------------------------------------------------------
        #endregion

        #region Abstract
        //**--------------------------------------------------------------------------------
        public int GetClientID() { return 0; }
        public abstract String ServiceName { get; }
        public abstract String ToString(ushort functionCode);
        public abstract void OnFailed();

        //**--------------------------------------------------------------------------------
        #endregion

        #region IRemoteDataHandler
        //**--------------------------------------------------------------------------------

        public bool IsConnected { set; protected get; }
        public abstract uint Signature { get; }
        public event GeneralEventHandler<DataContainer> RequestSend;

        public virtual void DataReceived(ushort functionCode, byte[] data, int frameNumber)
        {
            Log.Info(ServiceName + "; FC:" + ToString(functionCode) + "; FrameID=" + frameNumber + ";/" + data.Length);
            if (TimerOut != null) TimerOut.Stop();
        }


        protected void Send(ushort code, byte[] data)
        {
            if (!IsConnected)
            {
                Log.Info(ServiceName + " is not connected; FC" + ToString(code) + "/" + data.Length);
                OnFailed();
                return;
            }
            Log.Info(ServiceName + " send - FC" + ToString(code) + "/" + data.Length);
            DataContainer container;
            lock (SendDataLock)
            {
                container = new DataContainer(code, data, FrameID++);
            }
            var handler = RequestSend;
            if (handler != null) handler(this, new EventArgs<DataContainer>(container));

            TimerOut = new Timer(10000) { AutoReset = false };
            TimerOut.Elapsed += TimerOnElapsed;
            TimerOut.Start();
        }

        public virtual void Reset()
        {
            IsReady = false;
        }

        public virtual void Registered()
        {
            IsReady = false;
        }
        public virtual void Registered(int clientID)
        {
            IsReady = false;
        }
        public void Registered(EClientType eClientType)
        {
            IsReady = false;
        }

        public void Registered(int clientID, EClientType eClientType)
        {
            IsReady = false;
        }
        public virtual void UnRegistered()
        {
            // Do-nothing
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Log.Error(@"Time-Out");
            TimerOut.Stop();
            OnFailed();
        }



        //**--------------------------------------------------------------------------------

        #endregion
    }
}
