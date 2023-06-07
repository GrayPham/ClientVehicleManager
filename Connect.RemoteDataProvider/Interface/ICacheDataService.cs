using Connect.Common;
using Connect.Common.Contract;
using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Connect.RemoteDataProvider.Interface
{
    public interface ICacheDataService<TInfo> : IFuncClientDataService<TInfo>, IDataClient<TInfo> where TInfo : IInfo<TInfo>, new()
    {
        /// <summary>
        /// True if remote data service, false if local
        /// </summary>
        Boolean IsRemote { get; }
        Boolean IsConnected { get; }
        bool IsCustomerDataSync { get; }
        void CustomerDataSync(IList<TInfo> list);
        /// <summary>
        /// A Copy of Database
        /// </summary>
        IInfoCollection<TInfo> Data { get; }

        //**--------------------------------------------------------------------------
        ResultInfo RequestBulkInsert(int clientID, IList<TInfo> infos);
        Task<ResultInfo> RequestBulkInsertAsyn(int clientID, IList<TInfo> infos);
        //**--------------------------------------------------------------------------
        ResultInfo RequestUpdateCacheData(int clientID, RequestInfo info);
        Task<ResultInfo> RequestUpdateCacheDataAsyn(int clientID, RequestInfo info);
        //**--------------------------------------------------------------------------
        Task<ResultInfo> RequestSynchronized(int clientID, RequestInfo info);
        //**--------------------------------------------------------------------------
        ResultInfo RequestInsert(int clientID, TInfo info);
        Task<ResultInfo> RequestInsertAsyn(int clientID, TInfo info);
        //**--------------------------------------------------------------------------
        ResultInfo RequestMDInsert(int clientID, TInfo info, object details);
        Task<ResultInfo> RequestMDInsertAsyn(int clientID, TInfo info, object details);
        //**--------------------------------------------------------------------------
        ResultInfo RequestInsert(int clientID, IList<TInfo> infos);
        Task<ResultInfo> RequestInsertAsyn(int clientID, IList<TInfo> infos);
        //**--------------------------------------------------------------------------
        ResultInfo RequestInsertOrUpdate(int clientID, TInfo info);
        Task<ResultInfo> RequestInsertOrUpdateAsyn(int clientID, TInfo info);
        //**--------------------------------------------------------------------------
        ResultInfo RequestMDInsertOrUpdate(int clientID, TInfo info, object details);
        Task<ResultInfo> RequestMDInsertOrUpdateAsyn(int clientID, TInfo info, object details);
        //**--------------------------------------------------------------------------
        ResultInfo RequestInsertOrUpdate(int clientID, IList<TInfo> info);
        Task<ResultInfo> RequestInsertOrUpdateAsyn(int clientID, IList<TInfo> info);
        //**--------------------------------------------------------------------------
        ResultInfo RequestUpdate(int clientID, TInfo info);
        Task<ResultInfo> RequestUpdateAsyn(int clientID, TInfo info);
        //**--------------------------------------------------------------------------
        ResultInfo RequestMDUpdate(int clientID, TInfo info, object details);
        Task<ResultInfo> RequestMDUpdateAsyn(int clientID, TInfo info, object details);
        //**--------------------------------------------------------------------------
        ResultInfo RequestUpdateList(int clientID, IList<TInfo> infos);
        Task<ResultInfo> RequestUpdateListAsyn(int clientID, IList<TInfo> infos);
        //**--------------------------------------------------------------------------
        ResultInfo RequestRemove(int clientID, object id);
        Task<ResultInfo> RequestRemoveAsyn(int clientID, object id);

        ResultInfo RequestRemoveList(int clientID, IList<object> ids);
        Task<ResultInfo> RequestRemoveListAsyn(int clientID, IList<object> ids);
        //**--------------------------------------------------------------------------
        ResultInfo RequestListNotifyAdd(IList<TInfo> list);
        Task<ResultInfo> RequestListNotifyAddAsyn(IList<TInfo> list);
        ResultInfo RequestNotifyAdd(TInfo info);
        Task<ResultInfo> RequestNotifyAddAsyn(TInfo info);
        ResultInfo RequestListNotifyUpdate(IList<TInfo> list);
        Task<ResultInfo> RequestListNotifyUpdateAsyn(IList<TInfo> list);
        ResultInfo RequestNotifyUpdate(TInfo info);
        Task<ResultInfo> RequestNotifyUpdateAsyn(TInfo info);
        ResultInfo RequestListNotifyRemoved(IList<object> list);
        Task<ResultInfo> RequestListNotifyRemovedAsyn(IList<TInfo> list);
        ResultInfo RequestNotifyRemoved(object id);
        Task<ResultInfo> RequestNotifyRemovedAsyn(object id);
        //**--------------------------------------------------------------------------
        ResultInfo RequestNotifyCustomized(ResultInfo info);
        Task<ResultInfo> RequestNotifyCustomizedAsyn(ResultInfo info);
        //**--------------------------------------------------------------------------
        event GeneralEventHandler<ResultInfo> ResponsedEdit;
        event GeneralEventHandler<ResultInfo> RequestFailed;

    }
}
