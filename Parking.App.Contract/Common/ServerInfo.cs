using Connect.Common.Contract;
using Connect.Common.Interface;
using Server.Contract.Session;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public partial class ServerInfo : InfoBase<ServerInfo>, IInfo<ServerInfo>
    {
        //------------------------------------------------------------- 

        #region Member 

        private object _id;
        public int ServerID { get; set; }
        public string ServerShortName { get; set; }
        public string ServerName { get; set; }
        public string IPServer { get; set; }
        public int? Port { get; set; }
        public string Remark { get; set; }
        public bool IsActivity { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsProtected { get; set; }
        public bool IsAlow { get; set; }

        #endregion

        //-------------------------------------------------------------  

        #region Constructor 

        public ServerInfo()
        {

            ServerID = 0;
            ServerShortName = "";
            ServerName = "";
            IPServer = "";
            Port = 0;
            Remark = "";
            IsActivity = true;
            CreatedBy = SessionDatas.GetLoginUser();
            CreatedDate = DateTime.Now;
            LastModifiedBy = SessionDatas.GetLoginUser();
            LastModified = DateTime.Now;
            IsProtected = false;
            IsAlow = true;
        }

        public void Copy(ServerInfo info)
        {
            ServerID = info.ServerID;
            ServerShortName = info.ServerShortName;
            ServerName = info.ServerName;
            IPServer = info.IPServer;
            Port = info.Port;
            Remark = info.Remark;
            IsActivity = info.IsActivity;
            CreatedBy = info.CreatedBy;
            CreatedDate = info.CreatedDate;
            LastModifiedBy = info.LastModifiedBy;
            LastModified = info.LastModified;
            IsProtected = info.IsProtected;
            IsAlow = info.IsAlow;
        }

        #endregion

        //-------------------------------------------------------------  

        #region Propertise 
        public object ValueID { get { return ServerID; } set { _id = value; } }
        public override string ToString() { return ServerID + " - " + ServerName ?? string.Empty; }
        public string SQLData() { return @"GetServerInformations"; }
        public string PrimaryKey() { return @"ServerID"; }

        #endregion

        //-------------------------------------------------------------  


    }
}
