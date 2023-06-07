using Connect.Common.Interface;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Connect.Common.Collection
{
    public class ViewEntityCollection<TInfo, TViewEntity> : BindingList<TViewEntity>, IViewEntityCollection<TInfo>
        where TViewEntity : IViewEntity<TInfo>, new()
        where TInfo : IInfo<TInfo>, new()
    {
        #region Constructor

        public delegate void GetNewEntity(TInfo info, out TViewEntity entity);

        public ViewEntityCollection(SynchronizationContext currentContext)
        {
            CurrentContext = currentContext;
        }

        public ViewEntityCollection() { }
        public ViewEntityCollection(IInfoCollection<TInfo> infos, GetNewEntity getNewEntity, SynchronizationContext currentContext)
        {
            _getNewEntity = getNewEntity;
            CurrentContext = currentContext;
            SetBinding(infos);
        }

        public ViewEntityCollection(IInfoCollection<TInfo> infos, SynchronizationContext currentContext)
        {
            CurrentContext = currentContext;
            SetBinding(infos);
        }

        #endregion

        #region Private
        //**--------------------------------------------------------------------------------

        protected Dictionary<object, TViewEntity> InternalDictionary = new Dictionary<object, TViewEntity>();
        ListSortDirection _sortDirection;
        private PropertyDescriptor _sortProperty;

        protected readonly SynchronizationContext CurrentContext;
        private readonly GetNewEntity _getNewEntity;

        //**--------------------------------------------------------------------------------
        #endregion

        #region Public
        //**--------------------------------------------------------------------------------

        public void SetBinding(IInfoCollection<TInfo> infos)
        {
            Clear();
            InternalDictionary.Clear();
            foreach (TInfo info in infos)
            {
                TViewEntity entity;
                if (_getNewEntity == null)
                {
                    entity = new TViewEntity();
                    entity.Set(info);
                }
                else
                {
                    _getNewEntity(info, out entity);
                }
                Add(entity);
            }

            infos.Added += (sender, e) => CurrentContext.Send(state =>
            {
                TViewEntity entity;
                if (_getNewEntity == null)
                {
                    entity = new TViewEntity();
                    entity.Set(e.Data);
                }
                else
                {
                    _getNewEntity(e.Data, out entity);
                }
                Add(entity);
            }, null);

            infos.Removed += (sender, e) => CurrentContext.Send(state =>
            {
                TViewEntity entity;
                if (InternalDictionary.TryGetValue(e.Data, out entity))
                {
                    Remove(entity);
                }
            }, null);

            infos.Updated += (sender, e) => CurrentContext.Send(state =>
            {
                TViewEntity entity;
                if (InternalDictionary.TryGetValue((int)e.Data.ValueID, out entity))
                {
                    entity.Set(e.Data);
                }
            }, null);

            infos.ListAdded += (sender, e) => CurrentContext.Send(state =>
            {
                foreach (var info in e.Data)
                {
                    TViewEntity entity;
                    if (_getNewEntity == null)
                    {
                        entity = new TViewEntity();
                        entity.Set(info);
                    }
                    else
                    {
                        _getNewEntity(info, out entity);
                    }
                }
            }, null);

            infos.ListRemoved += (sender, e) => CurrentContext.Send(state =>
            {
                foreach (var id in e.Data)
                {
                    TViewEntity entity;
                    if (InternalDictionary.TryGetValue(id, out entity))
                    {
                        Remove(entity);
                    }
                }
            }, null);

            infos.ListUpdated += (sender, e) => CurrentContext.Send(state =>
            {
                foreach (var info in e.Data)
                {
                    TViewEntity entity;
                    if (InternalDictionary.TryGetValue((int)info.ValueID, out entity))
                    {
                        entity.Set(info);
                    }
                }

            }, null);
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        protected override void ClearItems()
        {
            //Debug.Assert(CurrentContext != null);
            CurrentContext.Send(state =>
            {
                try
                {
                    InternalDictionary.Clear();
                    base.ClearItems();
                }
                catch (Exception)
                {

                }

            }, null);
        }

        protected override void RemoveItem(int index)
        {
            Debug.Assert(CurrentContext != null);
            CurrentContext.Send((SendOrPostCallback)(state =>
            {
                try
                {
                    InternalDictionary.Remove((object)this[index].ValueID);
                    base.RemoveItem(index);
                }
                catch (Exception)
                {

                }

            }), null);
        }

        protected override void SetItem(int index, TViewEntity entity)
        {
            Debug.Assert(CurrentContext != null);
            CurrentContext.Send((SendOrPostCallback)(state =>
            {
                if (InternalDictionary.ContainsKey((object)entity.ValueID))
                {
                    InternalDictionary[(object)entity.ValueID] = entity;
                }
                base.SetItem(index, entity);
            }), null);
        }

        protected override void InsertItem(int index, TViewEntity entity)
        {
            Debug.Assert(CurrentContext != null);
            if (!InternalDictionary.ContainsKey(entity.ValueID))
            {
                CurrentContext.Send(state =>
                {
                    try
                    {
                        InternalDictionary.Add(entity.Info.ValueID, entity);
                        base.InsertItem(index, entity);
                    }
                    catch (Exception)
                    {

                    }

                }, null);
            }

        }

        //**--------------------------------------------------------------------------------
        #endregion

        #region Sort support
        //**--------------------------------------------------------------------------------

        protected virtual Comparison<TViewEntity> GetComparer(PropertyDescriptor prop)
        {
            /*
            Comparison<TViewEntity> comparer;
            switch (prop.Name)
            {
                case "Name":
                    comparer = new Comparison<TViewEntity>(delegate(TViewEntity x, TViewEntity y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.MyIntProperty.CompareTo(y.MyIntProperty));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    return comparer;
                    break;
            }
            */

            if (prop.PropertyType.GetInterface("IComparable") == null) return null;
            var comparison = new Comparison<TViewEntity>(delegate (TViewEntity x, TViewEntity y)
            {
                // Compare x to y if x is not null. If x is, but y isn't, we compare y
                // to x and reverse the result. If both are null, they're equal.
                if (prop.GetValue(x) != null)
                {
                    var comparable = (IComparable)prop.GetValue(x);
                    if (comparable != null)
                        return comparable.CompareTo(prop.GetValue(y)) *
                               (_sortDirection == ListSortDirection.Descending ? -1 : 1);
                }
                else if (prop.GetValue(y) != null)
                {
                    var comparable = (IComparable)prop.GetValue(y);
                    if (comparable != null)
                        return comparable.CompareTo(prop.GetValue(x)) *
                               (_sortDirection == ListSortDirection.Descending ? 1 : -1);
                }
                return 0;
            });
            return comparison;
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            /*
             Look for an appropriate sort method in the cache if not found .
             Call CreateOrderByMethod to create one. 
             Apply it to the original list.
             Notify any bound controls that the sort has been applied.
             */
            _sortProperty = prop;
            _sortDirection = direction;

            var itemsList = (List<TViewEntity>)Items;
            if (prop.PropertyType.GetInterface("IComparable") == null) return;
            var comparer = GetComparer(prop);
            if (comparer == null) return;
            itemsList.Sort(comparer);
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override bool IsSortedCore
        {
            get { return true; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return _sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return _sortProperty; }
        }

        //**--------------------------------------------------------------------------------
        #endregion
    }

}