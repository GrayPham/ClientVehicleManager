using Connect.Common.Helper;
using Connect.Common.Interface;
using DevExpress.Xpo;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Connect.Common.Collection
{
    public class InfoCollection<TInfo> : ICollection<TInfo>, IInfoCollection<TInfo> where TInfo : IInfo<TInfo>
    {
        #region Constructor

        public InfoCollection()
        {
            MaxID = int.MinValue;
            InternalDictionary = new Dictionary<string, TInfo>();
        }

        public InfoCollection(IEnumerable list)
        {
            if (list == null) return;
            MaxID = int.MinValue;
            InternalDictionary = new Dictionary<string, TInfo>();
            foreach (TInfo info in list)
            {
                if (ProviderHelper.IsNumeric(info.ValueID))
                {
                    Int64 value = Int64.Parse("" + info.ValueID);
                    if (value > MaxID) MaxID = value;
                }
                else
                {
                    MaxID++;
                }
                var type = typeof(TInfo);
                InternalDictionary.Add("" + info.ValueID, info);
            }
        }

        #endregion

        #region Protected

        protected object DictionaryLocker = new object();
        protected Dictionary<string, TInfo> InternalDictionary;

        #endregion

        #region ICollection interface

        public long MaxID { get; set; }

        public int Count
        {
            get { return InternalDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Contains(TInfo info)
        {
            lock (DictionaryLocker)
            {
                return InternalDictionary.ContainsValue(info);
            }

        }

        public void CopyTo(TInfo[] array, int arrayIndex)
        {
            lock (DictionaryLocker)
            {
                InternalDictionary.Values.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(TInfo info)
        {
            bool b;
            lock (DictionaryLocker)
            {
                b = InternalDictionary.Remove("" + info.ValueID);
            }
            if (Removed != null) Removed(this, new EventArgs<object>(info.ValueID));
            return b;
        }

        public void Clear()
        {
            lock (DictionaryLocker)
            {
                InternalDictionary.Clear();
            }
            if (Reset != null) Reset(this, EventArgs.Empty);
        }

        #endregion

        #region Interface implementation

        IEnumerator<TInfo> IEnumerable<TInfo>.GetEnumerator()
        {
            lock (DictionaryLocker)
            {
                foreach (var item in InternalDictionary.Values)
                {
                    yield return item;
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            lock (DictionaryLocker)
            {
                if (InternalDictionary == null) return null;
                return InternalDictionary.Values.GetEnumerator();
            }
        }

        public event GeneralEventHandler<TInfo> Added;
        public event GeneralEventHandler<TInfo> Updated;
        public event GeneralEventHandler<object> Removed;
        public event GeneralEventHandler<IList<TInfo>> ListAdded;
        public event GeneralEventHandler<IList<TInfo>> ListUpdated;
        public event GeneralEventHandler<IList<object>> ListRemoved;
        public event EventHandler Reset;
        public event GeneralEventHandler<TInfo> UpdateNotFound;
        public void Add(TInfo info)
        {
            lock (DictionaryLocker)
            {
                if (InternalDictionary.ContainsKey("" + info.ValueID)) return;
                if (ProviderHelper.IsNumeric(info.ValueID))
                {
                    Int64 value = Int64.Parse("" + info.ValueID);
                    if (value > MaxID) MaxID = value;
                }
                else
                {
                    MaxID++;
                }
                InternalDictionary.Add("" + info.ValueID, info);
            }
            if (Added != null) Added(this, new EventArgs<TInfo>(info));
        }

        public void AddList(IList<TInfo> infos)
        {
            var addedInfos = new List<TInfo>();
            foreach (var info in infos)
            {
                lock (DictionaryLocker)
                {
                    if (InternalDictionary.ContainsKey("" + info.ValueID)) continue;
                    InternalDictionary.Add("" + info.ValueID, info);
                }
                addedInfos.Add(info);
            }
            if (ListAdded != null) ListAdded(this, new EventArgs<IList<TInfo>>(addedInfos));
        }

        public void Update(TInfo info)
        {
            TInfo val;
            lock (DictionaryLocker)
            {
                if (!InternalDictionary.TryGetValue("" + info.ValueID, out val))
                {
                    if (UpdateNotFound != null) UpdateNotFound(this, new EventArgs<TInfo>(info));
                    //_log.Warn($"{GetType().Name} : {System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() } ==>{typeof(TInfo).Name} ValueID is null: {info.ValueID}");
                    return;
                }
            }
            val.Copy(info);
            if (Updated != null) Updated(this, new EventArgs<TInfo>(info));
        }

        public void UpdateList(IList<TInfo> infos)
        {
            var udpatedInfos = new List<TInfo>();
            foreach (var info in infos)
            {
                TInfo val;
                lock (DictionaryLocker)
                {
                    if (!InternalDictionary.TryGetValue("" + info.ValueID, out val))
                    {
                        if (UpdateNotFound != null) UpdateNotFound(this, new EventArgs<TInfo>(info));
                        continue;
                    }
                }
                val.Copy(info);
                udpatedInfos.Add(info);
            }
            if (ListUpdated != null) ListUpdated(this, new EventArgs<IList<TInfo>>(udpatedInfos));
        }

        public void Remove(object id)
        {
            lock (DictionaryLocker)
            {
                if (!InternalDictionary.Remove("" + id)) return;
            }
            if (Removed != null) Removed(this, new EventArgs<object>(id));
        }

        public void RemoveList(IList<object> ids)
        {
            var deletedIds = new List<object>();
            foreach (var id in ids)
            {
                lock (DictionaryLocker)
                {
                    if (InternalDictionary.Remove("" + id)) deletedIds.Add(id);
                }
            }
            if (ListRemoved != null) ListRemoved(this, new EventArgs<IList<object>>(deletedIds));
        }

        public void RemoveAll()
        {
            var deletedIds = new List<object>();
            foreach (TInfo info in InternalDictionary.Values)
            {
                deletedIds.Add(info.ValueID);
            }
            lock (DictionaryLocker)
            {
                InternalDictionary.Clear();
            }
            if (ListRemoved != null) ListRemoved(this, new EventArgs<IList<object>>(deletedIds));
        }

        public bool TryGet(object id, out TInfo info)
        {
            lock (DictionaryLocker)
            {
                return InternalDictionary.TryGetValue("" + id, out info);
            }
        }

        public IList ToList()
        {
            var list = new List<TInfo>();
            lock (DictionaryLocker)
            {
                foreach (var item in InternalDictionary.Values)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        //public void SetInfo(IList<TInfo> list)
        //{
        //    MaxID = int.MinValue;
        //    InternalDictionary.Clear();
        //    foreach (TInfo info in list)
        //    {
        //        if (info.ID > MaxID) MaxID = info.ID;
        //        InternalDictionary.Add(info.ID, info);
        //    }
        //}
        public bool ContainsKeys(object key)
        {
            return InternalDictionary.ContainsKey("" + key);
        }

        #endregion
    }
}