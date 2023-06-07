
using Connect.Common.Contract;
using Connect.Common.Interface;
using System;

namespace Parking.Contract.Common
{
    public class tblDeviceRequestConnectInfo : InfoBase<tblDeviceRequestConnectInfo>, IInfo<tblDeviceRequestConnectInfo>
    {
        // **-----------------------------------------------------------

        #region Constructor
        public tblDeviceRequestConnectInfo() { }
        #endregion

        // **-----------------------------------------------------------

        #region Member
        private object _id;
        public int RequestID { get; set; }
        public int StoreDeviceNo { get; set; }
        public int StoreNo { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string DeviceKeyNo { get; set; }
        public string DevicePublicIP { get; set; }
        public string PhoneNum { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        #endregion

        // **-----------------------------------------------------------

        #region IInfo
        public string ObjectName() { return ""; }

        public object PKID() { return RequestID; }

        public object ValueID { get => RequestID; set => _id = value; }

        #endregion

        // **-----------------------------------------------------------

        #region Public method
        public void Set(tblDeviceRequestConnectInfo info)
        {
            Copy(info);
        }

        public void Copy(tblDeviceRequestConnectInfo info)
        {
            RequestID = info.RequestID;
            StoreDeviceNo = info.StoreDeviceNo;
            StoreNo = info.StoreNo;
            DeviceName = info.DeviceName;
            DeviceType = info.DeviceType;
            DeviceKeyNo = info.DeviceKeyNo;
            DevicePublicIP = info.DevicePublicIP;
            PhoneNum = info.PhoneNum;
            Status = info.Status;
            CreateDate = info.CreateDate;
            CreateBy = info.CreateBy;
            UpdateDate = info.UpdateDate;
            UpdateBy = info.UpdateBy;

        }
        #endregion

        // **-----------------------------------------------------------
    }
}
