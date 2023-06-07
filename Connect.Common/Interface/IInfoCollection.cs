using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Connect.Common.Interface
{
    public interface IInfoCollection<TInfo> : IEnumerable where TInfo : IInfo<TInfo>
    {
        event GeneralEventHandler<TInfo> Added;
        event GeneralEventHandler<TInfo> Updated;
        event GeneralEventHandler<object> Removed;

        event GeneralEventHandler<IList<TInfo>> ListAdded;
        event GeneralEventHandler<IList<TInfo>> ListUpdated;
        event GeneralEventHandler<IList<object>> ListRemoved;
        event GeneralEventHandler<TInfo> UpdateNotFound;
        //void SetInfo(IList<TInfo> listinfos);
        bool ContainsKeys(object keys);
        void Add(TInfo info);
        void Update(TInfo info);
        void Remove(object id);
        void RemoveAll();

        void AddList(IList<TInfo> infos);
        void UpdateList(IList<TInfo> infos);
        void RemoveList(IList<object> ids);

        event EventHandler Reset;
        Boolean TryGet(object id, out TInfo info);
        IList ToList();

        long MaxID { get; set; }
    }
}
