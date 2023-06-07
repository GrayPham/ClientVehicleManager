using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public partial class tblUserInfo : IInfo<tblUserInfo>
    {
        private object _id;
        public string UserID { get; set; }
        public string UserType { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public bool? Gender { get; set; }
        public bool? ApproveReject { get; set; }

        public string UserStatus { get; set; }

        public bool isRemoveTempUser { get; set; }


        public DateTime RegistDate { get; set; }

        public string Desc { get; set; }
        public List<string> ListUserId { get; set; }

        public bool UseYN { get; set; }
        public string AuthMethod { get; set; }

        public string StoreList { get;set;}

        //-------------------------------------------------------------
        public tblUserMgtStoreInfo TblUserMgtStoreInfo { get; set; }
        public tblUserPhotoInfo TblUserPhotoInfo { get; set; }
        public tblStoreUseHistoryInfo TblStoreUseHistoryInfo { get; set; }
        public int? LastSimilarityRate { get; set; }
        public string LoginIP { get; set; }
        //-------------------------------------------------------------

        public tblUserInfo()
        {
            UserID = String.Empty;
            UserType = String.Empty;
            Password = String.Empty;
            UserName = String.Empty;
            PhoneNumber = String.Empty;
            Birthday = DateTime.Now;
            Email = String.Empty;
            Gender = false;
            ApproveReject = false;
            UserStatus = String.Empty;
            RegistDate = DateTime.Now;
            Desc = String.Empty;
            UseYN = false;
            LoginIP = String.Empty;
            AuthMethod=String.Empty;
        }

        public void Copy(tblUserInfo info)
        {
            UserID = info.UserID;
            UserType = info.UserType;
            Password = info.Password;
            UserName = info.UserName;
            PhoneNumber = info.PhoneNumber;
            Birthday = info.Birthday;
            Email = info.Email;
            Gender = info.Gender;
            ApproveReject = info.ApproveReject;
            UserStatus = info.UserStatus;
            RegistDate = info.RegistDate;
            Desc = info.Desc;
            UseYN = info.UseYN;
            LoginIP = info.LoginIP;
            AuthMethod = info.AuthMethod;
        }

        //-------------------------------------------------------------  

        public object ValueID { get { return UserID; } set { _id = value; } }
        public override string ToString() { return UserID; }
        public string SQLData() { return @""; }
        public string PrimaryKey() { return @" UserId"; }
    }
}

