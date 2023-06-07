using System;
using System.Collections.Generic;

namespace Connect.Common.Contract
{
    public partial class RequestInfo
    {
       
        public Guid KeyID { get; set; }
        public object ID { get; set; }
        public int Top { get; set; }
        public string FuncWhere { get; set; }
        public string TSQL { get; set; }
        public object Info { get; set; }
        public Type TypeInfo { get; set; }
        public object TypeCode { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public bool IsBranchID { get; set; }
        public int BranchID { get; set; }
        public object BranchCode { get; set; }
        public string Owner { get; set; }
        public List<SortDataInfo> Sortings { get; set; }
        public List<FilterDataInfo> Filtering { get; set; }
        public int Option { get; set; }
        //**----------------------------------------------------------------------------------------------------------
        public int PageSize { get; set; } = 100;
        public int PageIndex { get; set; } = 0;
        public string TSQLTotal { get; set; } = "";
        //**----------------------------------------------------------------------------------------------------------
        public string Columns { get; set; } = "[]";
        public dynamic Data { get; set; }
        public dynamic Details { get; set; }
        public object GroupCode { get; set; } = "";
        //**----------------------------------------------------------------------------------------------------------
        public string ModelName { get; set; }
        public int ClientServiceID { get; set; }
        public Guid AppKey { get; set; }
        public DateTime FromDate { get; set; } = DateTime.Now.AddDays(-1);
        public DateTime ToDate { get; set; } = DateTime.Now;
        //--------------------------------------------
        public SearchOrderInfo SearchOrderInfo { get; set; }
        public RequestInfo()
        {
            KeyID = Guid.NewGuid();
            ID = 0;
            Top = 9999;
            TSQL = "";
            Info = null;
            TypeInfo = null;
            //BranchID = SystemSetting.BranchSelectID;
            Data = null;
            AppKey = Guid.Empty;
            ClientServiceID = 0;
            ModelName = "";
            UserName = "";
            UserName = null;
        }
        public RequestInfo(object id, int top)
        {
            KeyID = Guid.NewGuid();
            ID = id;
            Top = top;
            TSQL = "";
            Info = null;
            TypeInfo = null;
            Data = null;
            AppKey = Guid.Empty;
            ClientServiceID = 0;
            ModelName = "";
            UserName = "";
            UserName = null;
        }
        public RequestInfo(PaginateInfo info)
        {
            PageSize = info.PageSize;
            PageIndex = info.PageIndex;
            TSQL = info.TSQL;
            TSQLTotal = info.TSQLTotal;
            Owner = info.Owner;
            UserName = info.UserName;
            FromDate = DateTime.Parse(info.FromDate);
            ToDate = DateTime.Parse(info.ToDate);
            GroupCode = info.Group;
            TSQL = info.SearchText;
        }

        public DateTime GetFromDate()
        {
            return DateTime.Parse(FromDate + " 00:00:00");
        }
        public DateTime GetToDate()
        {
            return DateTime.Parse(ToDate + " 00:00:00");
        }



    }
}
