using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public class tblStoreDeviceInfo : IInfo<tblStoreDeviceInfo>
    {
        private object _id;

        public int StoreDeviceNo { get; set; }

        public int StoreNo { get; set; }
        public string DeviceName { get; set; }

        public string DeviceType { get; set; }
        public string DeviceKeyNo { get; set; }
        public string DevicePublicIP { get; set; }
        public int DeviceUsePort { get; set; }
        public bool DeviceStatus { get; set; }

        public string RDPPath { get; set; }
        public string RegistUserId { get; set; }
        public DateTime RegistDate { get; set; }
        public string ListDeviceKeyNo { get; set; }

        public object ValueID { get { return StoreDeviceNo; } set { _id = value; } }

        public tblStoreDeviceInfo()
        {
            StoreDeviceNo = 0;
            StoreNo = 0;
            DeviceName = "";
            DeviceType = "";
            DeviceKeyNo = "";
            DevicePublicIP = "";
            RDPPath = "";
            RegistUserId = "";
            DeviceUsePort = 0;
            DeviceStatus = false;
            RegistDate = DateTime.Now;
            ListDeviceKeyNo = "";
        }

        public void Copy(tblStoreDeviceInfo info)
        {
            StoreDeviceNo = info.StoreDeviceNo;
            StoreNo = info.StoreNo;
            DeviceName = info.DeviceName;
            DeviceType = info.DeviceType;
            DeviceKeyNo = info.DeviceKeyNo;
            DevicePublicIP = info.DevicePublicIP;
            RDPPath = info.RDPPath;
            RegistUserId = info.RegistUserId;
            DeviceUsePort = info.DeviceUsePort;
            DeviceStatus = info.DeviceStatus;
            RegistDate = DateTime.Now;
            ListDeviceKeyNo = info.ListDeviceKeyNo;
        }

        public string PrimaryKey()
        {
            return @"StoreDeviceNo";
        }

        public override string ToString() { return StoreDeviceNo.ToString(); }

        public string SQLData()
        {
            return @"";
        }
    
    }
}
