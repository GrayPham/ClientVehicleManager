using Connect.Common;
using Connect.Common.Interface;
using Connect.Common.Logging;
using nsFramework.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Connect.SocketClient
{
    public class AsyncSocketClient<TClient> : ISocketClient where TClient : class, ITcpClientHandler, new()
    {
        #region Private Member
        //-----------------------------------------------------------------------------------------------------

        protected ILog Log;
        private readonly IPEndPoint _endPoint;
        private TcpClient _clientSocket;
        private TClient _client;
        private Timer _timer;
        private const string InternalName = "AsyncSocketClient";
        private bool _isTimerProcessing;

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Interface Implementation
        //-----------------------------------------------------------------------------------------------------

        public event GeneralEventHandler<ITcpClientHandler> Connected;
        public event GeneralEventHandler<ITcpClientHandler> ReConnected;
        public event GeneralEventHandler<String> Disconnected;
        private bool IsReconnect = false;
        void OnConnected(ITcpClientHandler handler)
        {
            if (Connected != null) Connected(this, new EventArgs<ITcpClientHandler>(handler));
        }
        void OnReConnected(ITcpClientHandler handler)
        {
            IsReconnect = false;
            if (ReConnected != null) ReConnected(this, new EventArgs<ITcpClientHandler>(handler));
        }

        void OnDisconnected(String st)
        {
            if (null != _client)
            {
                _client.Shutdown();
            }

            if (null == _clientSocket)
            {
                Log.Critical("Null thread or socket");
            }
            else
            {
                _clientSocket.Client.Close(100);   // Wait 100ms to data be send            
            }

            _clientSocket = null;
            _isTimerProcessing = true;
            if (_timer != null) _timer.Dispose();
            if (Disconnected != null) Disconnected(this, new EventArgs<String>(st));
            Log.Info("Client disconneted suceeded at: " + _endPoint);
        }

        public string Name
        {
            get { return InternalName; }
        }

        public Boolean IsConnected { get; private set; }

        public Boolean Connect()
        {
            if (IsConnected)
            {
                Log.Info("Connected!");
                return true;
            }

            try
            {
                _clientSocket = new TcpClient();
                //SslStream sslStream = new SslStream(mail.GetStream());

            }
            catch (Exception ex)
            {
                Log.Critical("Can't create new Socket: " + ex.Message);
                return false;
            }

            var e = new SocketAsyncEventArgs { RemoteEndPoint = _endPoint, UserToken = _clientSocket };
            e.Completed += ConnectCompleted;

            try
            {
                //_endPoint.Address.ToString(), _endPoint.Port
                if(_clientSocket == null)
                {
                    Connect();
                }
                else
                {
                    _clientSocket.Client.ConnectAsync(e);
                }
                
            }
            catch (Exception ex)
            {
                Log.Critical("Can't create ConnectAsync the socket: " + ex.Message);
                return false;
            }

            return true;
        }
        public Boolean ReConnect()
        {
            IsReconnect = true;
            return Connect();
        }
        public void ReConnectAsync()
        {
            if (_clientSocket != null)
            {
                var e = new SocketAsyncEventArgs { RemoteEndPoint = _endPoint, UserToken = _clientSocket };
                e.Completed += ConnectCompleted;

                try
                {
                    _clientSocket.Client.ConnectAsync(e);
                }
                catch (Exception ex)
                {
                    Log.Critical("Can't create ConnectAsync the socket: " + ex.Message);
                }

            }
        }
        public void Disconnect()
        {
            if (IsConnected)
            {
                Log.Info("Currently disconnected");
                return;
            }

            OnDisconnected(@"Đóng kết nối bởi chương trình");
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Public
        //-----------------------------------------------------------------------------------------------------

        public AsyncSocketClient(IPEndPoint endPoint) : this(endPoint, null)
        {
        }

        /// <summary>
        /// Init a new instance of server
        /// </summary>
        /// <param name="endPoint">IPAddress and Port</param>
        /// <param name="log">ILog object</param>
        public AsyncSocketClient(IPEndPoint endPoint, ILog log)
        {
            Debug.Assert(null != endPoint);

            Log = log ?? Singleton<DummyLog>.Instance;
            _endPoint = endPoint;


        }
        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Private Method
        //-----------------------------------------------------------------------------------------------------

        private void ClientTickCallback(object state)
        {
            if (_isTimerProcessing) return;
            _isTimerProcessing = true;
            if (_client != null) _client.InvokeTick();
            else
            {
                Log.Error(@"Client is null");
                OnDisconnected(@"Giá trị null trong client");
                return;
            }

            if (!_client.IsAlive)
            {
                OnDisconnected(@"Đóng kết nối bởi lỗi mạng hoặc server ngắt kết nối");
            }
            _isTimerProcessing = false;
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            // Set the flag for socket connected.
            var connected = (e.SocketError == SocketError.Success);
            if (connected)
            {
                _client = new TClient { Socket = _clientSocket, Log = Log };
                _client.Begin();
                OnConnected(_client);
                if (IsReconnect) OnReConnected(_client);
                try
                {
                    _isTimerProcessing = false;
                    _timer = new Timer(ClientTickCallback, _clientSocket, 100, 100);
                }
                catch (Exception ex)
                {
                    Log.Critical("Can't create listen the socket: " + ex.Message);
                }
            }
            else
            {
                IsConnected = false;
                if (e.SocketError == SocketError.TimedOut)
                {
                    OnDisconnected(@"Quá thời gian kết nối không có phản hồi!");
                }
                else if (e.SocketError == SocketError.Shutdown)
                {
                    OnDisconnected(@"Server đóng kết nối!");
                }
                else if (e.SocketError == SocketError.ConnectionRefused)
                {
                    OnDisconnected(@"Kết nối bị từ chối! (Server không chạy)");
                }
                else
                {
                    OnDisconnected(@"Lỗi kết nối khác!");
                }
            }
        }

        public void UpdateIP(string ip)
        {
            _endPoint.Address = IPAddress.Parse(ip);
        }
        public void UpdatePort(int port)
        {
            _endPoint.Port = port;
        }
        //-----------------------------------------------------------------------------------------------------
        #endregion
    }
}
