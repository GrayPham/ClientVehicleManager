using Connect.Common;
using Connect.Common.Common;
using Connect.Common.Contract;
using Connect.Common.Interface;
using Connect.Common.Logging;
using Connect.RemoteDataProvider.Client;
using Connect.RemoteDataProvider.Common;
using Connect.RemoteDataProvider.Interface;
using nsFramework.Common.Pattern;
using System;
using System.Threading;

namespace nsConnect.RemoteDataProvider.Client
{
    public class SessionStateDataHandler : IRemoteDataHandler
    {
        public SessionStateDataHandler(ILog log, String serial, String license)
        {
            _log = log ?? Singleton<DummyLog>.Instance;

            SerializationHelper.RegisterType<ClientSoftwareInfo>(_log);
            string mac = SerializationHelper.GetMacAddress() ?? "FFFFFFFF";
            _log.Info(mac);

            _clientSoftware = new ClientSoftwareInfo { LicenseKey = license, Mac = mac, SerialNumber = serial };
            _dataContainer = new DataContainer(1, SerializationHelper.Serialize(_clientSoftware), frameID++);
            _sessionSync = new EventArgs<DataContainer>(_dataContainer);

        }
        public SessionStateDataHandler(ILog log, EClientType clientType)
        {
            _log = log ?? Singleton<DummyLog>.Instance;

            SerializationHelper.RegisterType<ClientSoftwareInfo>(_log);
            string mac = SerializationHelper.GetMacAddress() ?? "FFFFFFFF";
            _log.Info(mac);
            _clientSoftware = new ClientSoftwareInfo
            {
                LicenseKey = ApplicationInfo.LicenseKey,
                Mac = mac,
                SerialNumber = ApplicationInfo.SeriKey,
                VersionKey = ApplicationInfo.VersionCode,
                LicenseSoftware = ApplicationInfo.SoftwareKey,
                ePlatform = EPlatform.Windows,
                eClientType = clientType,
                ClientTime = DateTime.Now,
                HardwareID = ApplicationInfo.PCID,
                CPUID = ApplicationInfo.CpuIdKey + "-" + ApplicationInfo.InstallID.Replace("-", ""),
            };
            _clientSoftware.GetAccessKey(ApplicationInfo.AccessKey);
            _dataContainer = new DataContainer(1, SerializationHelper.Serialize(_clientSoftware), frameID++);
            _sessionSync = new EventArgs<DataContainer>(_dataContainer);
        }

        #region Private
        //**--------------------------------------------------------------------------------

        private readonly ILog _log;
        private const uint SignID = 0;
        private bool _isConnected;
        private DataContainer _dataContainer;
        private EventArgs<DataContainer> _sessionSync;
        private ClientSoftwareInfo _clientSoftware;
        private int frameID;

        //**--------------------------------------------------------------------------------
        #endregion

        #region Propeties
        //**--------------------------------------------------------------------------------

        public uint Signature
        {
            get { return SignID; }
        }

        //**--------------------------------------------------------------------------------
        #endregion

        #region Event
        //**--------------------------------------------------------------------------------

        public event GeneralEventHandler<DataContainer> RequestSend;
        public event EventHandler SessionContinue;
        public event EventHandler SessionClose;
        public event GeneralEventHandler<ClientSoftwareInfo> SessionBegin;
        public event GeneralEventHandler<ConnectionFailedReason> SessionError;

        //**--------------------------------------------------------------------------------
        #endregion

        #region Public
        //**--------------------------------------------------------------------------------
        public int GetClientID() { return 0; }
        public void UpdateSession(ClientSoftwareInfo clientSoftware)
        {
            _clientSoftware = clientSoftware;
            _dataContainer = new DataContainer(1, SerializationHelper.Serialize(_clientSoftware), frameID++);
            _sessionSync = new EventArgs<DataContainer>(_dataContainer);
            if (!_isConnected) return;
            if (RequestSend != null) RequestSend(this, _sessionSync);
        }

        public void DataReceived(ushort functionCode, byte[] data, int frameID)
        {
            if (data == null)
            {
                _log.Error("Null Data");
                return;
            }
            _log.In("Data size: " + data.Length + " bytes");
            if (functionCode == ConnectionHelper.FunctionCreateSession)
            {
                _clientSoftware.ValueID = BitConverter.ToInt32(data, 0);
                if (SessionBegin != null) SessionBegin(this, new EventArgs<ClientSoftwareInfo>(_clientSoftware));
                _log.Info("New Session created ID = " + _clientSoftware.ValueID);
            }
            else if (functionCode == ConnectionHelper.FunctionReconnectSession)
            {
                if (SessionContinue != null) SessionContinue(this, EventArgs.Empty);
                _log.Info("Session continue");
            }
            else if (functionCode == ConnectionHelper.FunctionCloseSession)
            {
                _clientSoftware.ValueID = 0;
                if (SessionClose != null) SessionClose(this, EventArgs.Empty);
            }
            else if (functionCode == ConnectionHelper.FunctionSessionError)
            {
                if (data.Length == 1)
                {
                    var errorCode = data[0];
                    switch (errorCode)
                    {
                        case ConnectionHelper.InvalidCommand:
                            _log.Critical("Invalid Command");
                            break;
                        default:
                            var reason = ConnectionHelper.GetFailedReason(errorCode);
                            _log.Warn("Error Code " + errorCode);
                            if (SessionError != null) SessionError(this, new EventArgs<ConnectionFailedReason>(reason));
                            break;
                    }
                }
                else
                {
                    _log.Error("Feedback data length's not good");
                }
            }
            else
            {
                _log.Error("Function Code error");
            }
        }

        public bool IsConnected
        {
            set
            {
                _isConnected = value;
            }
        }

        public void Reset()
        {
            if (!_isConnected) return;
            if (RequestSend != null) RequestSend(this, _sessionSync);
        }
        public void Registered(EClientType eClientType)
        {

        }

        public void Registered(int clientID, EClientType eClientType)
        {

        }

        public void Registered()
        {

        }
        public void Registered(int clientID)
        {

        }
        public void UnRegistered()
        {

        }

        public void Disconnect()
        {
            if (!_isConnected) return;
            _dataContainer.FunctionCode = ConnectionHelper.FunctionCloseSession;
            if (RequestSend != null) RequestSend(this, _sessionSync);
            Thread.Sleep(100);
            if (SessionClose != null) SessionClose(this, EventArgs.Empty);
        }


        //**--------------------------------------------------------------------------------
        #endregion

    }
}
