using Connect.Common;
using Connect.Common.Contract;
using Connect.Common.Helper;
using Connect.Common.Interface;
using Connect.Common.Logging;
using nsFramework.Common.Pattern;
using System;
using System.Collections.Generic;

namespace nsConnect.RemoteDataProvider.Client
{
    public class FuncClientDataService<TInfo> where TInfo : IInfo<TInfo>, new()
    {

        //**------------------------------------------------------------------------------------

        #region Member
        protected ILog _log;
        protected readonly object GetDataSyncLocker = new object();
        protected readonly object GetDataAllSyncLocker = new object();
        protected readonly object TryGetSyncLocker = new object();
        protected readonly object GetIsActivitySyncLocker = new object();

        #region Event Return
        public event ServiceEventHandler<TInfo> Added;
        public event ServiceEventHandler<TInfo> Updated;
        public event GeneralEventHandler<UnknownUpdatedInfo> UnknownUpdated;
        public event GeneralEventHandler<ExceptionInfo> Errored;
        public event ServiceEventHandler<object> Removed;
        public event ServiceEventHandler<IList<TInfo>> ListAdded;
        public event ServiceEventHandler<IList<TInfo>> ListUpdated;
        public event ServiceEventHandler<IList<object>> ListRemoved;
        public event GeneralEventHandler<object> Requested;

        protected void SetErrored(string className, string message, string trac, string data, Exception ex)
        {
            _log.SError(className, message, trac, data);
        }

        protected virtual void SetAdded(ServiceErrorInfo infoError, TInfo info)
        {
            if (Added != null) Added(this, new ServiceEventArgs<TInfo>(infoError, info));
        }
        protected virtual void SetUnknownUpdated(UnknownUpdatedInfo info)
        {
            if (UnknownUpdated != null) UnknownUpdated(this, new EventArgs<UnknownUpdatedInfo>(info));
        }
        protected virtual void SetErrored(ExceptionInfo info)
        {
            if (Errored != null) Errored(this, new EventArgs<ExceptionInfo>(info));
        }
        protected virtual void SetListAdded(ServiceErrorInfo infoError, IList<TInfo> infos)
        {
            if (ListAdded != null) ListAdded(this, new ServiceEventArgs<IList<TInfo>>(infoError, infos));
        }
        protected virtual void SetUpdated(ServiceErrorInfo infoError, TInfo info)
        {
            if (Updated != null) Updated(this, new ServiceEventArgs<TInfo>(infoError, info));
        }
        protected virtual void SetListUpdated(ServiceErrorInfo infoError, IList<TInfo> infos)
        {
            if (ListUpdated != null) ListUpdated(this, new ServiceEventArgs<IList<TInfo>>(infoError, infos));
        }
        protected virtual void SetRemoved(ServiceErrorInfo infoError, object id)
        {
            if (Removed != null) Removed(this, new ServiceEventArgs<object>(infoError, id));
        }
        protected virtual void SetListRemoved(ServiceErrorInfo info, IList<object> ids)
        {
            if (Removed != null) ListRemoved(this, new ServiceEventArgs<IList<object>>(info, ids));
        }
        public void SetRequested(object value)
        {
            if (Requested != null) Requested(this, new EventArgs<object>(value));
        }

        #endregion

        #endregion

        //**------------------------------------------------------------------------------------

        #region Constructor
        public FuncClientDataService(ILog log)
        {
            _log = log ?? Singleton<DummyLog>.Instance;
        }
        public FuncClientDataService()
        {
            _log = Singleton<DummyLog>.Instance;
        }
        #endregion

