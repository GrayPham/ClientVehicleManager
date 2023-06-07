using nsConnect.RemoteDataProvider.Protocol;
using Connect.Common;
using Connect.Common.Interface;
using Connect.Common.Logging;
using System;
using System.Collections.Generic;
using Connect.RemoteDataProvider.Interface;
using nsFramework.Common.Pattern;
using Connect.RemoteDataProvider.Client;
using Connect.RemoteDataProvider.Protocol;
using Connect.Common.Collection;
using Connect.Common.Common;

namespace nsConnect.RemoteDataProvider.Client
{
    public class SessionHandler : ISessionHandler
    {
        #region Init
        //**--------------------------------------------------------------------------------

        public SessionHandler(ILog log, String serial, String license, EClientType eClientType)
        {
            _log = log ?? Singleton<DummyLog>.Instance;
            _sessionStateDataHandler = new SessionStateDataHandler(_log, serial, license);
            _sessionStateDataHandler.SessionClose += SessionStateDataHandlerOnSessionClose;
            _sessionStateDataHandler.SessionBegin += SessionStateDataHandlerOnSessionStateBegin;
            _sessionStateDataHandler.SessionContinue += SessionStateDataHandlerOnSessionStateContinue;
            _sessionStateDataHandler.SessionError += (sender, args) =>
            {
                if (SessionError != null) SessionError(sender, args);
            };
            _parser.ValidFrameDetected += OnValidFrameDetected;
            Register(_sessionStateDataHandler);
        }
        public SessionHandler(ILog log, EClientType clientType)
        {
            _log = log ?? Singleton<DummyLog>.Instance;
            _sessionStateDataHandler = new SessionStateDataHandler(_log, clientType);
            _sessionStateDataHandler.SessionClose += SessionStateDataHandlerOnSessionClose;
            _sessionStateDataHandler.SessionBegin += SessionStateDataHandlerOnSessionStateBegin;
            _sessionStateDataHandler.SessionContinue += SessionStateDataHandlerOnSessionStateContinue;
            _sessionStateDataHandler.SessionError += (sender, args) =>
            {
                if (SessionError != null) SessionError(sender, args);
            };
            _parser.ValidFrameDetected += OnValidFrameDetected;
            Register(_sessionStateDataHandler);
        }
        //**--------------------------------------------------------------------------------
        #endregion

        #region Private Variable
        //**--------------------------------------------------------------------------------

        readonly SessionStateDataHandler _sessionStateDataHandler;
        readonly FrameParser _parser = new FrameParser();
        readonly Dictionary<UInt32, IRemoteDataHandler> _handlers = new Dictionary<UInt32, IRemoteDataHandler>();
        readonly Dictionary<UInt32, UInt32> _frameID = new Dictionary<UInt32, UInt32>();
        readonly ThreadSafeQueue<Frame> _frames = new ThreadSafeQueue<Frame>(100);
        readonly object _isConnectedLocker = new object();
        readonly object _isHandlersLocker = new object();

        bool _isConnected;
        private Frame _writeFrame;

        //**--------------------------------------------------------------------------------
        #endregion

        #region Protected
        //**--------------------------------------------------------------------------------

        private readonly ILog _log;
        private IPort _port;

        //**--------------------------------------------------------------------------------
        #endregion

        #region Properties
        //**--------------------------------------------------------------------------------

        public ILog Log
        {
            get { return _log; }
        }

        //**--------------------------------------------------------------------------------
        #endregion

        #region Events
        //**--------------------------------------------------------------------------------

        public event GeneralEventHandler<ConnectionFailedReason> SessionError;

        //**--------------------------------------------------------------------------------
        #endregion

        #region Public
        //**--------------------------------------------------------------------------------

        public void ConnectedStatusChanged(bool connected)
        {
            Log.Info("ConnectedStatusChanged " + connected);

            foreach (var handler in _handlers.Values)
            {
                lock (_isHandlersLocker)
                {
                    if (handler != _sessionStateDataHandler)
                    {
                        handler.IsConnected = connected;
                    }
                }
            }

            //uint i = 0;
            //while (true)
            //{
            //    IRemoteDataHandler handler;
            //    lock (_isHandlersLocker)
            //    {
            //       if (i >= _handlers.Values.Count) break;
            //       handler = _handlers[i];
            //    }
            //    if (handler != _sessionStateDataHandler)
            //    {
            //        handler.IsConnected = connected;
            //    }
            //    i++;
            //}
        }

