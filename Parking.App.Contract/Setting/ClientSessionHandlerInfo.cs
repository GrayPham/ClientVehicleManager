using Connect.Common.Common;
using Connect.Common.Interface;
using System;

namespace Parking.App.Contract.Setting
{
    public class ClientSessionHandlerInfo : IInfo<ClientSessionHandlerInfo>
    {
        private object _id;
        public int ClientID { get; set; }
        public string LicenseKey { get; set; }
        public string LicenseSoftware { get; set; }
        public string SerialNumber { get; set; }
        public string Mac { get; set; }
        public string IpEndpoint { get; set; }
        public int VersionID { get; set; }
        public DateTime ClientTime { get; set; }
        public string AccessKey { get; set; }
        public string HardwareID { get; set; }
        public EPlatform ePlatform { get; set; }
        public EClientType eClientType { get; set; }
        public string CPUID { get; set; }
        //---------------------------------------------------------------------------------
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string OwnerCode { get; set; }
        //public UserTypes User_Type { get; set; }
        //---------------------------------------------------------------------------------
        public void Copy(ClientSessionHandlerInfo info)
        {
            ClientID = info.ClientID;
            LicenseKey = info.LicenseKey;
            LicenseSoftware = info.LicenseSoftware;
            SerialNumber = info.SerialNumber;
            Mac = info.Mac;
            IpEndpoint = info.IpEndpoint;
            VersionID = info.VersionID;
            ClientTime = info.ClientTime;
            AccessKey = info.AccessKey;
            HardwareID = info.HardwareID;
            ePlatform = info.ePlatform;
            eClientType = info.eClientType;
            CPUID = info.CPUID;

            UserID = info.UserID;
            UserName = info.UserName;
            OwnerCode = info.OwnerCode;
            //User_Type = UserTypes.User;
        }
        public object ID { get { return ClientID; } set { _id = value; } }

        public object ValueID { get => ClientID; set => _id = value; }

        public override string ToString() { return ClientID + " - " + LicenseSoftware ?? string.Empty; }
        public string PrimaryKey()
        {
            return "";
        }

        public string SQLData()
        {
            return "";
        }
    }
}
