using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Contract.Common
{
    public class tblCommonInfo : IInfo<tblCommonInfo>
    {
        //-------------------------------------------------------------

        private object _id;

        public string CommonCode { get; set; }

        public string CommonName1 { get; set; }

        public string CommonName2 { get; set; }

        public string Description { get; set; }

        public bool SystemCode { get; set; }


        //-------------------------------------------------------------

        public tblCommonInfo()
        {
            CommonCode = "";
            CommonName1 = "";
            CommonName2 = "";
            Description = "";
            SystemCode = false;

        }

        public void Copy(tblCommonInfo info)
        {
            CommonCode = info.CommonCode;
            CommonName1 = info.CommonName1;
            CommonName2 = info.CommonName2;
            Description = info.Description;
            SystemCode = info.SystemCode;
        }

        //-------------------------------------------------------------  
        public object ValueID { get { return CommonCode; } set { _id = value; } }
        public override string ToString() { return CommonCode.ToString(); }
        public string SQLData() { return @""; }
        public string PrimaryKey() { return @" CommonCode"; }
        //-------------------------------------------------------------  
    }
}
