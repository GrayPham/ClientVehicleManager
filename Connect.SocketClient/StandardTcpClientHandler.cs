using Connect.Common;
using Connect.Common.Interface;
using Connect.Common.Logging;
using nsFramework.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace Connect.SocketClient
{
    public class StandardTcpClientHandler : ITcpClientHandler, IPort
    {
        #region Private Member
        //-----------------------------------------------------------------------------------------------------

        private TcpClient _socket;
        private bool _isAlive;

        private const int CheckAliveTimerReload = 10;
        private int _checkAliveTimer = CheckAliveTimerReload;
        private readonly object _isAliveLock = new object();
        private ILog _log = Singleton<DummyLog>.Instance;
        private string _remoteEndpoint = "";

        public class StateObject
        {
            public const int BufferSize = 1500;
            public TcpClient WorkSocket = null;
            public byte[] Buffer = new byte[BufferSize];
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Interface Implementation - ITcpClientHandler

        //-----------------------------------------------------------------------------------------------------

        public bool IsAlive
        {
            get
            {
                Boolean temp;
                lock (_isAliveLock)
                {
                    temp = _isAlive;
                }
                return temp;
            }
            private set
            {
                Boolean val;
                lock (_isAliveLock)
                {
                    val = _isAlive;
                    _isAlive = value;
                }
                if (val != value) OnOpenStateChanged(value);
            }
        }

        public void Dispose()
        {
            Close();
        }

        public TcpClient Socket
        {
            get { return _socket; }
            set
            {
                try
                {
                    Debug.Assert(value != null);
                    if (value != null)
                    {
                        _socket = value;
                        _remoteEndpoint = _socket.Client.RemoteEndPoint.ToString();
                        CheckConnected();
                    }

                }
                catch 
                {
                    IsAlive = false;
                }
            }
        }

        public override string ToString()
        {
            return _remoteEndpoint;
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Interface Implementation - IPort
        //-----------------------------------------------------------------------------------------------------

        public ILog Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public void Shutdown()
        {
            try
            {
                _socket.Client.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                _log.Error(this.GetType().Name + " => " + ex.Message);
                //_Log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            _isAlive = false;
        }

        public void Begin()
        {
            Debug.Assert(_socket != null);
            if (_socket != null)
            {
                var so = new StateObject { WorkSocket = _socket };
                try
                {
                    if (_socket.Connected)
                    {
                        _socket.Client.BeginReceive(so.Buffer, 0, StateObject.BufferSize, SocketFlags.None, AcceptReceiveDataCallback, so);
                    }
                }
                catch (Exception ex)
                {
                    _log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
            }

        }

        public bool IsOpened
        {
            get { return IsAlive; }
        }

        public string Port
        {
            get
            {
                if (null == _socket)
                {
                    return "Null";
                }
                return _socket.Client.RemoteEndPoint.ToString();
            }

            set
            {
                // Not support for TCP
            }
        }

        public void Write(string message)
        {
            var data = Encoding.ASCII.GetBytes(message);
            try
            {
                _log.Out("BeginSend " + data.Length + " bytes");
                if (_socket.Connected)
                {
                    _socket.Client.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(WriteCallback), _socket);
                }
                else
                {
                    _log.Out("BeginSend " + data.Length + " bytes");
                }
            }
            catch (Exception ex)
            {
                _log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);
                OnWriteResponsed(false);
            }
        }

        public void Write(byte[] data)
        {
            try
            {
                _log.Out("BeginSend " + data.Length + " bytes");
                if (_socket.Connected)
                {
                    _socket.Client.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(WriteCallback), _socket);
                }
                else
                {
                    _log.Out("Wite data when connect status false.");
                }

            }
            catch (Exception ex)
            {
                _log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);
                OnWriteResponsed(false);
            }
        }

        public void Open()
        {
            // Not support for TCP
            OnOpenResponsed(IsOpened);
        }

        public void Close()
        {
            try
            {
                _socket.Client.BeginDisconnect(false, Disconnected, _socket);
            }
            catch (Exception ex)
            {
                OnMessageUpdate(ex.Message);
                OnCloseResponsed(false);
            }
        }

        #region Events

        public event L2LMessageEventHandler L2LMessageUpdated;
        public event BooleanEventHandler OpenResponsed;
        public event BooleanEventHandler CloseResponsed;
        public event BooleanEventHandler WriteResponsed;
        public event BooleanEventHandler OpenStateChanged;
        public event MessageEventHandler MessageUpdated;
        public event DataEventHandler DataReceived;
        public event EventHandler Tick;

        private void OnOpenResponsed(Boolean succeed)
        {
            if (OpenResponsed != null) OpenResponsed(this, new BooleanEventArgs(succeed));
        }

        private void OnCloseResponsed(Boolean succeed)
        {
            if (CloseResponsed != null) CloseResponsed(this, new BooleanEventArgs(succeed));
        }

        private void OnWriteResponsed(Boolean succeed)
        {
            if (WriteResponsed != null) WriteResponsed(this, new BooleanEventArgs(succeed));
        }

        private void OnOpenStateChanged(Boolean state)
        {
            if (OpenStateChanged != null) OpenStateChanged(this, new BooleanEventArgs(state));
        }

        private void OnDataReceived(Byte[] data)
        {
            if (DataReceived != null) DataReceived(this, new DataEventArgs(data));
        }

        private void OnMessageUpdate(String msg)
        {
            if (MessageUpdated != null) MessageUpdated(this, new MessageEventArgs(msg));
        }

        public void InvokeTick()
        {
            try
            {
                _checkAliveTimer--;
                if (_checkAliveTimer == 0)
                {
                    _checkAliveTimer = CheckAliveTimerReload;
                    CheckConnected();
                }
                if (Tick != null) Tick(this, EventArgs.Empty);
            }
            catch (Exception)
            {

            }

        }

        #endregion

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Public Member
        //-----------------------------------------------------------------------------------------------------

        public StandardTcpClientHandler(ILog log)
        {
            Log = log ?? Singleton<DummyLog>.Instance;
        }

        public StandardTcpClientHandler()
        {
            Log = Singleton<DummyLog>.Instance;
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Private Method
        //-----------------------------------------------------------------------------------------------------
        private void Disconnected(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as TcpClient;
                if (null != socket)
                {
                    socket.Client.EndDisconnect(ar);
                    socket.Client.Close(100);
                    OnCloseResponsed(true);
                }
                else
                {
                    OnMessageUpdate("Null Socket in Write Callback");
                    OnCloseResponsed(false);
                }
                IsAlive = false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }
        }
        private static bool IsSocketConnected(TcpClient socket)
        {
            try
            {
                return !(socket.Client.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }
        private void WriteCallback(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as TcpClient;
                bool isWritten = false;
                if (null != socket)
                {
                    try
                    {
                        if (socket.Connected)
                        {
                            socket.Client.EndSend(ar);
                            isWritten = true;
                        }
                    }
                    catch (SocketException ex)
                    {
                        IsAlive = false;
                        isWritten = false;
                        _log.Error(ex.Message + " => " + ex.StackTrace);
                    }
                }
                else
                {
                    OnMessageUpdate("Null Socket in Write Callback");
                    isWritten = false;
                }
                OnWriteResponsed(isWritten);
                _log.Trace("Completed:" + ar.IsCompleted + "; Sync:" + ar.CompletedSynchronously + "; IsWritten: " + isWritten);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }
        }

        private void AcceptReceiveDataCallback(IAsyncResult ar)
        {
            var so = (StateObject)ar.AsyncState;
            var s = so.WorkSocket;

            int read;
            try
            {
                read = s.Client.EndReceive(ar);
            }
            catch (SocketException)
            {
                // switch (ex.ErrorCode)
                // Socket was close
                IsAlive = false;
                return;
            }
            catch (ObjectDisposedException)
            {
                // Socket was close
                IsAlive = false;
                return;
            }

            Log.Trace(read.ToString(CultureInfo.InvariantCulture));

            if (read <= 0) return;
            var data = new byte[read];
            for (var i = 0; i < read; i++) data[i] = so.Buffer[i];
            OnDataReceived(data);
            if (!IsAlive) return;
            try
            {
                s.Client.BeginReceive(so.Buffer, 0, StateObject.BufferSize, SocketFlags.None, AcceptReceiveDataCallback, so);
            }
            catch (Exception ex)
            {
                _log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        void CheckConnected()
        {
            bool val;
            try
            {
                var poll = _socket.Client.Poll(1000, SelectMode.SelectRead);
                var available = _socket.Available == 0;
                val = !((poll && available) || !_socket.Connected);
                if (!val) _socket.Client.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
                val = false;
            }
            IsAlive = val;

            /* 
                bool part1 = s.Poll(1000, SelectMode.SelectRead);
                bool part2 = (s.Available == 0);
                if ((part1 && part2 ) || !s.Connected)
                    return false;
                else
                    return true;
            */
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion

    }
}
