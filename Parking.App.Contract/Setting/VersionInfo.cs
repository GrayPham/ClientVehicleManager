using Connect.Common.Interface;
using System;


namespace Parking.App.Contract.Setting
{
    public partial class VersionInfo : IInfo<VersionInfo>
    {
        //------------------------------------------------------------- 

        #region Member 

        private object _id;
        public int VersionID { get; set; }
        public string VersionCode { get; set; }
        public string VersionName { get; set; }
        public bool IsUpdate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public bool IsNotSupport { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string DownloadUrl { get; set; }
        public bool IsMaxVersion { get; set; }
        public long FileSize { get; set; }
        public string UpdateName { get; set; }
        public string Password { get; set; }
        #endregion

        //-------------------------------------------------------------  

        #region Constructor 

        public VersionInfo()
        {

            VersionID = 0;
            VersionCode = "";
            VersionName = "";
            IsUpdate = false;
            UpdateDate = DateTime.Now;
            CreatedBy = "System";
            CreatedDate = DateTime.Now;
            Description = "";
            IsNotSupport = false;
            LastModifiedBy = "System";
            LastModified = DateTime.Now;
            DownloadUrl = "";
            IsMaxVersion = false;
            FileSize = 0;
            UpdateName = "";
            Password = "";
        }

        public void Copy(VersionInfo info)
        {
            VersionID = info.VersionID;
            VersionCode = info.VersionCode;
            VersionName = info.VersionName;
            IsUpdate = info.IsUpdate;
            UpdateDate = info.UpdateDate;
            CreatedBy = info.CreatedBy;
            CreatedDate = info.CreatedDate;
            Description = info.Description;
            IsNotSupport = info.IsNotSupport;
            LastModifiedBy = info.LastModifiedBy;
            LastModified = info.LastModified;
            DownloadUrl = info.DownloadUrl;
            IsMaxVersion = info.IsMaxVersion;
            FileSize = info.FileSize;
            UpdateName = info.UpdateName;
            Password = info.Password;
        }

        #endregion

        //-------------------------------------------------------------  

        #region Propertise 
        public object ValueID { get { return VersionID; } set { _id = value; } }
        public override string ToString() { return VersionID + " - " + VersionName ?? string.Empty; }
        public string SQLData() { return @"GetVersions"; }
        public string PrimaryKey() { return @"VersionID"; }

        #endregion

        //-------------------------------------------------------------  


    }
}
