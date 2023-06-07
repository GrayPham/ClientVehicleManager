using Connect.Common;
using Connect.Common.Common;
using System;

namespace Connect.Common.Contract
{
    public partial class SearchOrderInfo
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public EFormBy By { get; set; }
        public int Status { get; set; }
        public object StatusItem { get; set; }
        public bool IsBranch { get; set; }
        public SearchOrderInfo()
        {
            FromDate = DateTime.Now.AddDays(-30);
            ToDate = DateTime.Now;
            By = EFormBy.Me;
            IsBranch = false;
            Status = -1;
            StatusItem = null;
        }
        public void Copy(SearchOrderInfo info)
        {
            FromDate = info.FromDate;
            ToDate = info.ToDate;
            By = info.By;
            Status = info.Status;
            IsBranch = info.IsBranch;
            StatusItem = info.StatusItem;
        }
    }
}