        //**------------------------------------------------------------------------------------
        public virtual ResultInfo OnAddLinQ(TInfo info)
        {
            try
            {
                //using (var context = new VSCDataContext(Connect.connectStringUpdate))
                //{
                //    context.Connection.Open();
                //    using (context.Transaction = context.Connection.BeginTransaction())
                //    {
                //var table = ToEntity(info);
                //DefaultInser(table);
                //context.GetTable<TEntity>().InsertOnSubmit(table);
                //context.SubmitChanges();
                //OnAddRefference(context, table, info);
                //context.Transaction.Commit();
                //info = context.ExecuteQuery<TInfo>(string.Format(GlobalServiceKey.CSQLTryGet, info.SQLData(), ToInfo(table).ValueID)).FirstOrDefault();
                //    }
                //}
                // if Service using data syschronized
                //if (IsDataSync) InfoCollection.Add(info);
                //NotifyAdded(info);
                return new ResultInfo() { Status = false, Record = 1, Data = info };
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        //**------------------------------------------------------------------------------------
        public virtual ResultInfo OnAddLinQ(IList<TInfo> infos)
        {
            try
            {
                //using (var context = new VSCDataContext(Connect.connectStringUpdate))
                //{
                //    context.Connection.Open();
                //    using (context.Transaction = context.Connection.BeginTransaction())
                //    {
                //var table = ToEntity(info);
                //DefaultInser(table);
                //context.GetTable<TEntity>().InsertOnSubmit(table);
                //context.SubmitChanges();
                //OnAddRefference(context, table, info);
                //context.Transaction.Commit();
                //info = context.ExecuteQuery<TInfo>(string.Format(GlobalServiceKey.CSQLTryGet, info.SQLData(), ToInfo(table).ValueID)).FirstOrDefault();
                //    }
                //}
                // if Service using data syschronized
                //if (IsDataSync) InfoCollection.Add(info);
                //NotifyAdded(info);
                return new ResultInfo() { Status = false, Record = 1, Data = infos };
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(infos), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual ResultInfo OnUpdateLinQ(IList<TInfo> infos)
        {
            try
            {
                //using (var context = new VSCDataContext(Connect.connectStringUpdate))
                //{
                //    context.Connection.Open();
                //    using (context.Transaction = context.Connection.BeginTransaction())
                //    {
                //var table = ToEntity(info);
                //DefaultInser(table);
                //context.GetTable<TEntity>().InsertOnSubmit(table);
                //context.SubmitChanges();
                //OnAddRefference(context, table, info);
                //context.Transaction.Commit();
                //info = context.ExecuteQuery<TInfo>(string.Format(GlobalServiceKey.CSQLTryGet, info.SQLData(), ToInfo(table).ValueID)).FirstOrDefault();
                //    }
                //}
                // if Service using data syschronized
                //if (IsDataSync) InfoCollection.Add(info);
                //NotifyAdded(info);
                return new ResultInfo() { Status = false, Record = 1, Data = infos };
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(infos), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual ResultInfo OnUpdateLinQ(TInfo info)
        {
            try
            {
                ResultInfo result = new ResultInfo();
                //using (var context = new VSCDataContext(Connect.connectStringUpdate))
                //{
                //    var mapping = context.Mapping.GetTable(typeof(TEntity));
                //    var pkfield = mapping.RowType.DataMembers.SingleOrDefault(d => d.IsPrimaryKey);
                //    if (pkfield != null)
                //    {
                //        var param = Expression.Parameter(typeof(TEntity), "t");
                //        var predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(Expression.Property(param, pkfield.Name), Expression.Constant(info.ValueID)), param);
                //        var table = context.GetTable<TEntity>().FirstOrDefault(predicate);
                //        if (table != null)
                //        {
                //            UpdateTo(table, info);
                //            try
                //            {
                //                context.SubmitChanges(ConflictMode.ContinueOnConflict);
                //            }
                //            catch (ChangeConflictException ex)
                //            {
                //                context.ChangeConflicts.ResolveAll(RefreshMode.KeepCurrentValues);
                //                context.SubmitChanges(ConflictMode.ContinueOnConflict);
                //            }
                //            result.Status = true;
                //            result.Record = 1;
                //            result.Data = info;
                //            OnUpdateRefference(context, table, info);
                //        }
                //        else
                //        {
                //            result.ErrorMessage = @"Không tìm thấy thông tin cần cập nhật";
                //            result.Status = false;
                //            result.Record = 0;
                //        }
                //    }
                //    // var table = context.GetTable<TEntity>().FirstOrDefault(t => ReflectionHelper.GetPropValue(t, info.PrimaryKey()) == info.ValueID);

                //}
                //if (_isDataSync) InfoCollection.Update(info);
                //NotifyUpdated(clientID, info);
                return result;
            }
            catch (Exception ex)
            {
                _log.SError(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info));
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual ResultInfo OnInsertOrUpdateLinQ(TInfo info)
        {
            try
            {
                //using (var context = new VSCDataContext(Connect.connectStringUpdate))
                //{
                //    context.Connection.Open();
                //    using (context.Transaction = context.Connection.BeginTransaction())
                //    {
                //var table = ToEntity(info);
                //DefaultInser(table);
                //context.GetTable<TEntity>().InsertOnSubmit(table);
                //context.SubmitChanges();
                //OnAddRefference(context, table, info);
                //context.Transaction.Commit();
                //info = context.ExecuteQuery<TInfo>(string.Format(GlobalServiceKey.CSQLTryGet, info.SQLData(), ToInfo(table).ValueID)).FirstOrDefault();
                //    }
                //}
                // if Service using data syschronized
                //if (IsDataSync) InfoCollection.Add(info);
                //NotifyAdded(info);
                return new ResultInfo() { Status = false, Record = 1, Data = info };
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.InfoToJson(info), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual ResultInfo OnInsertOrUpdateLinQ(IList<TInfo> infos)
        {
            try
            {
                //using (var context = new VSCDataContext(Connect.connectStringUpdate))
                //{
                //    context.Connection.Open();
                //    using (context.Transaction = context.Connection.BeginTransaction())
                //    {
                //var table = ToEntity(info);
                //DefaultInser(table);
                //context.GetTable<TEntity>().InsertOnSubmit(table);
                //context.SubmitChanges();
                //OnAddRefference(context, table, info);
                //context.Transaction.Commit();
                //info = context.ExecuteQuery<TInfo>(string.Format(GlobalServiceKey.CSQLTryGet, info.SQLData(), ToInfo(table).ValueID)).FirstOrDefault();
                //    }
                //}
                // if Service using data syschronized
                //if (IsDataSync) InfoCollection.Add(info);
                //NotifyAdded(info);
                return new ResultInfo() { Status = false, Record = 1, Data = infos };
            }
            catch (Exception ex)
            {
                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(infos), ex);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual ResultInfo OnRemoveLinQ(object id)
        {
            try
            {
                ResultInfo result = new ResultInfo();
                //using (var context = new VSCDataContext(Connect.connectStringUpdate))
                //{
                //    try
                //    {
                //        var mapping = context.Mapping.GetTable(typeof(TEntity));
                //        var pkfield = mapping.RowType.DataMembers.SingleOrDefault(d => d.IsPrimaryKey);
                //        if (pkfield != null)
                //        {
                //            var param = Expression.Parameter(typeof(TEntity), "t");
                //            var predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(Expression.Property(param, pkfield.Name), Expression.Constant(Convert.ChangeType(id, pkfield.Type))), param);
                //            var table = context.GetTable<TEntity>().FirstOrDefault(predicate);
                //            if (table != null)
                //            {
                //                var infoValidate = ValidateRemove(context, table);
                //                if (infoValidate.Status)
                //                {
                //                    context.GetTable<TEntity>().DeleteOnSubmit(table);
                //                    context.SubmitChanges(ConflictMode.ContinueOnConflict);
                //                    result.Status = true;
                //                    result.Record = 1;
                //                    result.Data = id;
                //                    OnDeleteRefference(context, table);
                //                    if (_isDataSync) InfoCollection.Remove(id);
                //                    NotifyRemoved(clientID, id);
                //                }
                //                else
                //                {
                //                    result.ErrorMessage = infoValidate.ErrorMessage;
                //                    result.Status = false;
                //                    result.Record = 0;
                //                }
                //            }
                //            else
                //            {
                //                result.ErrorMessage = @"Không tìm thấy thông tin cần xóa";
                //                result.Status = false;
                //                result.Record = 0;
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
                //        return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
                //    }
                //    finally
                //    {
                //        context.Dispose();
                //    }
                //}

                return result;
            }
            catch (Exception ex)
            {
                _log.SError(this.GetType().Name, ex.Message, ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        public virtual ResultInfo OnRemoveLinQ(IList<object> ids)
        {
            try
            {
                ResultInfo result = new ResultInfo();
                //using (var context = new VSCDataContext(Connect.connectStringUpdate1))
                //{
                //    var mapping = context.Mapping.GetTable(typeof(TEntity));
                //    var pkfield = mapping.RowType.DataMembers.SingleOrDefault(d => d.IsPrimaryKey);
                //    if (pkfield != null)
                //    {
                //        var param = Expression.Parameter(typeof(TEntity), "t");
                //        var idsRemove = new List<object>();
                //        foreach (var item in ids)
                //        {
                //            try
                //            {
                //                var predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(Expression.Property(param, pkfield.Name), Expression.Constant(Convert.ChangeType(item, pkfield.Type))), param);
                //                var table = context.GetTable<TEntity>().FirstOrDefault(predicate);
                //                if (table != null)
                //                {
                //                    var infoValidate = ValidateRemove(context, table);
                //                    if (infoValidate.Status)
                //                    {
                //                        context.GetTable<TEntity>().DeleteOnSubmit(table);
                //                        context.SubmitChanges(ConflictMode.ContinueOnConflict);
                //                        OnDeleteRefference(context, table);
                //                        idsRemove.Add(item);
                //                    }
                //                    else
                //                    {
                //                        result.ErrorMessage = infoValidate.ErrorMessage;
                //                    }
                //                }
                //            }
                //            catch (Exception ex)
                //            {
                //                result.ErrorMessage = ex.Message;
                //                break;
                //            }
                //        }
                //        result.Status = true;
                //        result.Data = idsRemove;
                //        if (_isDataSync) InfoCollection.RemoveList(idsRemove);
                //        ListNotifyRemoved(clientID, idsRemove);
                //    }
                //}
                return result;
            }
            catch (Exception ex)
            {
                _log.SError(this.GetType().Name, ex.Message, ex.StackTrace, JsonHelper.ListInfoToJson(ids));
                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
            }
        }
        //**------------------------------------------------------------------------------------

        #region Search method

        //public virtual ResultInfo GetDataLinQ(RequestInfo request)
        //{
        //    ResultInfo resultInfo = new ResultInfo();
        //    try
        //    {
        //        using (var context = new VSCDataContext(Connect.connectStringSelect))
        //        {
        //            try
        //            {
        //                TInfo info = new TInfo();
        //                resultInfo.TSql = string.Format(GlobalServiceKey.TSQLTryGet, info.SQLData(), ProviderHelper.OToInt64(request.ID),
        //                    request.Owner, request.UserName, request.FromDate.ToString(SystemSetting.FDate), request.ToDate.ToString(SystemSetting.FDate), request.ID, request.TSQL);
        //                resultInfo.Data = context.ExecuteQuery<TInfo>(resultInfo.TSql).ToList();
        //                resultInfo.Status = true;
        //                return resultInfo;
        //            }
        //            catch (Exception ex)
        //            {
        //                // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //            }
        //            finally
        //            {
        //                context.Dispose();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //        return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //    }
        //}
        //public virtual ResultInfo GetDataIsActivityByBranchLinQ(RequestInfo request)
        //{
        //    ResultInfo resultInfo = new ResultInfo();

        //    try
        //    {

        //        using (var context = new VSCDataContext(Connect.connectStringSelect))
        //        {
        //            try
        //            {
        //                TInfo info = new TInfo();
        //                resultInfo.Data = context.ExecuteQuery<TInfo>(string.Format(GlobalServiceKey.CSQLIsActivityByBranch, info.SQLData(), request.Top, request.BranchID)).ToList();
        //                resultInfo.Status = true;
        //                return resultInfo;
        //            }
        //            catch (Exception ex)
        //            {
        //                // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //            }
        //            finally
        //            {
        //                context.Dispose();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //        return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //    }
        //    return resultInfo;
        //}
        ////**------------------------------------------------------------------------------------
        //public virtual Task<ResultInfo> GetDataIsActivityLinQAsync(int top, int branchID)
        //{
        //    return Task.Run<ResultInfo>(() =>
        //   {
        //       try
        //       {
        //           using (var context = new VSCDataContext(Connect.connectStringSelect))
        //           {
        //               try
        //               {
        //                   ResultInfo resultInfo = new ResultInfo();
        //                   TInfo info = new TInfo();
        //                   resultInfo.Data = context.ExecuteQuery<TInfo>(string.Format(GlobalServiceKey.CSQLIsActivityByBranch, info.SQLData(), top, branchID)).ToList();
        //                   resultInfo.Status = true;
        //                   return resultInfo;
        //               }
        //               catch (Exception ex)
        //               {
        //                   // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //                   SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                   return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //               }
        //               finally
        //               {
        //                   context.Dispose();
        //               }
        //           }
        //       }
        //       catch (Exception ex)
        //       {
        //           // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //           SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //           return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //       }
        //   });
        //}
        //public virtual ResultInfo GetDataIsActivityByBranchLinQ(object id, int branchID, int top)
        //{
        //    ResultInfo resultInfo = new ResultInfo();

        //    try
        //    {

        //        using (var context = new VSCDataContext(Connect.connectStringSelect))
        //        {
        //            try
        //            {
        //                TInfo info = new TInfo();
        //                List<TInfo> list = context.ExecuteQuery<TInfo>(string.Format(GlobalServiceKey.CSQLGetDataIsActivityByGroupByBranch, info.SQLData(), id, top, branchID)).ToList<TInfo>();
        //                context.Dispose();
        //                resultInfo.Status = true;
        //                resultInfo.Data = list;
        //                return resultInfo;
        //            }
        //            catch (Exception ex)
        //            {
        //                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //            }
        //            finally
        //            {
        //                context.Dispose();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //        return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //    }

        //    return resultInfo;
        //}
        //public virtual ResultInfo ListToBBAll<TViewEntity>(IList<TInfo> infos, SynchronizationContext currentContext) where TViewEntity : class, IViewEntity<TInfo>, new()
        //{

        //    BindingList<TViewEntity> ViewEntityCollection = new ViewEntityCollection<TInfo, TViewEntity>(currentContext);
        //    try
        //    {
        //        if (infos != null)
        //        {
        //            TViewEntity Entity;
        //            foreach (TInfo info in infos)
        //            {
        //                Entity = new TViewEntity();
        //                Entity.Set(info);
        //                ViewEntityCollection.Add(Entity);
        //            }
        //        }
        //        ResultInfo resultInfo = new ResultInfo();
        //        resultInfo.Data = ViewEntityCollection;
        //        resultInfo.Status = true;
        //        return resultInfo;

        //    }
        //    catch (Exception ex)
        //    {
        //        // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //        return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //    }
        //}
        //public virtual ResultInfo ListToBBAll<TViewEntity>(IList<TInfo> infos, SynchronizationContext currentContext, Func<TInfo, bool> predicateh) where TViewEntity : class, IViewEntity<TInfo>, new()
        //{
        //    BindingList<TViewEntity> ViewEntityCollection = new ViewEntityCollection<TInfo, TViewEntity>(currentContext);
        //    try
        //    {
        //        if (infos != null)
        //        {
        //            IList<TInfo> list = infos.Where(predicateh).ToList();
        //            TViewEntity Entity;
        //            //Parallel.ForEach(list, item =>
        //            //{
        //            //    Entity = new TViewEntity();
        //            //    Entity.Set(item);
        //            //    ViewEntityCollection.Add(Entity);
        //            //});
        //            foreach (TInfo info in list)
        //            {
        //                Entity = new TViewEntity();
        //                Entity.Set(info);
        //                ViewEntityCollection.Add(Entity);
        //            }
        //        }
        //        ResultInfo resultInfo = new ResultInfo();
        //        resultInfo.Data = ViewEntityCollection;
        //        resultInfo.Status = true;
        //        return resultInfo;

        //    }
        //    catch (Exception ex)
        //    {
        //        // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //        return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //    }
        //}
        ////**------------------------------------------------------------------------------------
        //public virtual Task<ResultInfo> GetDataBBIsActivityLinQAsync<TViewEntity>(SynchronizationContext currentContext, RequestInfo request) where TViewEntity : class, IViewEntity<TInfo>, new()
        //{
        //    return Task.Run<ResultInfo>(() =>
        //    {

        //        BindingList<TViewEntity> ViewEntityCollection = new ViewEntityCollection<TInfo, TViewEntity>(currentContext);
        //        try
        //        {
        //            using (var context = new VSCDataContext(Connect.connectStringSelect))
        //            {
        //                try
        //                {
        //                    TInfo _info = new TInfo();
        //                    var tSql = string.Format(GlobalServiceKey.TSQLIsActivity, _info.SQLData(), request.BranchID,
        //                    request.Owner, request.UserName, request.FromDate.ToString(SystemSetting.FDate), request.ToDate.ToString(SystemSetting.FDate), request.ModelName, request.TSQL);
        //                    List<TInfo> list = context.ExecuteQuery<TInfo>(tSql).ToList();
        //                    if (list != null)
        //                    {
        //                        TViewEntity Entity;
        //                        //Parallel.ForEach(list, item =>
        //                        //{
        //                        //    Entity = new TViewEntity();
        //                        //    Entity.Set(item);
        //                        //    ViewEntityCollection.Add(Entity);
        //                        //});
        //                        foreach (TInfo info in list)
        //                        {
        //                            Entity = new TViewEntity();
        //                            Entity.Set(info);
        //                            ViewEntityCollection.Add(Entity);
        //                        }
        //                    }
        //                    ResultInfo resultInfo = new ResultInfo();
        //                    resultInfo.Data = ViewEntityCollection;
        //                    resultInfo.Status = true;
        //                    return resultInfo;
        //                }
        //                catch (Exception ex)
        //                {
        //                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //                }
        //                finally
        //                {
        //                    context.Dispose();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //            SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //            return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //        }
        //    });
        //}
        ////**------------------------------------------------------------------------------------
        //public virtual ResultInfo GetDataBBAllByFuncLinQ<TViewEntity>(SynchronizationContext currentContext, RequestInfo reqInfo) where TViewEntity : class, IViewEntity<TInfo>, new()
        //{
        //    ResultInfo resultInfo = new ResultInfo();
        //    BindingList<TViewEntity> ViewEntityCollection = new ViewEntityCollection<TInfo, TViewEntity>(currentContext);
        //    try
        //    {
        //        using (var context = new VSCDataContext(Connect.connectStringSelect))
        //        {
        //            try
        //            {
        //                TInfo _info = new TInfo();
        //                string sql = string.Format(GlobalServiceKey.TSQLGetDataAll, _info.SQLData(), reqInfo.BranchID,
        //                    reqInfo.Owner, reqInfo.UserName, reqInfo.FromDate.ToString(SystemSetting.FDate), reqInfo.ToDate.ToString(SystemSetting.FDate), reqInfo.ModelName, reqInfo.TSQL);
        //                List<TInfo> list = context.ExecuteQuery<TInfo>(sql).Where((Func<TInfo, bool>)reqInfo.Data).Take(reqInfo.Top).ToList();
        //                if (list != null)
        //                {
        //                    TViewEntity Entity;
        //                    foreach (TInfo info in list)
        //                    {
        //                        Entity = new TViewEntity();
        //                        Entity.Set(info);
        //                        ViewEntityCollection.Add(Entity);
        //                    }
        //                }
        //                resultInfo.Data = ViewEntityCollection;
        //                resultInfo.Status = true;
        //                return resultInfo;
        //            }
        //            catch (Exception ex)
        //            {
        //                // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //            }
        //            finally
        //            {
        //                context.Dispose();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //        return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //    }
        //    return resultInfo;
        //}
        //public virtual Task<ResultInfo> GetDataBBAllByFuncLinQAsync<TViewEntity>(SynchronizationContext currentContext, RequestInfo reqInfo) where TViewEntity : class, IViewEntity<TInfo>, new()
        //{
        //    return Task.Run<ResultInfo>(() =>
        //   {
        //       BindingList<TViewEntity> ViewEntityCollection = new ViewEntityCollection<TInfo, TViewEntity>(currentContext);
        //       try
        //       {
        //           using (var context = new VSCDataContext(Connect.connectStringSelect))
        //           {
        //               try
        //               {
        //                   TInfo _info = new TInfo();
        //                   string sql = string.Format(GlobalServiceKey.TSQLGetDataAll, _info.SQLData(), reqInfo.BranchID,
        //                    reqInfo.Owner, reqInfo.UserName, reqInfo.FromDate.ToString(SystemSetting.FDate), reqInfo.ToDate.ToString(SystemSetting.FDate), reqInfo.ModelName, reqInfo.TSQL);
        //                   List<TInfo> list = context.ExecuteQuery<TInfo>(sql).Where((Func<TInfo, bool>)reqInfo.Data).Take(reqInfo.Top).ToList();
        //                   if (list != null)
        //                   {
        //                       TViewEntity Entity;
        //                       //Parallel.ForEach(list, item =>
        //                       //{
        //                       //    Entity = new TViewEntity();
        //                       //    Entity.Set(item);
        //                       //    ViewEntityCollection.Add(Entity);
        //                       //});
        //                       foreach (TInfo info in list)
        //                       {
        //                           Entity = new TViewEntity();
        //                           Entity.Set(info);
        //                           ViewEntityCollection.Add(Entity);
        //                       }
        //                   }
        //                   ResultInfo resultInfo = new ResultInfo();
        //                   resultInfo.Data = ViewEntityCollection;
        //                   resultInfo.Status = true;
        //                   return resultInfo;
        //               }
        //               catch (Exception ex)
        //               {
        //                   // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //                   SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                   return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //               }
        //               finally
        //               {
        //                   context.Dispose();
        //               }
        //           }
        //       }
        //       catch (Exception ex)
        //       {
        //           // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //           SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //           return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //       }
        //   });
        //}
        ////**------------------------------------------------------------------------------------
        //public virtual Task<ResultInfo> GetDataBBGroupByLinQAsync<TViewEntity>(SynchronizationContext currentContext, RequestInfo reqInfo) where TViewEntity : class, IViewEntity<TInfo>, new()
        //{

        //    return Task.Run<ResultInfo>(() =>
        //    {
        //        BindingList<TViewEntity> ViewEntityCollection = new ViewEntityCollection<TInfo, TViewEntity>(currentContext);
        //        try
        //        {
        //            ResultInfo resultInfo = new ResultInfo();
        //            using (var context = new VSCDataContext(Connect.connectStringSelect))
        //            {
        //                try
        //                {
        //                    TInfo _info = new TInfo();
        //                    resultInfo.TSql = string.Format(GlobalServiceKey.TSQLGetDataIsActivityByGroup, _info.SQLData(), ProviderHelper.OToInt64(reqInfo.ID), reqInfo.BranchID,
        //                    reqInfo.Owner, reqInfo.UserName, reqInfo.FromDate.ToString(SystemSetting.FDate), reqInfo.ToDate.ToString(SystemSetting.FDate), reqInfo.ID, reqInfo.TSQL);
        //                    List<TInfo> list = context.ExecuteQuery<TInfo>(resultInfo.TSql).ToList<TInfo>();
        //                    if (list != null)
        //                    {
        //                        TViewEntity Entity;
        //                        foreach (TInfo info in list)
        //                        {
        //                            Entity = new TViewEntity();
        //                            Entity.Set(info);
        //                            ViewEntityCollection.Add(Entity);
        //                        }

        //                    }
        //                    context.Dispose();
                            
        //                    resultInfo.Data = ViewEntityCollection;
        //                    resultInfo.Status = true;
        //                    return resultInfo;
        //                }
        //                catch (Exception ex)
        //                {
        //                    // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //                    SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                    return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //                }
        //                finally
        //                {
        //                    context.Dispose();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //            return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //        }
        //    });
        //}
        ////**------------------------------------------------------------------------------------
        //public virtual ResultInfo GetByWhere(string sqlWhere)
        //{
        //    ResultInfo resultInfo = new ResultInfo();
        //    using (var context = new VSCDataContext(Connect.connectStringSelect))
        //    {
        //        try
        //        {
        //            TInfo infotmp = new TInfo();
        //            resultInfo.TSql = string.Format(GlobalServiceKey.CSQLByWhere, infotmp.SQLData(), 999999, sqlWhere);
        //            resultInfo.Data = context.ExecuteQuery<TInfo>(resultInfo.TSql).ToList();
        //            resultInfo.Status = true;
        //            return resultInfo;
        //        }
        //        catch (Exception ex)
        //        {
        //            // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //            SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //            return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //        }
        //        finally
        //        {
        //            context.Dispose();
        //        }
        //    }
        //    return resultInfo;
        //}
        #endregion

        //-------------------------------------------------

        #region Exec
        //public ResultInfo ExecuteQueryS2InfoLinQ(RequestInfo request)
        //{
        //    ResultInfo resultInfo = new ResultInfo();

        //    try
        //    {
        //        using (var context = new VSCDataContext(Connect.connectStringSelect))
        //        {
        //            try
        //            {
        //                resultInfo.Data = context.ExecuteQuery<TInfo>(request.TSQL).ToList();
        //                resultInfo.Status = true;
        //                return resultInfo;
        //            }
        //            catch (Exception ex)
        //            {
        //                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //            }
        //            finally
        //            {
        //                context.Dispose();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //        return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //    }
        //    return resultInfo;
        //}
        //public ResultInfo ExecuteQueryS2InfoLinQ<T>(RequestInfo request) where T : class, IInfo<T>, new()
        //{
        //    ResultInfo resultInfo = new ResultInfo();

        //    try
        //    {
        //        using (var context = new VSCDataContext(Connect.connectStringSelect))
        //        {
        //            try
        //            {
        //                resultInfo.Data = context.ExecuteQuery<T>(request.TSQL).ToList();
        //                resultInfo.Status = true;
        //                return resultInfo;
        //            }
        //            catch (Exception ex)
        //            {
        //                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //            }
        //            finally
        //            {
        //                context.Dispose();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //        return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //    }
        //    return resultInfo;
        //}
        //public ResultInfo ExecuteQueryS2DataTableLinQ(RequestInfo request)
        //{
        //    ResultInfo resultInfo = new ResultInfo();
        //    try
        //    {
        //        using (var context = new SqlConnection(Connect.connectStringSelect))
        //        {
        //            try
        //            {
        //                DataTable dataTable = new DataTable();
        //                SqlCommand cmd = new SqlCommand(request.TSQL, context);
        //                SqlDataAdapter da = new SqlDataAdapter(cmd);
        //                da.Fill(dataTable);
        //                cmd.Dispose();
        //                da.Dispose();
        //                resultInfo.Data = dataTable;
        //                resultInfo.Status = true;
        //                return resultInfo;
        //            }
        //            catch (Exception ex)
        //            {
        //                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //            }
        //            finally
        //            {
        //                context.Dispose();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //        return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //    }

        //    return resultInfo;
        //}
        //public ResultInfo ExecuteCommandLinQ(RequestInfo request)
        //{
        //    ResultInfo resultInfo = new ResultInfo();
        //    try
        //    {
        //        using (var context = new VSCDataContext(Connect.connectStringUpdate))
        //        {
        //            try
        //            {
        //                TInfo info = new TInfo();
        //                int result = context.ExecuteCommand(request.TSQL);
        //                resultInfo.Status = true;
        //                resultInfo.Data = result;
        //                resultInfo.Record = result;
        //                return resultInfo;
        //            }
        //            catch (Exception ex)
        //            {
        //                SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //                return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //            }
        //            finally
        //            {
        //                context.Dispose();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //        SetErrored(this.GetType().Name, ex.Message, ex.StackTrace, "", ex);
        //        return new ResultInfo() { Status = false, ErrorMessage = ex.Message };
        //    }

        //}
        #endregion

        //-------------------------------------------------

        #region Public Method
        //public virtual void Copy<T>(TInfo info, T des) where T : class, new()
        //{
        //    try
        //    {
        //        //Read Attribute Names and Types
        //        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        //        var objFieldNames = des.GetType().GetProperties(flags).Cast<PropertyInfo>().
        //            Select(item => new
        //            {
        //                Name = item.Name,
        //                Type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType,
        //                value = item.GetValue(des, null)
        //            }).ToList();

        //        foreach (var item in objFieldNames)
        //        {
        //            PropertyInfo propertyInfos = info.GetType().GetProperty(item.Name);
        //            PropertyInfo propertydes = des.GetType().GetProperty(item.Name);
        //            if (propertyInfos != null && propertydes != null)
        //            {
        //                propertydes.SetValue(des, propertyInfos.GetValue(info, null), null);
        //            }
        //            //if (item.Name.Equals("CreateBy") || item.value == null)
        //            //{
        //            //    propertydes.SetValue(des, DataLocal.LoginedInfo.UserName, null);
        //            //}
        //            //if (item.Name.Equals("CreateDate") || item.value == null)
        //            //{
        //            //    propertydes.SetValue(des, DateTime.Now, null);
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SetErrored(new ExceptionInfo(this.GetType().Name, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()));
        //    }

        //}
        #endregion

        //-------------------------------------------------
       
    }
}
