using Connect.Common;
using Connect.Common.Common;
using Connect.Common.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.RemoteDataProvider.Interface
{
    public interface IDataClient<T>
    {
        /// <summary>
        /// Register new client
        /// </summary>
        /// <param name="name">Name of client</param>
        /// <param name="handler">Fire immediately if data is ready or waiting until data is ready to fire</param>
        /// <returns></returns>
        int RegisterClient(String name, GeneralEventHandler<int> handler);
        int RegisterClient(String name, GeneralEventHandler<int> handler, int clientID);
        int RegisterClient(String name, GeneralEventHandler<int> handler, EClientType eClientType);
        int RegisterClient(String name, GeneralEventHandler<int> handler, int clientID, EClientType eClientType);
        void RemoveClient(int clientID);

        void SetNotifyAdded(int clientID, Boolean notify);
        void SetNotifyRemoved(int clientID, Boolean notify);
        void SetNotifyUpdated(int clientID, Boolean notify);
        void SetNotifyCustomized(int clientID, Boolean notify);

        void SetAddedListener(int clientID, GeneralEventHandler<T> listener);
        void SetRemovedListener(int clientID, GeneralEventHandler<object> listener);
        void SetCustomizedListener(int clientID, GeneralEventHandler<ResultInfo> listener);
        void SetUpdatedListener(int clientID, GeneralEventHandler<T> listener);

        void SetListAddedListener(int clientID, GeneralEventHandler<IList<T>> listener);
        void SetListRemovedListener(int clientID, GeneralEventHandler<IList<object>> listener);
        void SetListUpdatedListener(int clientID, GeneralEventHandler<IList<T>> listener);
    }
}
