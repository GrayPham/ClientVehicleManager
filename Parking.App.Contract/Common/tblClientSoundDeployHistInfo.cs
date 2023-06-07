using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public class tblClientSoundDeployHistInfo : IInfo<tblClientSoundDeployHistInfo>
    {
        //-------------------------------------------------------------

        private object _id;

        public int SoundDeplyHistNo { get; set; }

        public int TargetStoreNo { get; set; }


        public int TargetDeviceNo { get; set; }

        public int SoundNo { get; set; }

        public DateTime DeployTime { get; set; }

        public int TargetNo { get; set; }
        public bool DeployResult { get; set; }

        //-------------------------------------------------------------

        public tblClientSoundDeployHistInfo()
        {
            SoundDeplyHistNo = 0;
            TargetStoreNo = 0;
            TargetDeviceNo = 0;
            SoundNo = 0;
            DeployTime = DateTime.Now;
            TargetNo = 0;
            DeployResult = false;
        }

        public void Copy(tblClientSoundDeployHistInfo info)
        {
            SoundDeplyHistNo = info.SoundDeplyHistNo;
            TargetStoreNo = info.TargetStoreNo;
            TargetDeviceNo = info.TargetDeviceNo;
            SoundNo = info.SoundNo;
            DeployTime = info.DeployTime;
            TargetNo = info.TargetNo;
            DeployResult = info.DeployResult;
        }

        //-------------------------------------------------------------  

        #region Propertise 
        public object ValueID { get { return SoundDeplyHistNo; } set { _id = value; } }
        public override string ToString() { return SoundDeplyHistNo.ToString(); }
        public string SQLData() { return @""; }
        public string PrimaryKey() { return @" SoundDeplyHistNo"; }

        #endregion

        //-------------------------------------------------------------  

    }
}
