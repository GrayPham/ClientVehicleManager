using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public  class tblUserMgtStoreInfo : IInfo<tblUserMgtStoreInfo>
    {
        private object _id;

        public string UserID { get; set; }
        public int StoreNo { get; set; }
        public string Memo { get; set; }
        public DateTime RegistDate { get; set; }



        public tblUserMgtStoreInfo()
        {
            UserID = String.Empty;
            StoreNo = 0;
            Memo = String.Empty;
            RegistDate = DateTime.Now;
        }

        public void Copy(tblUserMgtStoreInfo info)
        {
            UserID = info.UserID;
            StoreNo = info.StoreNo;
            Memo = info.Memo;
            RegistDate = info.RegistDate;
        }


        public object ValueID { get { return UserID; } set { _id = value; } }
        public override string ToString() { return UserID; }
        public string SQLData() { return @""; }
        public string PrimaryKey() { return @" UserId"; }
    }  
    
}
