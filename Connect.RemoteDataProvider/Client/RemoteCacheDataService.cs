using Connect.Common.Contract;
using Connect.Common;
using Connect.Common.Interface;
using Connect.Common.Logging;
using Server.Contract.Session;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Connect.RemoteDataProvider.Interface;
using Connect.RemoteDataProvider.Common;
using Connect.Common.Languages;
using Connect.Common.Helper;
using nsFramework.Common.Pattern;
using Connect.Common.Collection;
using System.Reflection;
using Connect.Common.Common;

namespace nsConnect.RemoteDataProvider.Client
{
    public abstract class RemoteCacheDataService<TInfo> : FuncClientDataService<TInfo>, IRemoteDataHandler, ICacheDataService<TInfo> where TInfo : IInfo<TInfo>, new()
    {
        //**--------------------------------------------------------------------------------

        #region Constructor
        protected object DictionaryLocker = new object();
        public Dictionary<Guid, DataRequestInfo> WaitingRequests = new Dictionary<Guid, DataRequestInfo>();
        public RemoteCacheDataService(ILog log, bool isDataSync, uint signature, string name) : base(log)
        {
            Signature = signature;
            _log = log ?? Singleton<DummyLog>.Instance;
            Name = name;
            IsDataSync = isDataSync;
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Declare Member
        protected Queue<Action> Queue = new Queue<Action>();

        protected class CacheDataClient<T> : IIdentify
        {
            public String Name { get; set; }
            public Boolean IsAddedNotify { get; set; }
            public Boolean IsRemovedNotify { get; set; }
            public Boolean IsCustomizedNotify { get; set; }
            public Boolean IsUpdatedNotify { get; set; }
            public GeneralEventHandler<int> ReadyListener { get; set; }
            public GeneralEventHandler<T> AddedListener { get; set; }
            public GeneralEventHandler<object> RemovedListener { get; set; }
            public GeneralEventHandler<ResultInfo> CustomizedListener { get; set; }
            public GeneralEventHandler<T> UpdatedListener { get; set; }
            public GeneralEventHandler<IList<T>> ListAddedListener { get; set; }
            public GeneralEventHandler<IList<object>> ListRemovedListener { get; set; }
            public GeneralEventHandler<IList<T>> ListUpdatedListener { get; set; }
            public object ValueID { get; set; }
            public EClientType ClientType { get; set; } = EClientType.None;
        }

        protected String Name;
        protected readonly object SyncRootClients = new object();
        protected readonly Dictionary<int, CacheDataClient<TInfo>> Clients = new Dictionary<int, CacheDataClient<TInfo>>();
        protected readonly object DataSyncLocker = new object();
        protected Boolean IsDataSyncComplited;
        protected Boolean IsDataSync;
        protected int FrameID = 0;
        protected RequestInfo _requestInfo;
        protected ResultInfo _tmpResultInfo = new ResultInfo();
        protected ResultInfo _resultInfo = new ResultInfo();
        
        public ILog Log => _log;
        protected void SetErrored(string className, string message, string trac, string data, Exception ex)
        {
            _log.SError(className, message, trac, data);
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Properties
        [Description("Signature service identifier")]
        public uint Signature { get; protected set; }
        [Description("Server connection status")]
        public virtual bool IsConnected { get; set; }
        [Description("Data Sync status")]
        public bool IsCustomerDataSync { get; set; } = false;
        public bool IsRemote
        {
            get { return true; }
        }
        [Description("Data is synchronized and saved from the server, only works when IsCustomerDataSync is true.")]
        public IInfoCollection<TInfo> Data { get; protected set; }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Client Handler
        protected virtual int CaculatorID(int clientID)
        {
            if (clientID > 0)
            {
                return clientID;
            }
            else
            {
                return GetMaxID() + 1;
            }

        }
        protected virtual Int32 GetMaxID()
        {
            var max = Clients.Count;
            foreach (var item in Clients)
            {
                if (item.Key > max) max = item.Key;
            }
            return max;
        }
        #region Helper

        #endregion

        #region Register,Edit Client
        [Description("All service register.")]
        public virtual int GetClientID() { return 0; }

        protected virtual CacheDataClient<TInfo> GetClient(int clientID)
        {
            CacheDataClient<TInfo> client;
            lock (SyncRootClients)
            {
                if (!Clients.TryGetValue(clientID, out client))
                    throw new Exception(@"Invalid client");
            }
            return client;
        }
        protected virtual IList<CacheDataClient<TInfo>> CloneClientList()
        {
            var list = new List<CacheDataClient<TInfo>>();
            lock (SyncRootClients)
            {
                foreach (var client in Clients.Values)
                {
                    list.Add(client);
                }
            }
            return list;
        }

        public virtual int RegisterClient(string name, GeneralEventHandler<int> handler)
        {
            int id;
            lock (SyncRootClients)
            {
                id = GetMaxID() + 1;
            }

            var client = new CacheDataClient<TInfo> { ValueID = id, Name = name };

            lock (SyncRootClients)
            {
                Clients.Add(id, client);
            }

            lock (DataSyncLocker)
            {
                if (IsDataSyncComplited)
                {
                    handler.Invoke(this, new EventArgs<int>(id));
                }
                else
                {
                    client.ReadyListener += handler;
                }
            }

            return id;
        }
        public virtual int RegisterClient(string name, GeneralEventHandler<int> handler, int clientID)
        {
            int id;
            lock (SyncRootClients)
            {
                id = CaculatorID(clientID);
            }

            var client = new CacheDataClient<TInfo> { ValueID = id, Name = name };

            lock (SyncRootClients)
            {
                Clients.Add(id, client);
            }

            lock (DataSyncLocker)
            {
                if (IsDataSyncComplited)
                {
                    handler.Invoke(this, new EventArgs<int>(id));
                }
                else
                {
                    client.ReadyListener += handler;
                }
            }

            return id;
        }
        public virtual int RegisterClient(string name, GeneralEventHandler<int> handler, EClientType eClientType)
        {
            int id;
            lock (SyncRootClients)
            {
                id = GetMaxID() + 1;
            }

            var client = new CacheDataClient<TInfo> { ValueID = id, Name = name, ClientType = eClientType };

            lock (SyncRootClients)
            {
                Clients.Add(id, client);
            }

            lock (DataSyncLocker)
            {
                if (IsDataSyncComplited)
                {
                    handler.Invoke(this, new EventArgs<int>(id));
                }
                else
                {
                    client.ReadyListener += handler;
                }
            }

            return id;
        }
        public virtual int RegisterClient(string name, GeneralEventHandler<int> handler, int clientID, EClientType eClientType)
        {
            int id;
            lock (SyncRootClients)
            {
                id = CaculatorID(clientID);
            }

            var client = new CacheDataClient<TInfo> { ValueID = id, Name = name, ClientType = eClientType };

            lock (SyncRootClients)
            {
                Clients.Add(id, client);
            }

            lock (DataSyncLocker)
            {
                if (IsDataSyncComplited)
                {
                    handler.Invoke(this, new EventArgs<int>(id));
                }
                else
                {
                    client.ReadyListener += handler;
                }
            }

            return id;
        }
        public virtual void RemoveClient(int clientID)
        {
            if (!Clients.ContainsKey(clientID)) return;
            lock (SyncRootClients)
            {
                Clients.Remove(clientID);
            }
        }
        public virtual void Registered()
        {
            // No Code          
        }
        public virtual void Registered(int clientID)
        {
            // No Code          
        }
        public virtual void UnRegistered()
        {
            // No Code
        }
        public virtual void Registered(EClientType eClientType)
        {
            // No Code   
        }
        public virtual void Registered(int clientID, EClientType eClientType)
        {
            // No Code   
        }
        static public void RegisterType(ILog log)
        {
            SerializationHelper.RegisterType<TInfo>(log);
            SerializationHelper.RegisterType<List<TInfo>>(log);
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Setting Notify
        public virtual void SetNotifyAdded(int clientID, bool notify)
        {
            var client = GetClient(clientID);
            client.IsAddedNotify = notify;
        }
        public virtual void SetNotifyRemoved(int clientID, bool notify)
        {
            var client = GetClient(clientID);
            client.IsRemovedNotify = notify;
        }
        public virtual void SetNotifyCustomized(int clientID, bool notify)
        {
            var client = GetClient(clientID);
            client.IsCustomizedNotify = notify;
        }
        public virtual void SetNotifyUpdated(int clientID, bool notify)
        {
            var client = GetClient(clientID);
            client.IsUpdatedNotify = notify;
        }
        public virtual void SetAddedListener(int clientID, GeneralEventHandler<TInfo> listener)
        {
            var client = GetClient(clientID);
            client.IsAddedNotify = true;
            client.AddedListener += listener;
        }
        public virtual void SetRemovedListener(int clientID, GeneralEventHandler<object> listener)
        {
            var client = GetClient(clientID);
            client.IsRemovedNotify = true;
            client.RemovedListener += listener;
        }
        public virtual void SetCustomizedListener(int clientID, GeneralEventHandler<ResultInfo> listener)
        {
            var client = GetClient(clientID);
            client.IsCustomizedNotify = true;
            client.CustomizedListener += listener;
        }
        public virtual void SetUpdatedListener(int clientID, GeneralEventHandler<TInfo> listener)
        {
            var client = GetClient(clientID);
            client.IsUpdatedNotify = true;
            client.UpdatedListener = listener;
        }
        public virtual void SetListAddedListener(int clientID, GeneralEventHandler<IList<TInfo>> listener)
        {
            var client = GetClient(clientID);
            client.ListAddedListener += listener;
        }
        public virtual void SetListRemovedListener(int clientID, GeneralEventHandler<IList<object>> listener)
        {
            var client = GetClient(clientID);
            client.ListRemovedListener += listener;
        }
        public virtual void SetListUpdatedListener(int clientID, GeneralEventHandler<IList<TInfo>> listener)
        {
            var client = GetClient(clientID);
            client.ListUpdatedListener += listener;
        }
        #endregion

        #endregion

        //**--------------------------------------------------------------------------------

        #region Client Update
        protected virtual void OnAdded(TInfo info)
        {

            if (IsDataSync && Data != null) Data.Add(info);
            var list = CloneClientList();
            foreach (var client in list)
            {
                if (!client.IsAddedNotify) continue;
                if (client.AddedListener == null) continue;
                client.AddedListener.Invoke(this, new EventArgs<TInfo>(info));
            }
        }
        protected virtual void OnRemoved(object id)
        {

            if (IsDataSync && Data != null) Data.Remove(id);
            var list = CloneClientList();
            foreach (var client in list)
            {
                if (!client.IsRemovedNotify) continue;
                if (client.RemovedListener == null) continue;
                client.RemovedListener.Invoke(this, new EventArgs<object>(id));
            }
        }
        protected virtual void OnCustomized(ResultInfo data)
        {
            var list = CloneClientList();
            foreach (var client in list)
            {
                if (!client.IsCustomizedNotify) continue;
                if (client.CustomizedListener == null) continue;
                client.CustomizedListener.Invoke(this, new EventArgs<ResultInfo>(data));
            }
        }
        protected virtual void OnUpdate(TInfo info)
        {

            if (IsDataSync && Data != null) Data.Update(info);
            var list = CloneClientList();
            foreach (var client in list)
            {
                if (!client.IsUpdatedNotify) continue;
                if (client.UpdatedListener == null) continue;
                client.UpdatedListener.Invoke(this, new EventArgs<TInfo>(info));
            }
        }
        protected virtual void OnListAdded(IList<TInfo> infos)
        {
            if (IsDataSync && Data != null)
            {
                foreach (var info in infos)
                {
                    Data.Add(info);
                }
            }
            var list = CloneClientList();
            foreach (var client in list)
            {
                if (!client.IsAddedNotify) continue;
                if (client.ListAddedListener == null) continue;
                client.ListAddedListener.Invoke(this, new EventArgs<IList<TInfo>>(infos));

            }
        }
        protected virtual void OnListRemoved(IList<object> ids)
        {
            if (IsDataSync && Data != null)
            {
                foreach (var id in ids)
                {
                    Data.Remove(id);
                }
            }
            var list = CloneClientList();
            foreach (var client in list)
            {
                if (!client.IsRemovedNotify) continue;
                if (client.ListRemovedListener == null) continue;
                client.ListRemovedListener.Invoke(this, new EventArgs<IList<object>>(ids));
            }
        }
        protected virtual void OnListUpdate(IList<TInfo> infos)
        {
            if (IsDataSync && Data != null)
            {
                foreach (var info in infos)
                {
                    Data.Update(info);
                }
            }
            var list = CloneClientList();
            foreach (var client in list)
            {
                if (!client.IsUpdatedNotify) continue;
                if (client.ListUpdatedListener == null) continue;
                client.ListUpdatedListener.Invoke(this, new EventArgs<IList<TInfo>>(infos));
            }
        }
        protected virtual void OnListSync()
        {
            var list = CloneClientList();
            foreach (var client in list)
            {
                if (client.ReadyListener == null) continue;
                client.ReadyListener.Invoke(this, new EventArgs<int>((int)client.ValueID));
                Delegate.RemoveAll(client.ReadyListener, client.ReadyListener);
            }
        }

        #endregion

        //**--------------------------------------------------------------------------------

        #region BulkInsert
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestBulkInsert(int clientID, IList<TInfo> infos)
        {
            try
            {
                //lock (DataSyncLocker)
                //{
                foreach (var item in infos)
                {
                    DefaultInsert(item);
                }
                var _requestInfo = new RequestInfo() { Data = infos };
                FrameID++;
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestLargeImportInfo, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                Send(FunctionCode.RequestLargeImportInfo, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = 0 };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> RequestBulkInsertAsyn(int clientID, IList<TInfo> infos)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    foreach (var item in infos)
                    {
                        DefaultInsert(item);
                    }
                    FrameID++;
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = infos;
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestLargeImportInfo, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestLargeImportInfo, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                // if (!(dataSend.Result.Data is List<TInfo>)) dataSend.Result.Data = JsonHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(infos), ex);
                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(infos), ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Insert
        protected abstract void DefaultInsert(TInfo info);
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestMDInsert(int clientID, TInfo info,object listDetail)
        {
            try
            {
                //lock (DataSyncLocker)
                //{
                DefaultInsert(info);
                FrameID++;
                var _requestInfo = new RequestInfo() { Data = info, Details = listDetail };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestMDAdd, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }

                Send(FunctionCode.RequestMDAdd, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = FrameID };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> RequestMDInsertAsyn(int clientID, TInfo info, object listDetail)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    FrameID++;
                    DefaultInsert(info);
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = info;
                    dataSend.Request.Details = listDetail;
                    dataSend.Request.Owner = SessionDatas.GetOwner();
                    dataSend.Request.UserName = SessionDatas.GetLoginUser();
                    dataSend.Request.UserID = "" + SessionDatas.GetUserID();
                    dataSend.Request.BranchID = SessionDatas.GetBranchID();
                    dataSend.Request.BranchCode = SessionDatas.GetBranchCode();
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestMDAdd, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestMDAdd, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                if (!(dataSend.Result.Data is TInfo))
                                    dataSend.Result.Data = JsonHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        //SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                        //
                        Log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);

                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestInsert(int clientID, TInfo info)
        {
            try
            {
                //lock (DataSyncLocker)
                //{
                DefaultInsert(info);
                FrameID++;
                var _requestInfo = new RequestInfo() { Data = info };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }

                Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = FrameID };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> RequestInsertAsyn(int clientID, TInfo info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    FrameID++;
                    DefaultInsert(info);
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = info;
                    dataSend.Request.Owner = SessionDatas.GetOwner();
                    dataSend.Request.UserName = SessionDatas.GetLoginUser();
                    dataSend.Request.UserID = "" + SessionDatas.GetUserID();
                    dataSend.Request.BranchID = SessionDatas.GetBranchID();
                    dataSend.Request.BranchCode = SessionDatas.GetBranchCode();
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                if (!(dataSend.Result.Data is TInfo))
                                    dataSend.Result.Data = JsonHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        //SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                        //
                        Log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);

                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestInsert(int clientID, IList<TInfo> infos)
        {
            try
            {
                //lock (DataSyncLocker)
                //{
                foreach (var item in infos)
                {
                    DefaultInsert(item);
                }
                var _requestInfo = new RequestInfo() { Data = infos };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                FrameID++;
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestAddList, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }

                Send(FunctionCode.RequestAddList, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = FrameID };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(infos), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> RequestInsertAsyn(int clientID, IList<TInfo> infos)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    foreach (var item in infos)
                    {
                        DefaultInsert(item);
                    }
                    FrameID++;
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = infos;
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestAddList, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestAddList, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                if (!(dataSend.Result.Data is List<TInfo>))
                                    dataSend.Result.Data = JsonHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(infos), ex);
                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(infos), ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }
        //**--------------------------------------------------------------------------------
        #endregion

        //**--------------------------------------------------------------------------------

        #region Update
        protected abstract void DefaultModified(TInfo info);
        public virtual ResultInfo RequestMDUpdate(int clientID, TInfo info, object details)
        {
            try
            {
                //lock (DataSyncLocker)
                //{
                DefaultModified(info);
                FrameID++;
                var _requestInfo = new RequestInfo() { Data = info, Details = details };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestMDUpdate, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }

                Send(FunctionCode.RequestMDUpdate, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = FrameID };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> RequestMDUpdateAsyn(int clientID, TInfo info, object details)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    //lock (DataSyncLocker)
                    //{
                    FrameID++;
                    DefaultModified(info);
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = info;
                    dataSend.Request.Details = details;
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestMDUpdate, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestMDUpdate, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                if (!(dataSend.Result.Data is TInfo))
                                    dataSend.Result.Data = JsonHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    //}
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestUpdate(int clientID, TInfo info)
        {
            try
            {
                //lock (DataSyncLocker)
                //{
                DefaultModified(info);
                FrameID++;
                var _requestInfo = new RequestInfo() { Data = info };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestUpdate, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }

                Send(FunctionCode.RequestUpdate, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = FrameID };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> RequestUpdateAsyn(int clientID, TInfo info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    //lock (DataSyncLocker)
                    //{
                    FrameID++;
                    DefaultModified(info);
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = info;
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestUpdate, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestUpdate, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                if (!(dataSend.Result.Data is TInfo))
                                    dataSend.Result.Data = JsonHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    //}
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestUpdateList(int clientID, IList<TInfo> infos)
        {
            try
            {
                //lock (DataSyncLocker)
                //{
                FrameID++;
                foreach (var item in infos)
                {
                    DefaultModified(item);
                }
                var _requestInfo = new RequestInfo() { Data = infos };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestUpdateList, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }

                Send(FunctionCode.RequestUpdateList, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = FrameID };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(infos), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> RequestUpdateListAsyn(int clientID, IList<TInfo> infos)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    //lock (DataSyncLocker)
                    //{
                    FrameID++;
                    foreach (var item in infos)
                    {
                        DefaultModified(item);
                    }
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = infos;
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestUpdateList, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestUpdateList, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                if (!(dataSend.Result.Data is List<TInfo>))
                                    dataSend.Result.Data = JsonHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(infos), ex);
                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    //}
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(infos), ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Update Or Insert
        public virtual ResultInfo RequestMDInsertOrUpdate(int clientID, TInfo info, object details)
        {
            try
            {
                //lock (DataSyncLocker)
                //{
                DefaultInsert(info);
                DefaultModified(info);

                FrameID++;
                var _requestInfo = new RequestInfo() { Data = info, Details = details };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestMDIorU, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }

                Send(FunctionCode.RequestMDIorU, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = FrameID };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> RequestMDInsertOrUpdateAsyn(int clientID, TInfo info,object details)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {

                    FrameID++;
                    DefaultInsert(info);
                    DefaultModified(info);
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = info;
                    dataSend.Request.Details = details;
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestMDIorU, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestMDIorU, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                if (!(dataSend.Result.Data is TInfo))
                                    dataSend.Result.Data = JsonHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestInsertOrUpdate(int clientID, TInfo info)
        {
            try
            {
                //lock (DataSyncLocker)
                //{
                DefaultInsert(info);
                DefaultModified(info);

                FrameID++;
                var _requestInfo = new RequestInfo() { Data = info };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestInsertOrUpdate, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }

                Send(FunctionCode.RequestInsertOrUpdate, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = FrameID };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> RequestInsertOrUpdateAsyn(int clientID, TInfo info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {

                    FrameID++;
                    DefaultInsert(info);
                    DefaultModified(info);
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = info;
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestInsertOrUpdate, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestInsertOrUpdate, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                if (!(dataSend.Result.Data is TInfo))
                                    dataSend.Result.Data = JsonHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestInsertOrUpdate(int clientID, IList<TInfo> info)
        {
            try
            {
                //lock (DataSyncLocker)
                //{
                FrameID++;
                foreach (var item in info)
                {
                    DefaultInsert(item);
                    DefaultModified(item);
                }

                var _requestInfo = new RequestInfo() { Data = info };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestInsertOrUpdateList, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }

                Send(FunctionCode.RequestInsertOrUpdateList, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = FrameID };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> RequestInsertOrUpdateAsyn(int clientID, IList<TInfo> info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    FrameID++;
                    foreach (var item in info)
                    {
                        DefaultInsert(item);
                        DefaultModified(item);
                    }
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = info;
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestInsertOrUpdateList, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestInsertOrUpdateList, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                if (!(dataSend.Result.Data is List<TInfo>))
                                    dataSend.Result.Data = JsonHelper.JsonToListInfo<object>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(info), ex);
                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(info), ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });

        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Remove
        public virtual ResultInfo RequestRemove(int clientID, object id)
        {
            try
            {
                //lock (DataSyncLocker)
                //{
                FrameID++;
                var _requestInfo = new RequestInfo() { Data = id };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestRemove, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }

                Send(FunctionCode.RequestRemove, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = FrameID };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "" + id, ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> RequestRemoveAsyn(int clientID, object id)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    //lock (DataSyncLocker)
                    //{
                    FrameID++;
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = id;
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestRemove, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestRemove, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                // if (!(dataSend.Result.Data is List<TInfo>))
                                dataSend.Result.Data = JsonHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "" + id, ex);
                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    //}
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "" + id, ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestRemoveList(int clientID, IList<object> ids)
        {

            try
            {
                //lock (DataSyncLocker)
                //{
                FrameID++;
                var _requestInfo = new RequestInfo() { Data = ids };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                if (!IsConnected)
                {
                    Queue.Enqueue(() => { Send(FunctionCode.RequestRemoveList, SerializationHelper.Serialize(_requestInfo), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                Send(FunctionCode.RequestRemoveList, SerializationHelper.Serialize(_requestInfo), FrameID);
                return new ResultInfo() { Status = true, Record = FrameID };
                //}
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> RequestRemoveListAsyn(int clientID, IList<object> ids)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    //lock (DataSyncLocker)
                    //{
                    FrameID++;
                    var dataSend = new DataRequestInfo(clientID);
                    dataSend.Request.Data = ids;
                    if (!IsConnected)
                    {
                        Queue.Enqueue(() => { Send(FunctionCode.RequestRemoveList, SerializationHelper.Serialize(dataSend.Request), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestRemoveList, SerializationHelper.Serialize(dataSend.Request), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result != null)
                            {
                                //if (!(dataSend.Result.Data is List<TInfo>))
                                dataSend.Result.Data = JsonHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                            else
                            {
                                return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LErrorUnknown };
                            }
                        }
                        else
                        {
                            return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = FWLanguages.LResponseTimeout };
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                        return new ResultInfo() { Status = false, Record = FrameID, ErrorMessage = ex.Message };
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    //}
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Event
        public event GeneralEventHandler<ResultInfo> ResponsedEdit;
        public event GeneralEventHandler<ResultInfo> RequestFailed;
        public event GeneralEventHandler<DataContainer> RequestSend;

        protected virtual void OnRequestFailed(ResultInfo resultInfo)
        {
            if (resultInfo != null)
            {
                WaitingRelease(resultInfo);
            }
            GeneralEventHandler<ResultInfo> handler = RequestFailed;
            if (handler != null) handler(this, new EventArgs<ResultInfo>(resultInfo));
        }
        protected virtual void OnResponsed(ResultInfo resultInfo)
        {
            if (resultInfo != null)
            {
                WaitingRelease(resultInfo);
            }
            GeneralEventHandler<ResultInfo> handler = ResponsedEdit;
            if (handler != null) handler(this, new EventArgs<ResultInfo>(resultInfo));
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region IRemoteDataHandler Interface
        protected  virtual void Send(ushort code, byte[] data, int frameID)
        {
            if (!IsConnected)
            {
                Log.Warn(Name + " Failed; FC:" + FunctionCode.ToString(code) + "; FrameID:" + frameID + "/" + data.Length);
                return;
            }
            Log.Info(Name + "; FC:" + FunctionCode.ToString(code) + "; FrameID:" + frameID + "/" + data.Length);
            if (!IsConnected) return;
            var container = new DataContainer(code, data, frameID);
            var handler = RequestSend;
            if (handler != null) handler(this, new EventArgs<DataContainer>(container));
        }
        public virtual void DataReceived(ushort functionCode, byte[] data, int frameID)
        {
            Log.Info(Name + "; FC:" + FunctionCode.ToString(functionCode) + "; FrameID:" + frameID + ";/" + data.Length);
            switch (functionCode)
            {
                case FunctionCode.Customized:
                    OnCustomized(SerializationHelper.Deserialize<ResultInfo>(data));
                    break;
                case FunctionCode.SessionReady:
                    if (IsDataSync)
                    {
                        lock (DataSyncLocker)
                        {
                            Send(FunctionCode.RequestList, new byte[0], FrameID++);
                        }
                    }
                    else
                    {
                        Data = new InfoCollection<TInfo>(new List<TInfo>());
                        lock (DataSyncLocker)
                        {
                            IsDataSyncComplited = true;
                            ReadQueue();
                        }
                        OnListSync();
                    }
                    break;

                case FunctionCode.ResponseList:
                    var list = SerializationHelper.Deserialize<List<TInfo>>(data);
                    Data = new InfoCollection<TInfo>(list);
                    lock (DataSyncLocker)
                    {
                        IsDataSyncComplited = true;
                        ReadQueue();
                    }
                    OnListSync();
                    break;
                case FunctionCode.Added:
                    OnAdded(SerializationHelper.Deserialize<TInfo>(data));
                    break;
                case FunctionCode.Removed:
                    OnRemoved(SerializationHelper.Deserialize<object>(data));
                    break;
                case FunctionCode.Updated:
                    OnUpdate(SerializationHelper.Deserialize<TInfo>(data));
                    break;
                case FunctionCode.ListAdded:
                    OnListAdded(SerializationHelper.Deserialize<IList<TInfo>>(data));
                    break;
                case FunctionCode.ListRemoved:
                    OnListRemoved(SerializationHelper.Deserialize<IList<object>>(data));
                    break;
                case FunctionCode.ListUpdated:
                    OnListUpdate(SerializationHelper.Deserialize<IList<TInfo>>(data));
                    break;
                case FunctionCode.RequestFailed:
                    OnRequestFailed(SerializationHelper.Deserialize<ResultInfo>(data));
                    break;
                case FunctionCode.ResponseEdit:
                   var responseEditInfo = SerializationHelper.Deserialize<ResultInfo>(data);
                    WaitingRelease_ResponseEdit(responseEditInfo);
                    OnResponsed(responseEditInfo);
                    break;

                #region Response
                case FunctionCode.ResponseSynchronized:
                case FunctionCode.ResponseTryGetID:
                case FunctionCode.ResponseGetDataAll:
                case FunctionCode.ResponseGetDataIsActivity:
                case FunctionCode.ResponseGetDataIsActivityByBranch:
                case FunctionCode.ResponseGetDataIsActivityByGroup:
                case FunctionCode.ResponseGetDataNotIsActivity:
                case FunctionCode.ResponseExecTSQL:
                case FunctionCode.ResponseExecTSQLToEn:
                case FunctionCode.ResponseExecTSQLTable:
                case FunctionCode.ResponseSearchInfo:
                case FunctionCode.ResponseExecuteCommand:
                case FunctionCode.ResponseUpdateCache:
                case FunctionCode.ResponsePaginateInfo:
                case FunctionCode.ResponseLargeImportInfo:
                case FunctionCode.ResponseListNotifyAdded:
                case FunctionCode.ResponseNotifyAdded:
                case FunctionCode.ResponseListNotifyUpdated:
                case FunctionCode.ResponseNotifyUpdated:
                case FunctionCode.ResponseListNotifyRemoved:
                case FunctionCode.ResponseNotifyRemoved:
                case FunctionCode.ResponseNotifyCustomized:
                case FunctionCode.ResponseGetDataIsActivityByGroupCode:
                case FunctionCode.ResponseGetDataByOption:
                case FunctionCode.ResponseTryGetBySCondition:
                case FunctionCode.ResponseGetDataBySCondition:
                case FunctionCode.ResponseCheckExistsBySCondition:
                case FunctionCode.ResponseGetByWhere:
                case FunctionCode.ResponseValidateInfo:
                    _tmpResultInfo = SerializationHelper.Deserialize<ResultInfo>(data);
                    WaitingRelease(_tmpResultInfo);
                    break;
                #endregion

                #region Request

                case FunctionCode.RequestGetDataAll:
                    _requestInfo = SerializationHelper.Deserialize<RequestInfo>(data);
                    if (_requestInfo != null)
                    {
                        _resultInfo = GetDataAll(_requestInfo.Top);
                        Send(FunctionCode.ResponseGetDataAll, SerializationHelper.Serialize(_resultInfo), FrameID++);
                    }
                    else
                    {
                        Send(FunctionCode.RequestFailed, SerializationHelper.Serialize(_resultInfo), FrameID++);
                    }
                    break;

                case FunctionCode.RequestTryGetID:
                    _requestInfo = SerializationHelper.Deserialize<RequestInfo>(data);
                    if (_requestInfo != null)
                    {
                        //_resultInfo = TryGetLinQAsync(_requestInfo).Result;
                        Send(FunctionCode.ResponseTryGetID, SerializationHelper.Serialize(_resultInfo), FrameID++);
                    }
                    else
                    {
                        Send(FunctionCode.RequestFailed, SerializationHelper.Serialize(_resultInfo), FrameID++);
                    }
                    break;
                case FunctionCode.RequestGetDataIsActivity:
                    _requestInfo = SerializationHelper.Deserialize<RequestInfo>(data);
                    if (_requestInfo != null)
                    {
                        _resultInfo = GetDataIsActivityAsync(_requestInfo.Top).Result;
                        Send(FunctionCode.ResponseGetDataIsActivity, SerializationHelper.Serialize(_resultInfo), FrameID++);
                    }
                    else
                    {
                        Send(FunctionCode.RequestFailed, SerializationHelper.Serialize(_resultInfo), FrameID++);
                    }
                    break;
                case FunctionCode.RequestGetDataIsActivityByGroup:
                    _requestInfo = SerializationHelper.Deserialize<RequestInfo>(data);
                    if (_requestInfo != null)
                    {
                        _resultInfo = GetDataIsActivityByGroupAsync(_requestInfo.ID, _requestInfo.Top).Result;
                        Send(FunctionCode.ResponseGetDataIsActivityByGroup, SerializationHelper.Serialize(_resultInfo), FrameID++);
                    }
                    else
                    {
                        Send(FunctionCode.RequestFailed, SerializationHelper.Serialize(_resultInfo), FrameID++);
                    }
                    break;
                case FunctionCode.RequestGetDataNotIsActivity:
                    _requestInfo = SerializationHelper.Deserialize<RequestInfo>(data);
                    if (_requestInfo != null)
                    {
                        _resultInfo = GetDataNotIsActivityAsync(_requestInfo.Top).Result;
                        Send(FunctionCode.ResponseGetDataNotIsActivity, SerializationHelper.Serialize(_resultInfo), FrameID++);

                    }
                    else
                    {
                        Send(FunctionCode.RequestFailed, SerializationHelper.Serialize(_resultInfo), FrameID++);
                    }
                    break;
                #endregion

                default:
                    Log.Warn("Unsupport function code");
                    break;
            }
        }
        public virtual void Reset()
        {
            // No Code
        }
        public virtual void CustomerDataSync(IList<TInfo> list)
        {
            Data = new InfoCollection<TInfo>(list);
            lock (DataSyncLocker)
            {
                IsCustomerDataSync = true;
                ReadQueue();
            }
            OnListSync();
        }

        #endregion

        //**--------------------------------------------------------------------------------

        #region Protected
        protected virtual void ReadQueue()
        {
            while (IsConnected && Queue.Count > 0)
            {
                Action action = Queue.Dequeue();
                action();
            }
        }
        protected virtual void WaitingRequestsAdd(Guid key, DataRequestInfo data)
        {
            try
            {
                lock (DictionaryLocker)
                {
                    if (WaitingRequests.ContainsKey(key)) return;
                    WaitingRequests.Add(key, data);
                }
            }
            catch 
            {

            }

        }
        protected virtual void WaitingRequestsRemove(Guid key)
        {
            try
            {
                lock (DictionaryLocker)
                {
                    if (!WaitingRequests.Remove(key)) return;
                }
            }
            catch 
            {

            }
        }
        protected virtual void WaitingRelease(ResultInfo resultInfo)
        {
            try
            {
                if (resultInfo == null) return;
                DataRequestInfo dataRequestInfo = null;
                WaitingRequests.TryGetValue(resultInfo.RequestKeyID, out dataRequestInfo);
                if (dataRequestInfo != null)
                {
                    dataRequestInfo.Result = resultInfo;
                    dataRequestInfo.SemaWaiting.Set();
                    //WaitingRequestsRemove(resultInfo.RequestKeyID);
                }
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
            }
        }
        protected virtual void WaitingRelease_ResponseEdit(ResultInfo resultInfo)
        {
            try
            {
                if (resultInfo == null) return;
                DataRequestInfo dataRequestInfo = null;
                WaitingRequests.TryGetValue(resultInfo.RequestKeyID, out dataRequestInfo);
                if (dataRequestInfo != null)
                {
                   //var _tmpResultInfo = SerializationHelper.Deserialize<ResultInfo>(resultInfo);
                    dataRequestInfo.Result = resultInfo;
                    dataRequestInfo.SemaWaiting.Set();
                    //WaitingRequestsRemove(resultInfo.RequestKeyID);
                }
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
            }
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Request Notify 
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestListNotifyAdd(IList<TInfo> list)
        {
            try
            {
                if (!IsConnected)
                {
                    // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo() { Data = list };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestListNotifyAdded, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> RequestListNotifyAddAsyn(IList<TInfo> list)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo() { Data = list };
                    _requestInfo.Owner = SessionDatas.GetOwner();
                    _requestInfo.UserName = SessionDatas.GetLoginUser();
                    _requestInfo.UserID = "" + SessionDatas.GetUserID();
                    _requestInfo.BranchID = SessionDatas.GetBranchID();
                    _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestListNotifyAdded, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
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
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestNotifyAdd(TInfo info)
        {
            try
            {
                if (!IsConnected)
                {
                    // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo() { Data = info };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestNotifyAdded, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> RequestNotifyAddAsyn(TInfo info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo() { Data = info };
                    _requestInfo.Owner = SessionDatas.GetOwner();
                    _requestInfo.UserName = SessionDatas.GetLoginUser();
                    _requestInfo.UserID = "" + SessionDatas.GetUserID();
                    _requestInfo.BranchID = SessionDatas.GetBranchID();
                    _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestNotifyAdded, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
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
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestListNotifyUpdate(IList<TInfo> list)
        {
            try
            {
                if (!IsConnected)
                {
                    // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo() { Data = list };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestListNotifyUpdated, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> RequestListNotifyUpdateAsyn(IList<TInfo> list)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo() { Data = list };
                    _requestInfo.Owner = SessionDatas.GetOwner();
                    _requestInfo.UserName = SessionDatas.GetLoginUser();
                    _requestInfo.UserID = "" + SessionDatas.GetUserID();
                    _requestInfo.BranchID = SessionDatas.GetBranchID();
                    _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestListNotifyUpdated, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
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
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestNotifyUpdate(TInfo info)
        {
            try
            {
                if (!IsConnected)
                {
                    // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo() { Data = info };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestNotifyUpdated, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> RequestNotifyUpdateAsyn(TInfo info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo() { Data = info };
                    _requestInfo.Owner = SessionDatas.GetOwner();
                    _requestInfo.UserName = SessionDatas.GetLoginUser();
                    _requestInfo.UserID = "" + SessionDatas.GetUserID();
                    _requestInfo.BranchID = SessionDatas.GetBranchID();
                    _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestNotifyUpdated, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
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
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestListNotifyRemoved(IList<object> list)
        {
            try
            {
                if (!IsConnected)
                {
                    // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo() { Data = list };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestListNotifyRemoved, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> RequestListNotifyRemovedAsyn(IList<TInfo> list)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo() { Data = list };
                    _requestInfo.Owner = SessionDatas.GetOwner();
                    _requestInfo.UserName = SessionDatas.GetLoginUser();
                    _requestInfo.UserID = "" + SessionDatas.GetUserID();
                    _requestInfo.BranchID = SessionDatas.GetBranchID();
                    _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestListNotifyRemoved, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
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
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestNotifyRemoved(object id)
        {
            try
            {
                if (!IsConnected)
                {
                    // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo() { Data = id };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestNotifyRemoved, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> RequestNotifyRemovedAsyn(object id)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo() { Data = id };
                    _requestInfo.Owner = SessionDatas.GetOwner();
                    _requestInfo.UserName = SessionDatas.GetLoginUser();
                    _requestInfo.UserID = "" + SessionDatas.GetUserID();
                    _requestInfo.BranchID = SessionDatas.GetBranchID();
                    _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestNotifyRemoved, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
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
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }
        //**--------------------------------------------------------------------------------
        public virtual ResultInfo RequestNotifyCustomized(ResultInfo info)
        {
            try
            {
                if (!IsConnected)
                {
                    // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo() { Data = info };
                _requestInfo.Owner = SessionDatas.GetOwner();
                _requestInfo.UserName = SessionDatas.GetLoginUser();
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.BranchID = SessionDatas.GetBranchID();
                _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestNotifyCustomized, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> RequestNotifyCustomizedAsyn(ResultInfo info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo() { Data = info };
                    _requestInfo.Owner = SessionDatas.GetOwner();
                    _requestInfo.UserName = SessionDatas.GetLoginUser();
                    _requestInfo.UserID = "" + SessionDatas.GetUserID();
                    _requestInfo.BranchID = SessionDatas.GetBranchID();
                    _requestInfo.BranchCode = SessionDatas.GetBranchCode();
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestNotifyCustomized, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
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
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }
        //**--------------------------------------------------------------------------------
        #endregion

        //**--------------------------------------------------------------------------------

        #region Get Data
        //**------------------------------------------------------------------------------------

        #region Get Data All (Type: 0)
        [Description("Get all record in table, return list data info.")]
        public virtual ResultInfo GetDataAll(int top)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                //if (IsDataSync && Data != null)
                //{
                //    _resultInfo = new ResultInfo();
                //    _resultInfo.Data = Data.Cast<TInfo>().Take(top).ToList();
                //    return _resultInfo;
                //}
                var _requestInfo = new RequestInfo()
                {
                    Top = top,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetDataAll, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
            //finally
            //{
            //    if (_tmpDataCurent != null)
            //        if (!(_tmpDataCurent.Data is TInfo))
            //            _tmpDataCurent.Data = JsonHelper.JsonToInfo<TInfo>("" + _tmpDataCurent.Data);
            //}

        }
        [Description("Get all record in table, return list data info.")]
        public virtual Task<ResultInfo> GetDataAllAsync(int top)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        Top = top,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestGetDataAll, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
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
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }
        [Description("Get all record in table, return list data info.")]
        public virtual ResultInfo GetData(object id, int top)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    ID = id,
                    Top = top,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetDataAll, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        [Description("Get all record in table, return list data info.")]
        public virtual Task<ResultInfo> GetDataAsync(object id, int top)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        ID = id,
                        Top = top,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestGetDataAll, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
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
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        //**------------------------------------------------------------------------------------
        [Description("Get all record in table, return list data View Entity.")]
        public virtual ResultInfo GetDataBBAll<TViewEntity>(SynchronizationContext currentContext, int top) where TViewEntity : class, IViewEntity<TInfo>, new()
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Top = top,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetDataAll, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return ListToBBAll<TViewEntity>(dataSend.Result, dataSend.Result.Data as IList<TInfo>, currentContext);
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        [Description("Get all record in table, return list data View Entity.")]
        public virtual Task<ResultInfo> GetDataBBAllAsync<TViewEntity>(SynchronizationContext currentContext, int top) where TViewEntity : class, IViewEntity<TInfo>, new()
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    //if (IsDataSync && Data != null)
                    //{
                    //    _resultInfo = new ResultInfo();
                    //    return ListToBBAll<TViewEntity>(Data.Cast<TInfo>().Take(top).ToList(), currentContext); ;
                    //}

                    var _requestInfo = new RequestInfo()
                    {
                        Top = top,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestGetDataAll, SerializationHelper.Serialize(_requestInfo), FrameID);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return ListToBBAll<TViewEntity>(dataSend.Result, dataSend.Result.Data as IList<TInfo>, currentContext);
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Try Get (Type: 1)
        [Description("Get record by ID, if ID is string then edit in sp, id by Group(1:ID,2:Owner,3:UserName,4:FromDate,5:ToDate,6:ID,7:TSQL).")]
        public virtual ResultInfo TryGet(object id)
        {
            try
            {
                if (!IsConnected)
                {
                    // Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    ID = id,
                    Top = SystemSetting.TopDafault,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestTryGetID, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (!(dataSend.Result.Data is TInfo))
                            dataSend.Result.Data = SerializationHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> TryGetAsync(object id)
        {
            return Task.Run<ResultInfo>(() =>
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }

                if (IsDataSync && Data != null)
                {
                    var _tmpTryGetResultInfo = new ResultInfo();
                    _tmpTryGetResultInfo.Data = Data.Cast<TInfo>().FirstOrDefault(t => ("" + t.ValueID) == ("" + id));
                    _tmpTryGetResultInfo.Status = true;
                    return _tmpTryGetResultInfo;
                }
                var _requestInfo = new RequestInfo()
                {
                    ID = id,
                    Top = SystemSetting.TopDafault,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestTryGetID, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (!(dataSend.Result.Data is TInfo))
                            dataSend.Result.Data = SerializationHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;

            });
        }
        public virtual ResultInfo TryGet(Func<TInfo, bool> predicate)
        {

            if (!IsConnected)
            {
                //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
            }
            if (IsDataSync && Data != null)
            {
                var _resultInfo = new ResultInfo();
                _resultInfo.Data = Data.Cast<TInfo>().FirstOrDefault(predicate);
                return _resultInfo;
            }
            var _requestInfo = new RequestInfo()
            {
                Top = SystemSetting.TopDafault,
                Owner = SessionDatas.GetOwner(),
                UserName = SessionDatas.GetLoginUser(),
                UserID = "" + SessionDatas.GetUserID(),
                BranchID = SessionDatas.GetBranchID(),
                BranchCode = SessionDatas.GetBranchCode(),
                TSQL = "",
                Data = predicate,
                ModelName = "",
            };
            var dataSend = new DataRequestInfo(_requestInfo);
            try
            {
                WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                Send(FunctionCode.RequestGetDataAll, SerializationHelper.Serialize(_requestInfo), FrameID++);
                if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                {
                    IList<TInfo> infos = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                    dataSend.Result.Data = infos.FirstOrDefault(predicate);
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
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
            }
            finally
            {
                WaitingRequestsRemove(dataSend.Request.KeyID);
            }
            return dataSend.Result;
        }
        public virtual Task<ResultInfo> TryGetAsync(Func<TInfo, bool> predicate)
        {
            return Task.Run<ResultInfo>(() =>
            {
                if (!IsConnected)
                        {
                            //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                            return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                        }
                        if (IsDataSync && Data != null)
                        {
                            var _resultInfo = new ResultInfo();
                            _resultInfo.Data = Data.Cast<TInfo>().FirstOrDefault(predicate);
                            return _resultInfo;
                        }
                        var _requestInfo = new RequestInfo()
                        {
                            Data = predicate,
                            Top = SystemSetting.TopDafault,
                            Owner = SessionDatas.GetOwner(),
                            UserName = SessionDatas.GetLoginUser(),
                            UserID = ""+SessionDatas.GetUserID(),
                            BranchID = SessionDatas.GetBranchID(),
                            BranchCode = SessionDatas.GetBranchCode(),
                            TSQL = "",
                            ModelName = "",
                        };
                        var dataSend = new DataRequestInfo(_requestInfo);
                        try
                        {
                            WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                            Send(FunctionCode.RequestTryGetID, SerializationHelper.Serialize(_requestInfo), FrameID++);
                            if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
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
                            SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                        }
                        finally
                        {
                            WaitingRequestsRemove(dataSend.Request.KeyID);
                        }
                        return dataSend.Result;
            });
        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Get Data IsActivity
        public virtual ResultInfo GetDataIsActivity(int top)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Top = top,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetDataIsActivity, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> GetDataIsActivityAsync(int top)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        Top = top,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestGetDataIsActivity, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        public virtual Task<ResultInfo> GetDataIsActivityAsync(int top, int branchID)
        {
            return Task.Run<ResultInfo>(() =>
           {
               try
               {
                   if (!IsConnected)
                   {
                       //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                       return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                   }

                   var _requestInfo = new RequestInfo()
                   {
                       Top = top,
                       BranchID = branchID,
                       Owner = SessionDatas.GetOwner(),
                       UserName = SessionDatas.GetLoginUser(),
                       UserID = "" + SessionDatas.GetUserID(),
                       BranchCode = SessionDatas.GetBranchCode(),
                       TSQL = "",
                       ModelName = "" + branchID,
                   };
                   var dataSend = new DataRequestInfo(_requestInfo);
                   try
                   {
                       WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                       Send(FunctionCode.RequestGetDataIsActivityByBranch, SerializationHelper.Serialize(_requestInfo), FrameID++);
                       if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                       {
                           if (dataSend.Result.Status)
                           {
                               dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                               return dataSend.Result;
                           }
                       }
                       else
                       {
                           dataSend.Result.Status = false;
                           dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                       }
                   }
                   catch (Exception ex)
                   {
                       SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                   }
                   finally
                   {
                       WaitingRequestsRemove(dataSend.Request.KeyID);
                   }
                   return dataSend.Result;

               }
               catch (Exception ex)
               {
                   SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                   return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
               }

           });
        }
        
        //**------------------------------------------------------------------------------------
        public virtual ResultInfo GetDataNotIsActivity(int top)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Top = top,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetDataNotIsActivity, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> GetDataNotIsActivityAsync(int top)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        Top = top,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestGetDataNotIsActivity, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        //**------------------------------------------------------------------------------------
        public virtual ResultInfo GetDataBBIsActivity<TViewEntity>(SynchronizationContext currentContext, int top) where TViewEntity : class, IViewEntity<TInfo>, new()
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Top = top,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetDataIsActivity, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return ListToBBAll<TViewEntity>(dataSend.Result, dataSend.Result.Data as IList<TInfo>, currentContext);
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> GetDataBBIsActivityAsync<TViewEntity>(SynchronizationContext currentContext, int top) where TViewEntity : class, IViewEntity<TInfo>, new()
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    var _requestInfo = new RequestInfo()
                    {
                        Top = top,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestGetDataIsActivity, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return ListToBBAll<TViewEntity>(dataSend.Result, dataSend.Result.Data as IList<TInfo>, currentContext);
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Get Data All
        public virtual ResultInfo GetDataBBAll<TViewEntity>(SynchronizationContext currentContext, int top, Func<TInfo, bool> predicateh) where TViewEntity : class, IViewEntity<TInfo>, new()
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Top = top,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                    Data = predicateh
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetDataAll, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return ListToBBAll<TViewEntity>(dataSend.Result.Data as IList<TInfo>, currentContext, predicateh);
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> GetDataBBAllAsync<TViewEntity>(SynchronizationContext currentContext, int top, Func<TInfo, bool> predicateh) where TViewEntity : class, IViewEntity<TInfo>, new()
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    var _requestInfo = new RequestInfo()
                    {
                        Top = top,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                        Data = predicateh
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestGetDataAll, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return ListToBBAll<TViewEntity>(dataSend.Result.Data as IList<TInfo>, currentContext, predicateh);
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Get Data By Group
        public virtual ResultInfo GetDataIsActivityByGroup(object id, int top)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    ID = id,
                    Top = top,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                    GroupCode = id,
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetDataIsActivityByGroup, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> GetDataIsActivityByGroupAsync(object id, int top)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        Top = top,
                        ID = id,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                        GroupCode = id
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestGetDataIsActivityByGroup, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        public virtual ResultInfo GetDataIsActivityByGroupCode(object id, int top)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    ID = id,
                    Top = top,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "" + id,
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetDataIsActivityByGroupCode, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> GetDataIsActivityByGroupCodeAsync(object id, int top)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        Top = top,
                        ID = id,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                        GroupCode = id
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestGetDataIsActivityByGroupCode, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        //**------------------------------------------------------------------------------------
        public virtual ResultInfo GetDataBBGroupBy<TViewEntity>(SynchronizationContext currentContext, object group, int top) where TViewEntity : class, IViewEntity<TInfo>, new()
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Top = top,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "" + group,
                    GroupCode = group
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetDataIsActivityByGroup, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return ListToBBAll<TViewEntity>(dataSend.Result, dataSend.Result.Data as IList<TInfo>, currentContext);
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> GetDataBBGroupByAsync<TViewEntity>(SynchronizationContext currentContext, object group, int top) where TViewEntity : class, IViewEntity<TInfo>, new()
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }

                    var _requestInfo = new RequestInfo()
                    {
                        Top = top,
                        UserName = SessionDatas.GetLoginUser(),
                        Owner = SessionDatas.GetOwner(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "" + group,
                        ID = group,
                        GroupCode = group
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestGetDataIsActivityByGroup, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return ListToBBAll<TViewEntity>(dataSend.Result, dataSend.Result.Data as IList<TInfo>, currentContext);
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }

                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Get By Where
        [Description("Get data by string where(0:SP Name,1:UserID,2:BranchID,3:Owner,4:UserName,5:FromDate,6:ToDate,7:ModelName,8:TSQL)")]
        public virtual Task<ResultInfo> GetByWhereAsync(string whereSQL)
        {
            return Task.Run(() =>
             {
                 try
                 {
                     if (!IsConnected)
                     {
                         //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                         return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                     }
                     var _requestInfo = new RequestInfo()
                     {
                         Owner = SessionDatas.GetOwner(),
                         UserName = SessionDatas.GetLoginUser(),
                         UserID = "" + SessionDatas.GetUserID(),
                         BranchID = SessionDatas.GetBranchID(),
                         BranchCode = SessionDatas.GetBranchCode(),
                         TSQL = whereSQL,
                         ModelName = "",
                         FuncWhere = whereSQL,
                     };
                     var dataSend = new DataRequestInfo(_requestInfo);
                     try
                     {
                         WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                         Send(FunctionCode.RequestGetByWhere, SerializationHelper.Serialize(_requestInfo), FrameID++);
                         if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                         {
                             if (dataSend.Result.Status)
                             {
                                 dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                 return dataSend.Result;
                             }
                         }
                         else
                         {
                             dataSend.Result.Status = false;
                             dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                         }
                     }
                     catch (Exception ex)
                     {
                         SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                     }
                     finally
                     {
                         WaitingRequestsRemove(dataSend.Request.KeyID);
                     }
                     return dataSend.Result;
                 }
                 catch (Exception ex)
                 {
                     SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                     return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                 }
             });
        }
        [Description("Get data by string where(0:SP Name,1:UserID,2:BranchID,3:Owner,4:UserName,5:FromDate,6:ToDate,7:ModelName,8:TSQL)")]
        public virtual ResultInfo GetByWhere(string sqlWhere)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = sqlWhere,
                    ModelName = "",
                    FuncWhere = sqlWhere,
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetByWhere, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public Task<ResultInfo> RequestGetByWhereAsync(RequestInfo request)
        {
            return Task.Run<ResultInfo>(() =>
            {
                return RequestGetByWhere(request);
            });
        }
        public ResultInfo RequestGetByWhere(RequestInfo request)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var dataSend = new DataRequestInfo(request);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    request.Owner = SessionDatas.GetOwner();
                    request.UserName = SessionDatas.GetLoginUser();
                    request.UserID = "" + SessionDatas.GetUserID();
                    request.BranchID = SessionDatas.GetBranchID();
                    request.BranchCode = SessionDatas.GetBranchCode();
                    Send(FunctionCode.RequestGetByWhere, SerializationHelper.Serialize(request), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }

        #endregion

        //**------------------------------------------------------------------------------------

        #region Request Validate
        [Description("Call Func check data from server.")]
        public virtual Task<ResultInfo> RequestValidateAsync(dynamic data, EValidateType type)
        {
            return Task.Factory.StartNew<ResultInfo>(() =>
            {
                return RequestValidate(data, type);
            });
        }
        [Description("Call Func check data from server.")]
        public virtual ResultInfo RequestValidate(dynamic data, EValidateType type)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                    FuncWhere = "",
                    Data = data,
                    Option = (int)type,
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestValidateInfo, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        //if (dataSend.Result.Status)
                        //{
                        //    dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                        //    return dataSend.Result;
                        //}
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Execute Query S2Info
        public virtual ResultInfo ExecuteQueryS2Info(string tSql)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    TSQL = tSql,
                    Top = SystemSetting.TopDafault,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestExecTSQL, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> ExecuteQueryS2InfoAsync(string tSql)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        TSQL = tSql,
                        Top = SystemSetting.TopDafault,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        ModelName = "",
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestExecTSQL, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }


            });

        }
        //**------------------------------------------------------------------------------------
        public virtual ResultInfo ExecuteQueryS2Info<T>(string tSql) where T : class, IInfo<T>, new()
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    TSQL = tSql,
                    TypeInfo = typeof(T),
                    Top = SystemSetting.TopDafault,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestExecTSQLToEn, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<T>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> ExecuteQueryS2InfoAsync<T>(string tSql) where T : class, IInfo<T>, new()
        {

            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        TSQL = tSql,
                        Top = SystemSetting.TopDafault,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        ModelName = "",
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(_requestInfo.KeyID, dataSend);
                        Send(FunctionCode.RequestExecTSQLTable, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                //if(_tmpResultInfo.Data is string)

                                dataSend.Result.Data = SerializationHelper.JsonToDataTable2Info<T>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }

                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });


        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Execute Query S2Data Table
        public virtual ResultInfo ExecuteQueryS2DataTable(string tSql)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    TSQL = tSql,
                    Top = SystemSetting.TopDafault,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    ModelName = "",
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestExecTSQLTable, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToDataTable("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        public virtual Task<ResultInfo> ExecuteQueryS2DataTableAsync(string tSql)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        TSQL = tSql,
                        Top = SystemSetting.TopDafault,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        ModelName = "",
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestExecTSQLTable, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToDataTable("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }

                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Execute Search From To
        public virtual Task<ResultInfo> ExecuteSearchFromToAsync(SearchOrderInfo info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        Top = SystemSetting.TopDafault,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                    };
                    _requestInfo.SearchOrderInfo = info;
                    _requestInfo.UserID = "" + SessionDatas.GetUserID();
                    _requestInfo.IsBranchID = info.IsBranch;
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestSearchInfo, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        public virtual ResultInfo ExecuteSearchFromTo(SearchOrderInfo info)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Top = SystemSetting.TopDafault,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                };
                _requestInfo.SearchOrderInfo = info;
                _requestInfo.UserID = "" + SessionDatas.GetUserID();
                _requestInfo.IsBranchID = info.IsBranch;
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestExecTSQL, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }

        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Execute Command
        public virtual Task<ResultInfo> ExecuteCommandAsync(string tsql)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        TSQL = tsql,
                        Top = SystemSetting.TopDafault,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        ModelName = "",
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestExecuteCommand, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                //_tmpResultInfo.Data = SerializationHelper.JsonToDataTable("" + _tmpResultInfo.Data);
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        public virtual ResultInfo ExecuteCommand(string tsql)
        {
            if (!IsConnected)
            {
                //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
            }
            var _requestInfo = new RequestInfo()
            {
                TSQL = tsql,
                Top = SystemSetting.TopDafault,
                Owner = SessionDatas.GetOwner(),
                UserName = SessionDatas.GetLoginUser(),
                UserID = "" + SessionDatas.GetUserID(),
                BranchID = SessionDatas.GetBranchID(),
                BranchCode = SessionDatas.GetBranchCode(),
                ModelName = "",
            };
            var dataSend = new DataRequestInfo(_requestInfo);
            try
            {
                WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                Send(FunctionCode.RequestExecuteCommand, SerializationHelper.Serialize(_requestInfo), FrameID++);
                if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                {
                    if (dataSend.Result.Status)
                    {
                        //_tmpResultInfo.Data = SerializationHelper.JsonToDataTable("" + _tmpResultInfo.Data);
                    }
                }
                else
                {
                    dataSend.Result.Status = false;
                    dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                }
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

            }
            finally
            {
                WaitingRequestsRemove(dataSend.Request.KeyID);
            }
            return dataSend.Result;
        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Request Get Data By Option
        public virtual ResultInfo RequestGetDataByOption(RequestInfo request)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var dataSend = new DataRequestInfo(request);

                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    request.Owner = SessionDatas.GetOwner();
                    request.UserName = SessionDatas.GetLoginUser();
                    request.UserID = "" + SessionDatas.GetUserID();
                    request.BranchID = SessionDatas.GetBranchID();
                    Send(FunctionCode.RequestGetDataByOption, SerializationHelper.Serialize(request), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public Task<ResultInfo> RequestGetDataByOptionAsync(RequestInfo request)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
            {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var dataSend = new DataRequestInfo(request);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        request.Owner = SessionDatas.GetOwner();
                        request.UserName = SessionDatas.GetLoginUser();
                        request.UserID = "" + SessionDatas.GetUserID();
                        request.BranchID = SessionDatas.GetBranchID();
                        request.BranchCode = SessionDatas.GetBranchCode();
                        Send(FunctionCode.RequestGetDataByOption, SerializationHelper.Serialize(request), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
            });
        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Request Synchronized
        public virtual Task<ResultInfo> RequestSynchronized(int clientID, RequestInfo info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                if (info == null) return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LRequestInfoNull };
                info.Owner = SessionDatas.GetOwner();
                info.UserName = SessionDatas.GetLoginUser();
                info.UserID =""+ SessionDatas.GetUserID();
                info.BranchID = SessionDatas.GetBranchID();
                info.BranchCode = SessionDatas.GetBranchCode();
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var dataSend = new DataRequestInfo(clientID, info);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestSynchronized, SerializationHelper.Serialize(dataSend.Request), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            //_tmpResultInfo.Data = SerializationHelper.JsonToInfo("" + _tmpResultInfo.Data);
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            });
        }
        #endregion

        //**------------------------------------------------------------------------------------

        #region Request Update Cache Data
        public virtual ResultInfo RequestUpdateCacheData(int clientID, RequestInfo info)
        {
            if (info == null) return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LRequestInfoNull };
            info.Owner = SessionDatas.GetOwner();
            info.UserName = SessionDatas.GetLoginUser();
            info.UserID =""+ SessionDatas.GetUserID();
            info.BranchID = SessionDatas.GetBranchID();
            info.BranchCode = SessionDatas.GetBranchCode();
            if (!IsConnected)
            {
                //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
            }
            var dataSend = new DataRequestInfo(clientID, info);
            try
            {
                WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                Send(FunctionCode.RequestUpdateCache, SerializationHelper.Serialize(dataSend.Request), FrameID++);
                if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                {
                    if (dataSend.Result.Status)
                    {
                        //_tmpResultInfo.Data = SerializationHelper.JsonToInfo("" + _tmpResultInfo.Data);
                    }
                }
                else
                {
                    dataSend.Result.Status = false;
                    dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                }
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

            }
            finally
            {
                WaitingRequestsRemove(dataSend.Request.KeyID);
            }
            return dataSend.Result;
        }
        public virtual Task<ResultInfo> RequestUpdateCacheDataAsyn(int clientID, RequestInfo info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                if (info == null) return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LRequestInfoNull };
                info.Owner = SessionDatas.GetOwner();
                info.UserName = SessionDatas.GetLoginUser();
                info.UserID =""+ SessionDatas.GetUserID();
                info.BranchID = SessionDatas.GetBranchID();
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var dataSend = new DataRequestInfo(clientID, info);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestUpdateCache, SerializationHelper.Serialize(dataSend.Request), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            //_tmpResultInfo.Data = SerializationHelper.JsonToInfo("" + _tmpResultInfo.Data);
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            });
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Exc Paginate
        public virtual Task<ResultInfo> ExcPaginateAsync(PaginateInfo info)
        {
            return Task.Run<ResultInfo>(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        Top = SystemSetting.TopDafault,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                    };
                    _requestInfo.Data = info;

                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestPaginateInfo, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }

            });
        }
        public virtual ResultInfo ExcPaginate(PaginateInfo info)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Top = SystemSetting.TopDafault,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                };
                _requestInfo.Data = info;
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestPaginateInfo, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;

            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region TryGet By String Condition
        public virtual ResultInfo TryGetBySCondition(string condition)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Top = SystemSetting.TopDafault,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                    FuncWhere = condition,
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestTryGetBySCondition, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> TryGetBySConditionAsync(string condition)
        {
            return Task.Run<ResultInfo>(()=> {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        Top = SystemSetting.TopDafault,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                        FuncWhere = condition,
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestTryGetBySCondition, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }

        #endregion

        //**--------------------------------------------------------------------------------

        #region Check Exists By Sting Condition
        
        public virtual ResultInfo CheckExistsBySCondition(string condition)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Top = SystemSetting.TopDafault,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                    FuncWhere = condition,
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestCheckExistsBySCondition, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        //if (dataSend.Result.Status)
                        //{
                        //    dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                        //    return dataSend.Result;
                        //}
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
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual Task<ResultInfo> CheckExistsBySConditionAsync(string condition)
        {
            return Task.Run<ResultInfo>(() => {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        Top = SystemSetting.TopDafault,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                        FuncWhere = condition,
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestCheckExistsBySCondition, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            //if (dataSend.Result.Status)
                            //{
                            //    dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            //    return dataSend.Result;
                            //}
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
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }
        #endregion

        //**--------------------------------------------------------------------------------

        #region Get Data By Sting Condition

        public virtual ResultInfo GetDataBySCondition(string condition)
        {
            try
            {
                if (!IsConnected)
                {
                    //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                    return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                }
                var _requestInfo = new RequestInfo()
                {
                    Top = SystemSetting.TopDafault,
                    Owner = SessionDatas.GetOwner(),
                    UserName = SessionDatas.GetLoginUser(),
                    UserID = "" + SessionDatas.GetUserID(),
                    BranchID = SessionDatas.GetBranchID(),
                    BranchCode = SessionDatas.GetBranchCode(),
                    TSQL = "",
                    ModelName = "",
                    FuncWhere = condition,
                };
                var dataSend = new DataRequestInfo(_requestInfo);
                try
                {
                    WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                    Send(FunctionCode.RequestGetDataBySCondition, SerializationHelper.Serialize(_requestInfo), FrameID++);
                    if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                    {
                        if (dataSend.Result.Status)
                        {
                            dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                            return dataSend.Result;
                        }
                    }
                    else
                    {
                        dataSend.Result.Status = false;
                        dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                    }
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                }
                finally
                {
                    WaitingRequestsRemove(dataSend.Request.KeyID);
                }
                return dataSend.Result;
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }

        public virtual Task<ResultInfo> GetDataBySConditionAsync(string condition)
        {
            return Task.Run<ResultInfo>(()=> {
                try
                {
                    if (!IsConnected)
                    {
                        //Queue.Enqueue(()=>{ Send(FunctionCode.RequestAdd, SerializationHelper.Serialize(info), FrameID); });
                        return new ResultInfo() { Status = false, ErrorMessage = FWLanguages.LConnectFail };
                    }
                    var _requestInfo = new RequestInfo()
                    {
                        Top = SystemSetting.TopDafault,
                        Owner = SessionDatas.GetOwner(),
                        UserName = SessionDatas.GetLoginUser(),
                        UserID = "" + SessionDatas.GetUserID(),
                        BranchID = SessionDatas.GetBranchID(),
                        BranchCode = SessionDatas.GetBranchCode(),
                        TSQL = "",
                        ModelName = "",
                        FuncWhere = condition,
                    };
                    var dataSend = new DataRequestInfo(_requestInfo);
                    try
                    {
                        WaitingRequestsAdd(dataSend.GetKeyID(), dataSend);
                        Send(FunctionCode.RequestGetDataBySCondition, SerializationHelper.Serialize(_requestInfo), FrameID++);
                        if (dataSend.SemaWaiting.WaitOne(ConnectDatas.TimeOut))
                        {
                            if (dataSend.Result.Status)
                            {
                                dataSend.Result.Data = SerializationHelper.JsonToListInfo<TInfo>("" + dataSend.Result.Data);
                                return dataSend.Result;
                            }
                        }
                        else
                        {
                            dataSend.Result.Status = false;
                            dataSend.Result.ErrorMessage = FWLanguages.LResponseTimeout;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                    }
                    finally
                    {
                        WaitingRequestsRemove(dataSend.Request.KeyID);
                    }
                    return dataSend.Result;
                }
                catch (Exception ex)
                {
                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);

                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                }
            });
        }

        #endregion

        #region ListToBBAll
        public virtual ResultInfo ListToBBAll<TViewEntity>(ResultInfo resultInfo, IList<TInfo> infos, SynchronizationContext currentContext) where TViewEntity : class, IViewEntity<TInfo>, new()
        {
            //ResultInfo resultInfo = new ResultInfo();
            BindingList<TViewEntity> ViewEntityCollection = new ViewEntityCollection<TInfo, TViewEntity>(currentContext);
            try
            {
                if (infos != null)
                {
                    TViewEntity Entity;
                    foreach (TInfo info in infos)
                    {
                        Entity = new TViewEntity();
                        Entity.Set(info);
                        ViewEntityCollection.Add(Entity);
                    }
                }

                resultInfo.Data = ViewEntityCollection;
                resultInfo.Status = true;
                return resultInfo;

            }
            catch (Exception ex)
            {
                // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                resultInfo.Status = false;
                resultInfo.ErrorMessage = ex.Message;
                return resultInfo;
            }
        }
        public virtual ResultInfo ListToBBAll<TViewEntity>(IList<TInfo> infos, SynchronizationContext currentContext, Func<TInfo, bool> predicateh) where TViewEntity : class, IViewEntity<TInfo>, new()
        {
            ResultInfo resultInfo = new ResultInfo();
            BindingList<TViewEntity> ViewEntityCollection = new ViewEntityCollection<TInfo, TViewEntity>(currentContext);
            try
            {
                if (infos != null)
                {
                    IList<TInfo> list = infos.Where(predicateh).ToList();
                    TViewEntity Entity;
                    foreach (TInfo info in list)
                    {
                        Entity = new TViewEntity();
                        Entity.Set(info);
                        ViewEntityCollection.Add(Entity);
                    }
                }

                resultInfo.Data = ViewEntityCollection;
                resultInfo.Status = true;
                return resultInfo;

            }
            catch (Exception ex)
            {
                // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                resultInfo.Status = false;
                resultInfo.ErrorMessage = ex.Message;
                return resultInfo;
            }
        }

        #endregion


        #region Public Method
        public virtual void Copy<T>(TInfo info, T des) where T : class, new()
        {
            try
            {
                //Read Attribute Names and Types
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                var objFieldNames = des.GetType().GetProperties(flags).Cast<PropertyInfo>().
                    Select(item => new
                    {
                        Name = item.Name,
                        Type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType,
                        value = item.GetValue(des, null)
                    }).ToList();

                foreach (var item in objFieldNames)
                {
                    PropertyInfo propertyInfos = info.GetType().GetProperty(item.Name);
                    PropertyInfo propertydes = des.GetType().GetProperty(item.Name);
                    if (propertyInfos != null && propertydes != null)
                    {
                        propertydes.SetValue(des, propertyInfos.GetValue(info, null), null);
                    }
                }
            }
            catch 
            {

            }

        }
        #endregion

        #endregion

        //**--------------------------------------------------------------------------------
    }
}
