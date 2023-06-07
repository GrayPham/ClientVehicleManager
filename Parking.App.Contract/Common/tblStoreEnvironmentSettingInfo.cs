using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public class tblStoreEnvironmentSettingInfo : IInfo<tblStoreEnvironmentSettingInfo>
    {
        //-------------------------------------------------------------

        private object _id;

        public int EnvironmentSettingNo { get; set; }
        public int StoreNo { get; set; }
        public bool CertifCriteria { get; set; }
        public bool PhoneInput { get; set; }
        public int SimilarityRateApproval { get; set; }
        public bool AuthAfterCompleted { get; set; }
        public bool? AuthAfterCardId { get; set; }
        public bool? EId { get; set; }
        public bool? UseScanner { get; set; }
        public bool? UseCamera { get; set; }


        //-------------------------------------------------------------

        public tblStoreEnvironmentSettingInfo()
        {
            EnvironmentSettingNo = 0;
            StoreNo = 0;
            CertifCriteria = false;
            PhoneInput = false;
            SimilarityRateApproval = 0;
            AuthAfterCompleted = false;
            AuthAfterCardId = false;
            EId = false;
            UseScanner = false;
            UseCamera = false;
        }

        public void Copy(tblStoreEnvironmentSettingInfo info)
        {
            EnvironmentSettingNo = info.EnvironmentSettingNo;
            StoreNo = info.StoreNo;
            CertifCriteria = info.CertifCriteria;
            PhoneInput = info.PhoneInput;
            SimilarityRateApproval = info.SimilarityRateApproval;
            AuthAfterCompleted = info.AuthAfterCompleted;
            AuthAfterCardId = info.AuthAfterCardId;
            EId = false;
            UseScanner = false;
            UseCamera = false;
        }


        //-------------------------------------------------------------  

        #region Propertise 
        public object ValueID { get { return EnvironmentSettingNo; } set { _id = value; } }
        public override string ToString() { return EnvironmentSettingNo.ToString(); }
        public string SQLData() { return @""; }
        public string PrimaryKey() { return @" EnvironmentSettingNo"; }

        #endregion

        //-------------------------------------------------------------  

    }
}
