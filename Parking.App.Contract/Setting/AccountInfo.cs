using Connect.Common.Interface;
using Server.Contract.Session;
using System;


namespace Parking.App.Contract.Setting
{
    public partial class AccountInfo : IInfo<AccountInfo>
    {
        //------------------------------------------------------------- 

        #region Member

        private object _id;
        public int UserID { get; set; }
        public string UserNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? EmployeeID { get; set; }
        public int UserCategory { get; set; }
        public int LeverUser { get; set; }
        public string EmailRegistration { get; set; }
        public string Remark { get; set; }
        public bool IsActivity { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsAccept { get; set; }
        public bool IsConfirm { get; set; }
        public DateTime? LastModified { get; set; }
        public string AcceptedBy { get; set; }
        public string ConfirmBy { get; set; }
        public string LastModifiedBy { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string UserTypeName { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OwnerCode { get; set; }
        public string QueryKey { get; set; }
        public bool IsProtected { get; set; }
        #endregion

        //-------------------------------------------------------------  

        #region Constructor

        public AccountInfo()
        {

            UserID = 0;
            UserNumber = "";
            UserName = "";
            Password = "";
            EmployeeID = 0;
            UserCategory = 0;
            LeverUser = 0;
            EmailRegistration = "";
            Remark = "";
            IsActivity = true;
            CreatedBy = SessionDatas.GetLoginUser();
            CreatedDate = DateTime.Now;
            IsAccept = false;
            IsConfirm = false;
            LastModified = DateTime.Now;
            AcceptedBy = "";
            ConfirmBy = "";
            LastModifiedBy = SessionDatas.GetLoginUser();
            EmployeeCode = "";
            FirstName = "";
            LastName = "";
            Gender = "";
            UserTypeName = "";
            Description = "";
            BeginDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(1);
            OwnerCode = "";
            QueryKey = "";
            IsProtected = false;
        }

        public void Copy(AccountInfo info)
        {
            UserID = info.UserID;
            UserNumber = info.UserNumber;
            UserName = info.UserName;
            Password = info.Password;
            EmployeeID = info.EmployeeID;
            UserCategory = info.UserCategory;
            LeverUser = info.LeverUser;
            EmailRegistration = info.EmailRegistration;
            Remark = info.Remark;
            IsActivity = info.IsActivity;
            CreatedBy = info.CreatedBy;
            CreatedDate = info.CreatedDate;
            IsAccept = info.IsAccept;
            IsConfirm = info.IsConfirm;
            LastModified = info.LastModified;
            AcceptedBy = info.AcceptedBy;
            ConfirmBy = info.ConfirmBy;
            LastModifiedBy = info.LastModifiedBy;
            EmployeeCode = info.EmployeeCode;
            FirstName = info.FirstName;
            LastName = info.LastName;
            Gender = info.Gender;
            UserTypeName = info.UserTypeName;
            Description = info.Description;
            BeginDate = info.BeginDate;
            EndDate = info.EndDate;
            OwnerCode = info.OwnerCode;
            QueryKey = info.QueryKey;
            IsProtected = info.IsProtected;
        }

        #endregion

        //-------------------------------------------------------------  

        #region Propertise
        public object ID { get { return UserID; } set { _id = value; } }

        public object ValueID { get => UserID; set => _id = value; }

        public override string ToString() { return UserID + " - " + UserName ?? string.Empty; }
        public string SQLData()
        {
            return @"GetAccounts";
        }
        public string PrimaryKey()
        {
            return @"UserID";
        }
        #endregion

        //-------------------------------------------------------------  

    }
}
