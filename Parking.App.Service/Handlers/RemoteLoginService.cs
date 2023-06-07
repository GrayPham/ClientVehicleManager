using Connect.Common;
using Connect.Common.Contract;
using Connect.Common.Interface;
using Connect.Common.Languages;
using Connect.Common.Logging;
using Connect.RemoteDataProvider.Common;
using Parking.App.Contract.Service;
using Parking.App.Contract.Setting;
using nsConnect.RemoteDataProvider.Client;
using nsFramework.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Parking.App.Service.Handlers
{
    public class RemoteLoginService : RemoteServiceBase, ILoginService
    {
        #region Constructor
        //**--------------------------------------------------------------------------------

        public RemoteLoginService(ILog log)
        {
            Log = log ?? Singleton<DummyLog>.Instance;
        }

        //**--------------------------------------------------------------------------------
        #endregion

        #region Private/Protected
        //**--------------------------------------------------------------------------------
        protected readonly ManualResetEvent _semaWaiting = new ManualResetEvent(false);
        protected ResultInfo _resultInfo = new ResultInfo();
        protected IList<ClientSessionHandlerInfo> _clientSessionHandlerBases;
        protected const int FunctionRequestLogin = 0x2001;
        protected const int FunctionResponseLogin = 0x2002;

        protected const int FunctionRequestReLogin = 0x2003;
        protected const int FunctionResponseReLogin = 0x2004;

        protected const int FunctionRequestChangePassWord = 0x2005;
        protected const int FunctionResponseChangePassWord = 0x2006;

        protected const int FunctionCreateSessionCompleted = 0x2007;

        protected const int FunctionRequestClientList = 0x2008;
        protected const int FunctionResponseClientList = 0x2009;

        protected const int FunctionRequestClientClosed = 0x2010;
        protected const int FunctionResponseClientClosed = 0x2011;

        protected const int FunctionRequestSend2Client = 0x2012;
        protected const int FunctionResponseSend2Client = 0x2013;
        protected const int FunctionMessageReceived = 0x2014;

        protected const int FunctionRequestLogOut = 0x2015;
        protected const int FunctionResponseLogOut = 0x2016;

        protected UInt16 CurrentRequest;

        public const int LoginAccountNotExists = 1;
        public const int LoginAccountNotActivity = 2;
        public const int LoginAccountNotBeginDate = 3;
        public const int LoginAccountNotEndDate = 4;
        public const int LoginAccountDateServer = 5;
        public const int LoginAccountErrorSystem = 6;
        public const int LoginNotConnected = 7;
        public const int LoginUnsupportFunctionCode = 8;
        public const int LoginCantAccessBranch = 13;
        //**--------------------------------------------------------------------------------
        #endregion

        #region IRemoteDataHandler
        //**--------------------------------------------------------------------------------

        public void RegisterType()
        {
            SerializationHelper.RegisterType<AccountInfo>(Log);
            SerializationHelper.RegisterType<SessionInfo>(Log);
        }

        public override uint Signature
        {
            get { return SSignature.LoginService; }
        }

        public override String ServiceName
        {
            get { return @"RemoteLoginService"; }
        }

        public override string ToString(ushort functionCode)
        {
            if (functionCode == FunctionRequestLogin) return "FunctionRequestLogin";
            if (functionCode == FunctionResponseLogin) return "FunctionResponseLogin";
            if (functionCode == FunctionCreateSessionCompleted) return "FunctionCreateSessionCompleted";
            if (functionCode == FunctionRequestClientList) return "FunctionRequestClientList";
            if (functionCode == FunctionResponseClientList) return "FunctionResponseClientList";
            if (functionCode == FunctionRequestClientClosed) return "FunctionRequestClientClosed";
            if (functionCode == FunctionResponseClientClosed) return "FunctionResponseClientClosed";
            return FunctionCode.ToString(functionCode);
        }

        public override void DataReceived(ushort functionCode, byte[] data, int frameNumber)
        {
            base.DataReceived(functionCode, data, frameNumber);
            switch (functionCode)
            {
                case FunctionCode.SessionReady:
                    IsReady = true;
                    break;
                case FunctionResponseLogin:
                    var sessionInfo = SerializationHelper.Deserialize<SessionInfo>(data);
                    OnResponseLogin(true, sessionInfo);
                    break;
                case FunctionResponseReLogin:
                    var reSessionInfo = SerializationHelper.Deserialize<SessionInfo>(data);
                    OnResponseReLogin(true, reSessionInfo);
                    break;
                case FunctionResponseChangePassWord:
                    _resultInfo = SerializationHelper.Deserialize<ResultInfo>(data);
                    _semaWaiting.Set();
                    break;
                case FunctionCreateSessionCompleted:
                    Log.Critical("Create Session Completed.");
                    if (ResponseSessionCompleted != null) ResponseSessionCompleted(this, new EventArgs<string>(""));
                    break;
                case FunctionResponseClientList:
                    Log.Info("FunctionResponseClientList");
                    _clientSessionHandlerBases = SerializationHelper.Deserialize<List<ClientSessionHandlerInfo>>(data);
                    _semaWaiting.Set();
                    break;
                case FunctionResponseClientClosed:
                    Log.Info("FunctionResponseClientClosed");
                    _clientSessionHandlerBases = SerializationHelper.Deserialize<List<ClientSessionHandlerInfo>>(data);
                    _semaWaiting.Set();
                    break;
                case FunctionResponseSend2Client:
                    Log.Info("FunctionResponseSend2Client");
                    _resultInfo = SerializationHelper.Deserialize<ResultInfo>(data);
                    _semaWaiting.Set();
                    break;
                case FunctionMessageReceived:
                    Log.Info("FunctionMessageReceived");
                    _resultInfo = SerializationHelper.Deserialize<ResultInfo>(data);
                    if (_resultInfo != null)
                    {
                        OnMessageReceived(_resultInfo);
                    }
                    break;
                case FunctionResponseLogOut:
                    Log.Info("FunctionResponseLogOut");
                    var result = SerializationHelper.Deserialize<ResultInfo>(data);
                    if (result != null)
                    {
                        if (ResponseLogOut != null) ResponseLogOut(this, new EventArgs<ResultInfo>(result));
                    }
                    break;
                default:
                    Log.Error("Unsupport function code");
                    OnResponseLogin(false, new SessionInfo() { Succeed = false, FailedNumber = ServiceHelpers.LoginUnsupportFunctionCode });
                    break;
            }
        }
        //**--------------------------------------------------------------------------------
        #endregion

        #region ILoginService
        //**--------------------------------------------------------------------------------
        public event ResponsedEventHandler<SessionInfo> ResponseLogin;
        public event ResponsedEventHandler<SessionInfo> ResponseReLogin;
        public event GeneralEventHandler<ResultInfo> ResponseLogOut;
        public event GeneralEventHandler<string> ResponseSessionCompleted;
        public event GeneralEventHandler<ResultInfo> MessageReceived;
        //**--------------------------------------------------------------------------------
        private void OnMessageReceived(ResultInfo info)
        {
            if (MessageReceived != null) MessageReceived(this, new EventArgs<ResultInfo>(_resultInfo));
        }
        //**--------------------------------------------------------------------------------
        public void RequestLogin(SessionInfo info)
        {
            if (!IsConnected || !IsReady)
            {
                info.FailedNumber = LoginNotConnected;
                info.Succeed = false;
                OnResponseLogin(false, info);
                return;
            }
            var data = SerializationHelper.Serialize(info);
            if (IsConnected) Send(FunctionRequestLogin, data);
        }
        public void RequestReLogin(SessionInfo info)
        {
            if (!IsConnected || !IsReady)
            {
                info.FailedNumber = LoginNotConnected;
                info.Succeed = false;
                OnResponseLogin(false, info);
                return;
            }
            var data = SerializationHelper.Serialize(info);
            if (IsConnected) Send(FunctionRequestReLogin, data);
        }
        protected virtual void OnResponseLogin(bool succeed, SessionInfo e)
        {
            var handler = ResponseLogin;
            if (handler != null) handler(this, new ResponsedEventArgs<SessionInfo>(succeed, e));
        }
        protected virtual void OnResponseLogOut(ResultInfo e)
        {
            var handler = ResponseLogOut;
            if (handler != null) handler(this, new EventArgs<ResultInfo>(e));
        }
        protected virtual void OnResponseReLogin(bool v, SessionInfo reSessionInfo)
        {
            var handler = ResponseReLogin;
            if (handler != null) handler(this, new ResponsedEventArgs<SessionInfo>(v, reSessionInfo));
        }
        public override void OnFailed()
        {
            OnResponseLogin(false, new SessionInfo() { Succeed = false, FailedNumber = LoginAccountErrorSystem });
        }
        //**--------------------------------------------------------------------------------
        public ResultInfo ChangePassWord(AccountInfo info)
        {
            if (!IsConnected)
            {
                return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
            }
            _semaWaiting.Reset();
            Send(FunctionRequestChangePassWord, SerializationHelper.Serialize(info));
            return _resultInfo;
        }
        public ResultInfo ChangePassWordAsyn(AccountInfo info)
        {
            if (!IsConnected)
            {

                return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
            }
            _semaWaiting.Reset();
            Send(FunctionRequestChangePassWord, SerializationHelper.Serialize(info));
            if (_semaWaiting.WaitOne(ConnectDatas.TimeOut))
            {
                return _resultInfo;
            }
            else
            {
                _resultInfo.Status = false;
                _resultInfo.ErrorMessage = FWLanguages.LResponseTimeout;
            }
            return _resultInfo;
        }
        //**--------------------------------------------------------------------------------
        public Task<ResultInfo> RequestClientList()
        {
            return Task.Run<ResultInfo>(() =>
            {
                if (!IsConnected)
                {
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo();
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    _clientSessionHandlerBases = null;
                    _semaWaiting.Reset();
                    Send(FunctionRequestClientList, SerializationHelper.Serialize(_requestInfo));
                    if (_semaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (_clientSessionHandlerBases == null)
                        {
                            _clientSessionHandlerBases = new List<ClientSessionHandlerInfo>();
                        }
                        dataSend.Result.Data = _clientSessionHandlerBases;
                        dataSend.Result.Status = true;
                        return dataSend.Result;
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    // SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    Log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                return dataSend.Result;
            });
        }
        public Task<ResultInfo> RequestClientClose(int ClientID)
        {
            return Task.Run<ResultInfo>(() =>
            {
                if (!IsConnected)
                {
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo();
                _requestInfo.ClientServiceID = ClientID;
                try
                {
                    Send(FunctionRequestClientClosed, SerializationHelper.Serialize(_requestInfo));
                }
                catch (Exception ex)
                {
                    Log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                return _resultInfo;
            });
        }
        public Task<ResultInfo> RequestClientCloseAsyn(int ClientID)
        {
            return Task.Run<ResultInfo>(() =>
            {
                if (!IsConnected)
                {
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo();
                _requestInfo.ClientServiceID = ClientID;
                try
                {
                    _clientSessionHandlerBases = null;
                    _semaWaiting.Reset();
                    Send(FunctionRequestClientClosed, SerializationHelper.Serialize(_requestInfo));
                    _resultInfo = new ResultInfo();
                    if (_semaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        return _resultInfo;
                    }
                    else
                    {
                        _resultInfo.Status = false;
                        _resultInfo.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    Log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                return _resultInfo;
            });
        }
        //**--------------------------------------------------------------------------------
        public ResultInfo RequestSend2Client(int clientID, RequestInfo info)
        {
            if (!IsConnected)
            {
                return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
            }
            var dataSend = new DataRequestInfo(clientID, info);
            try
            {
                _clientSessionHandlerBases = null;
                _semaWaiting.Reset();
                _resultInfo = new ResultInfo();
                Send(FunctionRequestSend2Client, SerializationHelper.Serialize(dataSend.Request));
            }
            catch (Exception ex)
            {
                Log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            return dataSend.Result;
        }
        public Task<ResultInfo> RequestSend2ClientAsyn(int clientID, RequestInfo info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                if (!IsConnected)
                {
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var dataSend = new DataRequestInfo(clientID, info);
                try
                {
                    _clientSessionHandlerBases = null;
                    _semaWaiting.Reset();
                    _resultInfo = new ResultInfo();
                    Send(FunctionRequestSend2Client, SerializationHelper.Serialize(dataSend.Request));
                    if (_semaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        return _resultInfo;
                    }
                    else
                    {
                        _resultInfo.Status = false;
                        _resultInfo.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    Log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                return dataSend.Result;
            });
        }
        //**--------------------------------------------------------------------------------
        public void RequestLogOut(SessionInfo info)
        {
            if (!IsConnected || !IsReady)
            {
                OnResponseLogOut(new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail });
                return;
            }
            var data = SerializationHelper.Serialize(info);
            if (IsConnected) Send(FunctionRequestLogOut, data);
        }
        //**--------------------------------------------------------------------------------
        #endregion
    }
}
