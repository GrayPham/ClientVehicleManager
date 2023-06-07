using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.Common.Helper
{
    public class SystemSetting
    {
        public static int TopDafault = 99999;
        public static int TopMax = 999999999;
        public static int RowTemplate_Height = 22;
        public static string FormatDateTime = @"dd/MM/yyyy";
        public static string FDate = @"dd/MM/yyyy";
        public static string FSQLDate = @"yyyy-MM-dd";

        public static int MaintenanceCycleDayDefault = 3;
        //public static int UserID = 0;
        public static int BranchSelectID = 0;
        public static object BranchCode;
        public static object Branch;
        public static int DefaultHandoverType = 6;

        public static int UserID = -1;
        public static string UserName = "";
        public static int BranchID = -1;
        public static string Owner = "";
    }
}
