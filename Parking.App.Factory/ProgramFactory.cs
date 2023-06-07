using Connect.Common;
using Connect.Common.Contract;
using Connect.Common.Interface;
using Connect.Common.Logging;
using Connect.RemoteDataProvider.Interface;
using Connect.SocketClient;
using nsConnect.RemoteDataProvider.Client;
using nsFramework.Common.Pattern;
using Parking.App.Interface.Common;
using Parking.App.Service.Handlers;
using System.Net;

namespace Parking.App.Factory
{
    public class ProgramFactory : IClientHandler
    {
        //---------------------------------------------------------------

        #region Singleton

        //**--------------------------------------------------------------------------------
        public event GeneralEventHandler<string> ClientConnected;
        public event GeneralEventHandler<string> ClientDisconnected;
        public event GeneralEventHandler<string> ClientReConnected;

        private void OnClientConnected(string data)
        {
            if (ClientConnected != null) ClientConnected(this, new EventArgs<string>(data));
        }
        private void OnClientDisconnected(string data)
        {
            if (ClientDisconnected != null) ClientDisconnected(this, new EventArgs<string>(data));
        }
        private void OnClientReConnected(string data)
        {
            if (ClientReConnected != null) ClientReConnected(this, new EventArgs<string>(data));
        }
        public virtual IProgramController ProgramController { get; set; }
        public static ProgramFactory Instance
        {
            get { return Nested.Factory; }
        }

        private class Nested
        {
            internal static readonly ProgramFactory Factory;
            internal static readonly bool IsCreated = false;
            // Explicit static constructor to tell C# compiler
            // not to mark type as before field initialize

            static Nested()
            {
                Factory = new ProgramFactory();
                IsCreated = true;
            }
        }

        //**--------------------------------------------------------------------------------

        #endregion

        //---------------------------------------------------------------


        #region Member
        protected readonly ILog _log = new DirectLog(null, Singleton<StandardConsole>.Instance);
        protected readonly ISocketClient _socketClientServer;
        protected readonly ISessionHandler _clientSessionHandler;

        protected readonly IPort _serialPort;
        protected string _ipServer = "";
        protected int _port = 9000;
        protected string _path = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        public string IPServer => _ipServer;
        public ILog Log
        {
            get { return _log; }
        }
        public ISocketClient SocketClientServer
        {
            get { return _socketClientServer; }
        }
        public ISessionHandler ClientSessionHandler
        {
            get { return _clientSessionHandler; }
        }
        public string Path => _path;
        #endregion

        //---------------------------------------------------------------

        #region Service
        private readonly ILoginService _loginService;
        #endregion

        //---------------------------------------------------------------

        #region Constructor

        public ProgramFactory()
        {
            _path = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            _log = new DirectLog(_path, null, Singleton<StandardConsole>.Instance);

            #region Connect Server

            #endregion

            _ipServer = ApplicationInfo.IPServer;
            _port = ApplicationInfo.PortUser;
            _clientSessionHandler = new SessionHandler(_log, ApplicationInfo.eClientType);

            ApplicationInfo.PortUser = _port;
            var ipaddress = IPAddress.Parse(_ipServer);
            var endpoint = new IPEndPoint(ipaddress, _port);
            _socketClientServer = new AsyncSocketClient<StandardTcpClientHandler>(endpoint, _log);
            _socketClientServer.Connected += (sender, e) => { 
                _clientSessionHandler.Connected((IPort)e.Data);
                OnClientConnected(e.Data.ToString()); };
            _socketClientServer.Disconnected += (send, e) => { _clientSessionHandler.Disconnected(); OnClientDisconnected(""); 
            };
            _socketClientServer.ReConnected += (send, e) => { OnClientReConnected(""); };

            var loginService = new RemoteLoginService(_log);
            _loginService = loginService;
            loginService.RegisterType();
            _clientSessionHandler.Register(loginService);
            //_socketClientServer.Connect();

        }

        #endregion

        //---------------------------------------------------------------

        #region Property
        public ILoginService loginService { get { return _loginService; } }
        #endregion

        //---------------------------------------------------------------
    }
}
