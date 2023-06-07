using Connect.Common;
using Connect.Common.Common;
using Connect.Common.Helper;
using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.RemoteDataProvider.Client
{
    public class ClientSoftwareInfo : IInfo<ClientSoftwareInfo>
    {
        public String LicenseKey { get; set; }
        public String SerialNumber { get; set; }
        public String LicenseSoftware { get; set; }
        public String Mac { get; set; }
        public object ValueID { get; set; }
        public int VersionKey { get; set; }
        public DateTime ClientTime { get; set; }
        public string AccessKey { get; set; }
        public string HardwareID { get; set; }
        public EPlatform ePlatform { get; set; }
        public EClientType eClientType { get; set; }
        public string CPUID { get; set; }
        public ClientSoftwareInfo()
        {
            ValueID = 0;
            ClientTime = DateTime.Now;
        }

        public ClientSoftwareInfo(ClientSoftwareInfo info)
        {
            ValueID = info.ValueID;
            Copy(info);
        }

        public void Copy(ClientSoftwareInfo info)
        {
        }
        public void GetAccessKey(string key)
        {
            AccessKey = HexHelper.CalculateMd5Hash(key + Mac + LicenseKey + SerialNumber);
        }
        public string SQLData()
        {
            return "";
        }

        public string PrimaryKey()
        {
            return "";
        }
    }
}
