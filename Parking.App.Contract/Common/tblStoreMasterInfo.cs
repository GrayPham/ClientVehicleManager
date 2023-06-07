using Connect.Common.Contract;
using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public class tblStoreMasterInfo : InfoBase<tblStoreMasterInfo>, IInfo<tblStoreMasterInfo>
    {
        // **-----------------------------------------------------------

        #region Constructor

        #endregion

        // **-----------------------------------------------------------

        #region Member
        private object _id;
        public int StoreNo { get; set; }
        public string Location { get; set; }
        public string StoreName { get; set; }
        public string BizNumber { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string BizPhoneNumber { get; set; }
        public string Memo { get; set; }
        public DateTime RegistDate { get; set; }
        public DateTime? MonitoringStartime { get; set; }
        public DateTime? MonitoringEndtime { get; set; }
        public int MinorAge { get; set; }
        public bool OwlClientHomeSound { get; set; }
        public DateTime ClientHomeSound { get; set; }
        public bool Status { get; set; }

        public tblStoreMasterInfo()
        {
            StoreNo = 0;
            Location = "";
            StoreName = "";
            BizNumber = "";
            ZipCode = "";
            Address1 = "";
            Address2 = "";
            BizPhoneNumber = "";
            Memo = "";
            RegistDate = DateTime.Now;
            MonitoringStartime = DateTime.Now;
            MonitoringEndtime = DateTime.Now;
            MinorAge = 0;
            OwlClientHomeSound = false;
            ClientHomeSound = DateTime.Now;
            Status = false;
        }

        public string PrimaryKey()
        {
            return @"StoreNo";
        }

        public override string ToString() { return StoreNo.ToString(); }

        public string SQLData()
        {
            return @"";
        }

        #endregion

        // **-----------------------------------------------------------

        #region IInfo
        public object ValueID { get { return StoreNo; } set { _id = value; } }

        public string ObjectName() { return ""; }

        public object PKID() { return StoreNo; }


        #endregion

        // **-----------------------------------------------------------

        #region Public method
        public void Set(tblStoreMasterInfo info)
        {
            Copy(info);
        }

        public void Copy(tblStoreMasterInfo info)
        {
            StoreNo = info.StoreNo;
            Location = info.Location;
            StoreName = info.StoreName;
            BizNumber = info.BizNumber;
            ZipCode = info.ZipCode;
            Address1 = info.Address1;
            Address2 = info.Address2;
            BizPhoneNumber = info.BizPhoneNumber;
            Memo = info.Memo;
            RegistDate = info.RegistDate;
            MonitoringStartime = info.MonitoringStartime;
            MonitoringEndtime = info.MonitoringEndtime;
            MinorAge = info.MinorAge;
            OwlClientHomeSound = info.OwlClientHomeSound;
            ClientHomeSound = info.ClientHomeSound;
            Status = info.Status;
        }
        #endregion

        // **-----------------------------------------------------------

    }
}
