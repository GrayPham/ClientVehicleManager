using Connect.Common.Interface;
using System;
namespace Parking.Contract.Common
{
    public partial class tblClientSoundMgtInfo : IInfo<tblClientSoundMgtInfo>
    {
        //------------------------------------------------------------- 

        #region Member 
        private object _id;
        public int SoundNo { get; set; }
        public string SoundName { get; set; }
        public string LocalFileLocation { get; set; }
        public string KioskFolderLocation { get; set; }
        public bool DeployStatus { get; set; }
        public DateTime RegistDate { get; set; }
        public string ResitUser { get; set; }
        public bool IsActivity { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Version { get; set; }

        public string SoundType { get; set; }

        #endregion

        //-------------------------------------------------------------  

        #region Constructor 

        public tblClientSoundMgtInfo()
        {
            SoundNo=0;
            SoundName="";
            LocalFileLocation="";
            KioskFolderLocation = "";
            DeployStatus=false;
            RegistDate=DateTime.Now;
            ResitUser="";
            IsActivity=false;
            CreatedDate=DateTime.Now;
            CreatedBy="";
            LastModifiedBy="";
            LastModified=DateTime.Now;
            Version = 0;
            SoundType = "";
        }

        public void Copy(tblClientSoundMgtInfo info)
        {
            SoundNo = info.SoundNo;
            SoundName = info.SoundName;
            LocalFileLocation = info.LocalFileLocation;
            KioskFolderLocation = info.KioskFolderLocation;
            DeployStatus = info.DeployStatus;
            RegistDate = info.RegistDate;
            ResitUser = info.ResitUser;
            IsActivity = info.IsActivity;
            CreatedDate = info.CreatedDate;
            CreatedBy = info.CreatedBy;
            LastModifiedBy = info.LastModifiedBy;
            LastModified = info.LastModified;
            Version = info.Version;
            SoundType = info.SoundType;
        }

        #endregion

        //-------------------------------------------------------------  

        #region Propertise 
        public object ValueID { get { return  SoundNo; } set { _id = value; } }
        public override string ToString() { return  SoundNo + " - " + SoundName ?? string.Empty; }
        public string SQLData() { return @""; }
        public string PrimaryKey() { return @"SoundNo"; }

        #endregion

        //-------------------------------------------------------------  
    }
}