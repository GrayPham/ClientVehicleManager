using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public class tblAdStoreMgtInfo : IInfo<tblAdStoreMgtInfo>
    {
        //-------------------------------------------------------------

        private object _id;

        public int? AdNo { get; set; }
        public int StoreNo { get; set; }



        public tblAdStoreMgtInfo()
        {
            AdNo = 0;
            StoreNo = 0;

        }

        public void Copy(tblAdStoreMgtInfo info)
        {
            AdNo = info.AdNo;
            StoreNo = info.StoreNo;
        }

        //-------------------------------------------------------------  
        public object ValueID { get { return AdNo; } set { _id = value; } }

        public override string ToString() { return AdNo.ToString(); }

        public string PrimaryKey()
        {
            return @"AdNo";
        }

        public string SQLData()
        {
            return @"";
        }
        //-------------------------------------------------------------  
    }
}
