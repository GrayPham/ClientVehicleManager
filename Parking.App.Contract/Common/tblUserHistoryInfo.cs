using Connect.Common.Interface;
using System;
using static Parking.App.Common.Helper.SoundRegulations;

namespace Parking.App.Contract.Common
{
    public class tblUserHistoryInfo : IInfo<tblUserHistoryInfo>
    {
        //-------------------------------------------------------------

        private object _id;

        public int UserLoginNo { get; set; }

        public string UserID { get; set; }


        public string ApprovalType { get; set; }

        public int LastSimilarityRate { get; set; }

        public bool ApproveReject { get; set; }

        public DateTime LoginTime { get; set; }
        public string LoginIP { get; set; }
        public string TypeCode { get; set; }

        public byte[] FaceOkImage { get; set; }

        //-------------------------------------------------------------

        public tblUserHistoryInfo()
        {
            UserLoginNo = 0;
            UserID = "";
            ApprovalType = "";
            LastSimilarityRate = 0;
            ApproveReject = false;
            LoginTime = DateTime.Now;
            LoginIP = "";
            TypeCode = EHistoryType.FACEOK.ToString();
            FaceOkImage = new byte[0];

        }

        public void Copy(tblUserHistoryInfo info)
        {
            UserLoginNo = info.UserLoginNo;
            UserID = info.UserID;
            ApprovalType = info.ApprovalType;
            LastSimilarityRate = info.LastSimilarityRate;
            ApproveReject = info.ApproveReject;
            LoginTime = info.LoginTime;
            LoginIP = info.LoginIP;
            TypeCode = info.TypeCode;
            FaceOkImage=info.FaceOkImage;


        }

        //-------------------------------------------------------------  

        #region Propertise 
        public object ValueID { get { return UserLoginNo; } set { _id = value; } }
        public override string ToString() { return UserLoginNo.ToString(); }
        public string SQLData() { return @""; }
        public string PrimaryKey() { return @" UserLoginNo"; }

        #endregion

        //-------------------------------------------------------------  
       
    }
}