        public void Connected(IPort port)
        {
            lock (_isConnectedLocker)
            {
                _isConnected = true;
            }

            if (_port != port)
            {
                _port = port;
                _port.DataReceived += PortOnDataReceived;
                _port.WriteResponsed += PortOnWriteResponsed;
            }

            _sessionStateDataHandler.IsConnected = true;
            ConnectedStatusChanged(true);
            _sessionStateDataHandler.Reset();
        }

        public void Disconnected()
        {
            lock (_isConnectedLocker)
            {
                _isConnected = false;
            }
            _sessionStateDataHandler.IsConnected = false;
            ConnectedStatusChanged(false);
        }

        public void Register(IRemoteDataHandler handler)
        {
            lock (_isHandlersLocker)
            {
                if (_handlers.ContainsKey(handler.Signature)) return;
                _handlers.Add(handler.Signature, handler);
                _frameID.Add(handler.Signature, 0);
            }
            handler.RequestSend += HandlerOnRequestSend;
            lock (_isConnectedLocker)
            {
                handler.IsConnected = _isConnected;
                handler.Registered();
            }
        }

        private void HandlerOnRequestSend(object sender, EventArgs<DataContainer> e)
        {
            var handler = sender as IRemoteDataHandler;
            if (handler != null) Send(handler.Signature, e.Data);
        }

        public int Send(UInt32 signature, DataContainer container)
        {
            var frame = new Frame();
            frame.Signature = signature;
            frame.FrameID = container.FrameNumber;
            frame.FunctionCode = container.FunctionCode;
            frame.DataLength = (uint)container.Data.Length;
            frame.Data = container.Data;

            if (_frames.Count == 0)
            {
                var data = frame.GetFrame();
                _port.Write(data);
                return container.FrameNumber;
            }
            return _frames.TryQueue(frame) ? container.FrameNumber : -1;
        }

        public void Close()
        {
            _sessionStateDataHandler.Disconnect();
            _parser.Reset();
        }

        //**--------------------------------------------------------------------------------
        #endregion

        #region Private Method
        //**--------------------------------------------------------------------------------

        private void SessionStateDataHandlerOnSessionStateBegin(object sender, EventArgs<ClientSoftwareInfo> eventArgs)
        {
            Log.Info("Session Create");
            int i = 0;
            var keys = new List<uint>();
            foreach (var key in _handlers.Keys)
            {
                keys.Add(key);
            }

            while (true)
            {
                IRemoteDataHandler handler;
                lock (_isHandlersLocker)
                {
                    i++;
                    if (i >= keys.Count) break;
                    handler = _handlers[keys[i]];
                }
                if (handler == _sessionStateDataHandler) continue;
                handler.IsConnected = true;
                handler.Reset();
            }
        }

        private void SessionStateDataHandlerOnSessionStateContinue(object sender, EventArgs eventArgs)
        {
            Log.Info("Session Continue");
            ConnectedStatusChanged(true);
        }

        private void SessionStateDataHandlerOnSessionClose(object sender, EventArgs eventArgs)
        {
            _log.Warn("Server going to close");
            _port.Close();
        }

        private void PortOnDataReceived(object sender, DataEventArgs e)
        {
            _parser.Parse(e.Data);
        }

        private void OnValidFrameDetected(object sender, EventArgs<Frame> e)
        {
            IRemoteDataHandler handler;
            var frame = e.Data;
            _log.In("Valid frame. Signature " + frame.Signature + " Length " + frame.Data.Length);
            if (_handlers.TryGetValue(frame.Signature, out handler))
            {
                handler.DataReceived(frame.FunctionCode, frame.Data, frame.FrameID);
            }
        }

        private void PortOnWriteResponsed(object sender, BooleanEventArgs e)
        {
            if (!e.Value)
            {
                _log.Error("Failed response");
            }
            else
            {
                if (!_frames.TryDequeue(out _writeFrame))
                {
                    return;
                }
            }

            if (_writeFrame != null)
            {
                var data = _writeFrame.GetFrame();
                _port.Write(data);
            }
            else
            {
                PortOnWriteResponsed(sender, e);
            }
        }

        //**--------------------------------------------------------------------------------
        #endregion
    }
}
