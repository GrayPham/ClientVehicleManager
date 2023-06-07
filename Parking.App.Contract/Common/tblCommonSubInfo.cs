using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public class tblCommonSubInfo : IInfo<tblCommonSubInfo>
    {
        //-------------------------------------------------------------

        private object _id;

        public string CommonSubCode { get; set; }

        public string CommonCode { get; set; }


        public string CommonSubName1 { get; set; }

        public string CommonSubName2 { get; set; }

        public string Description { get; set; }

        public bool SystemCode { get; set; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        //-------------------------------------------------------------

        public tblCommonSubInfo()
        {
            CommonSubCode = "";
            CommonCode = "";
            CommonSubName1 = "";
            CommonSubName2 = "";
            Description = "";
            SystemCode = false;
            Image1 = "";
            Image2 = "";
            Image3 = "";
        }

        public void Copy(tblCommonSubInfo info)
        {
            CommonSubCode = info.CommonSubCode;
            CommonCode = info.CommonCode;
            CommonSubName1 = info.CommonSubName1;
            CommonSubName2 = info.CommonSubName2;
            Description = info.Description;
            SystemCode = info.SystemCode;
            Image1 = info.Image1;
            Image2 = info.Image2;
            Image3 = info.Image3;
        }

        //-------------------------------------------------------------  

        #region Propertise 
        public object ValueID { get { return CommonSubCode; } set { _id = value; } }
        public override string ToString() { return CommonSubCode.ToString(); }
        public string SQLData() { return @""; }
        public string PrimaryKey() { return @" CommonSubCode"; }

        #endregion

        //-------------------------------------------------------------  

    }
}
