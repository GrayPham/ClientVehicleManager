using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public partial class tblAdMgtInfo : IInfo<tblAdMgtInfo>
    {
        //-------------------------------------------------------------

        #region Member 
        private object _id;
        public int? AdNo { get; set; }
        public string AdType { get; set; }
        public string AdName { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public DateTime DayStartTime { get; set; }
        public DateTime DayEndTime { get; set; }
        public bool AdStatus { get; set; }
        public DateTime RegistDate { get; set; }
        public string ResitUser { get; set; }
        public bool AdLocation { get; set; }
        public string AttachFilePath { get; set; }
        public int? Version { get; set; }
        public string LocalPath { get; set; }

        public bool Check { get; set; }
        #endregion

        //-------------------------------------------------------------  

        #region Constructor 

        public tblAdMgtInfo()
        {
            AdNo = 0;
            AdType = "";
            AdName = "";
            PeriodStartDate = DateTime.Now;
            PeriodEndDate = DateTime.Now;
            DayStartTime = DateTime.Now;
            DayEndTime = DateTime.Now;
            AdStatus = false;
            RegistDate = DateTime.Now;
            ResitUser = ""; 
            AdLocation = false;
            AttachFilePath = "";
            Version = 0;
            LocalPath = "";
            Check = false;
        }

        public void Copy(tblAdMgtInfo info)
        {
            AdNo = info.AdNo;
            AdType = info.AdType;
            AdName = info.AdName;
            PeriodStartDate = info.PeriodStartDate;
            PeriodEndDate = info.PeriodEndDate;
            DayStartTime = info.DayStartTime;
            DayEndTime = info.DayEndTime;
            AdStatus = info.AdStatus;
            RegistDate = info.RegistDate;
            ResitUser = info.ResitUser;
            AdLocation = info.AdLocation;
            AttachFilePath = info.AttachFilePath;
            Version = info.Version;
            LocalPath = info.LocalPath;
            Check = info.Check;
        }

        #endregion

        //-------------------------------------------------------------  

        #region Propertise 
        public object ValueID { get { return AdNo; } set { _id = value; } }
        public override string ToString() { return AdNo.ToString(); }
        public string SQLData() { return @""; }
        public string PrimaryKey() { return @"AdNo"; }

        #endregion

        //-------------------------------------------------------------  
    }
}
