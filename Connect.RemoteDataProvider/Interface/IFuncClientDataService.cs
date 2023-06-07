using Connect.Common.Contract;
using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.RemoteDataProvider.Interface
{
    public interface IFuncClientDataService<TInfo> where TInfo : IInfo<TInfo>, new()
    {

        //-------------------------------------------------------

        #region Search method

        ResultInfo TryGet(object id);
        Task<ResultInfo> TryGetAsync(object id);
        ResultInfo TryGet(Func<TInfo, bool> predicate);
        Task<ResultInfo> TryGetAsync(Func<TInfo, bool> predicate);
        ResultInfo GetDataAll(int top);
        Task<ResultInfo> GetDataAllAsync(int top);
        ResultInfo GetDataIsActivity(int top);
        Task<ResultInfo> GetDataIsActivityAsync(int top);
        Task<ResultInfo> GetDataIsActivityAsync(int top, int branchID);
        ResultInfo GetDataIsActivityByGroup(object id, int top);
        Task<ResultInfo> GetDataIsActivityByGroupAsync(object id, int top);
        ResultInfo GetDataIsActivityByGroupCode(object id, int top);
        Task<ResultInfo> GetDataIsActivityByGroupCodeAsync(object id, int top);
        ResultInfo GetDataNotIsActivity(int top);
        Task<ResultInfo> GetDataNotIsActivityAsync(int top);

        ResultInfo GetDataBBAll<TViewEntity>(SynchronizationContext currentContext, int top) where TViewEntity : class, IViewEntity<TInfo>, new();
        Task<ResultInfo> GetDataBBAllAsync<TViewEntity>(SynchronizationContext currentContext, int top) where TViewEntity : class, IViewEntity<TInfo>, new();

        ResultInfo GetDataBBIsActivity<TViewEntity>(SynchronizationContext currentContext, int top) where TViewEntity : class, IViewEntity<TInfo>, new();
        Task<ResultInfo> GetDataBBIsActivityAsync<TViewEntity>(SynchronizationContext currentContext, int top) where TViewEntity : class, IViewEntity<TInfo>, new();

        ResultInfo GetDataBBGroupBy<TViewEntity>(SynchronizationContext currentContext, object group, int top) where TViewEntity : class, IViewEntity<TInfo>, new();
        Task<ResultInfo> GetDataBBGroupByAsync<TViewEntity>(SynchronizationContext currentContext, object group, int top) where TViewEntity : class, IViewEntity<TInfo>, new();
        #endregion

        //-------------------------------------------------------

        #region Exec
        ResultInfo ExecuteQueryS2Info(string tsql);
        Task<ResultInfo> ExecuteQueryS2InfoAsync(string tSql);
        //-------------------------------------------------------
        ResultInfo ExecuteQueryS2DataTable(string tsql);
        Task<ResultInfo> ExecuteQueryS2DataTableAsync(string tsql);

        //-------------------------------------------------------
        ResultInfo ExecuteQueryS2Info<T>(string tsql) where T : class, IInfo<T>, new();
        Task<ResultInfo> ExecuteQueryS2InfoAsync<T>(string tSql) where T : class, IInfo<T>, new();
        #endregion
        Task<ResultInfo> ExecuteSearchFromToAsync(SearchOrderInfo info);
        ResultInfo ExecuteSearchFromTo(SearchOrderInfo info);
        Task<ResultInfo> ExcPaginateAsync(PaginateInfo info);
        ResultInfo ExcPaginate(PaginateInfo info);
        //-------------------------------------------------------
        Task<ResultInfo> ExecuteCommandAsync(string tsql);
        ResultInfo ExecuteCommand(string tsql);
        //-------------------------------------------------------
        ResultInfo RequestGetDataByOption(RequestInfo request);
        Task<ResultInfo> RequestGetDataByOptionAsync(RequestInfo request);
        //-------------------------------------------------------

        #region Condition
        [Description("Gets the value associated with the specified func.")]
        ResultInfo TryGetBySCondition(string condition);
        [Description("Gets the value associated with the specified func.")]
        Task<ResultInfo> TryGetBySConditionAsync(string condition);
        //-------------------------------------------------------
        [Description("Check Exists the value associated with the specified Condition.")]
        ResultInfo CheckExistsBySCondition(string condition);
        [Description("Check Exists the value associated with the specified Condition.")]
        Task<ResultInfo> CheckExistsBySConditionAsync(string condition);
        //-------------------------------------------------------
        [Description("Gets data the value associated with the specified Condition.")]
        ResultInfo GetDataBySCondition(string condition);
        [Description("Gets data the value associated with the specified Condition.")]
        Task<ResultInfo> GetDataBySConditionAsync(string condition);
        #endregion

        //-------------------------------------------------------

        #region Get data by where
        ResultInfo GetByWhere(string sqlWhere);
        Task<ResultInfo> GetByWhereAsync(string sqlWhere);
        //-------------------------------------------------------
        Task<ResultInfo> RequestGetByWhereAsync(RequestInfo request);
        ResultInfo RequestGetByWhere(RequestInfo request);
        #endregion

        //-------------------------------------------------------

        #region Public Method

        void Copy<T>(TInfo info, T des) where T : class, new();

        #endregion

        //-------------------------------------------------------
        //-------------------------------------------------------
    }
}
