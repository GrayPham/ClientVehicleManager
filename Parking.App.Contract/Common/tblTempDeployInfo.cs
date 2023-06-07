using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public class tblTempDeployInfo : IInfo<tblTempDeployInfo>
    {
        //-------------------------------------------------------------

        private object _id;

        public int DeployNo { get; set; }
        public int SoundNo { get; set; }
        public int StoreNo { get; set; }
        public int DeviceNo { get; set; }
        public byte[] Source { get; set; }

        //-------------------------------------------------------------

        public tblTempDeployInfo()
        {
            DeployNo = 0;
            SoundNo = 0;
            StoreNo = 0;
            DeviceNo = 0;
            Source = new byte[0];
        }

        public void Copy(tblTempDeployInfo info)
        {
            DeployNo = info.DeployNo;
            SoundNo = info.SoundNo;
            StoreNo = info.StoreNo;
            DeviceNo = info.DeviceNo;
            Source = info.Source;
        }


        //-------------------------------------------------------------  

        #region Propertise 
        public object ValueID { get { return DeployNo; } set { _id = value; } }
        public override string ToString() { return DeployNo.ToString(); }
        public string SQLData() { return @""; }
        public string PrimaryKey() { return @" DeployNo"; }

        #endregion

        //-------------------------------------------------------------  

    }
}
