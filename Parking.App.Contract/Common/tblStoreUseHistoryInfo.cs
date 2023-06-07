using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public class tblStoreUseHistoryInfo : IInfo<tblStoreUseHistoryInfo>
    {
        private object _id;

        public int StoreUserId { get; set; }
        public int StoreNo { get; set; }
        public string UserID { get; set; }
        public DateTime UseDate { get; set; }



        public tblStoreUseHistoryInfo()
        {
            StoreNo = 0;
            UserID = String.Empty;
            UseDate = DateTime.Now;
        }

        public void Copy(tblStoreUseHistoryInfo info)
        {
            StoreUserId = info.StoreUserId;
            StoreNo = info.StoreNo;
            UserID = info.UserID;
            UseDate = info.UseDate;
        }


        public object ValueID { get { return StoreUserId; } set { _id = value; } }
        public override string ToString() { return StoreUserId.ToString(); }
        public string SQLData() { return @""; }
        public string PrimaryKey() { return @" StoreUserId"; }
    }
}
