using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.Common.Contract
{
    public class SessionInfo : IInfo<SessionInfo>
    {
        public bool Succeed { get; set; }
        public int FailedNumber { get; set; }
        public string FailedReason { get; set; }
        public string Login_Username { get; set; }
        public string Login_Password { get; set; }
        public int Login_LoginID { get; set; }
        public int Login_AgencyID { get; set; }
        //public UserTypes User_Type { get; set; }
        //public AccountInfo Logininfo { get; set; }
        //public List<GroupSecurityDetailInfo> SecurityDetails { get; set; }
        //public LoginHistoryInfo LoginSession { get; set; }
        //public EmployeeInfo Employee { get; set; }
        public int NumberOfLogins { get; set; }
        public event GeneralEventHandler<SessionInfo> SessionInfoChanged;
        public string VersionName { get; set; }
        public string WindowUserName { get; set; }
        public int BranchID { get; set; }
        public string Owner { get; set; }
        public string BranchCode { get; set; }

        public SessionInfo()
        {
            ReSet();
        }
        public void Set(SessionInfo info)
        {
            Copy(info);
            if (SessionInfoChanged != null) SessionInfoChanged(this, new EventArgs<SessionInfo>(info));
        }
        public void Copy(SessionInfo info)
        {
            Succeed = info.Succeed;
            BranchID = info.BranchID;
            FailedNumber = info.FailedNumber;
            FailedReason = info.FailedReason;
            Login_Username = info.Login_Username;
            Login_Password = info.Login_Password;
            Login_LoginID = info.Login_LoginID;
            Login_AgencyID = info.Login_AgencyID;
            //User_Type = info.User_Type;
            //SecurityDetails = info.SecurityDetails;
            //LoginSession = info.LoginSession;
            //Employee = info.Employee;
            //Logininfo = info.Logininfo;
            //Branch = info.Branch;
            NumberOfLogins = info.NumberOfLogins;
            
            VersionName = info.VersionName;
            Owner = info.Owner;
    
        }
        public void ReSet()
        {
            Succeed = false;
            FailedNumber = 0;
            FailedReason = "";
            Login_Username = "";
            Login_Password = "";
            Login_LoginID = 0;
            Login_AgencyID = 0;
            //SecurityDetails = new List<GroupSecurityDetailInfo>();
            //LoginSession = null;
            //Employee = null;
            //BranchID = SystemSetting.BranchSelectID;
            NumberOfLogins = 0;
           
            Owner = "";
            //  User_Type = UserTypes.User;
        }
        public object ValueID
        {
            get
            {
                return Login_LoginID;
            }
            set
            {

            }
        }
        public string SQLData()
        {
            return @"";
        }
        public string PrimaryKey()
        {
            return "";
        }
        
    }
}
