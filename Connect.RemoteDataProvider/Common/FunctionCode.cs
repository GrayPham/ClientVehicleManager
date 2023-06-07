using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.RemoteDataProvider.Common
{
    public class FunctionCode
    {
        public const UInt16 SetNotify = 0x10A0;
        public const UInt16 SessionReady = 0x10A1;

        public const UInt16 RequestList = 0x1001;
        public const UInt16 ResponseList = 0x1011;

        public const UInt16 RequestLargeImportInfo = 0x1023;
        public const UInt16 ResponseLargeImportInfo = 0x1024;

        public const UInt16 RequestSynchronized = 0x1008;
        public const UInt16 ResponseSynchronized = 0x1018;

        public const UInt16 RequestUpdateCache = 0x1009;
        public const UInt16 ResponseUpdateCache = 0x1019;

        public const UInt16 RequestAdd = 0x1002;
        public const UInt16 RequestRemove = 0x1003;
        public const UInt16 RequestUpdate = 0x1004;
        public const UInt16 RequestAddList = 0x1005;
        public const UInt16 RequestRemoveList = 0x1006;
        public const UInt16 RequestUpdateList = 0x1007;
        public const UInt16 RequestInsertOrUpdate = 0x2003;
        public const UInt16 RequestInsertOrUpdateList = 0x2004;

        public const UInt16 RequestMDAdd = 0x1025;
        public const UInt16 RequestMDUpdate = 0x1026;
        public const UInt16 RequestMDIorU = 0x1027;

        public const UInt16 ResponseMDAdd = 0x2032;
        public const UInt16 ResponseMDUpdate = 0x2033;
        public const UInt16 ResponseMDIorU = 0x2034;

        public const UInt16 ResponseAdd = 0x1102;
        public const UInt16 ResponseRemove = 0x1103;
        public const UInt16 ResponseUpdate = 0x1104;
        public const UInt16 ResponseAddList = 0x1105;
        public const UInt16 ResponseRemoveList = 0x1106;
        public const UInt16 ResponseUpdateList = 0x1107;
        public const UInt16 ResponseInsertOrUpdate = 0x1108;
        public const UInt16 ResponseInsertOrUpdateList = 0x1109;

        public const UInt16 RequestListNotifyAdded = 0x2005;
        public const UInt16 RequestNotifyAdded = 0x2006;
        public const UInt16 RequestListNotifyUpdated = 0x2007;
        public const UInt16 RequestNotifyUpdated = 0x2008;
        public const UInt16 RequestListNotifyRemoved = 0x2009;
        public const UInt16 RequestNotifyRemoved = 0x2030;
        public const UInt16 RequestNotifyCustomized = 0x2031;

        public const UInt16 RequestCheckRemove = 0x1021;
        public const UInt16 RequestPortUpdate = 0x1022;

        public const UInt16 Added = 0x1012;
        public const UInt16 Updated = 0x1013;
        public const UInt16 Removed = 0x1014;
        public const UInt16 ListAdded = 0x1015;
        public const UInt16 ListUpdated = 0x1016;
        public const UInt16 ListRemoved = 0x1017;
        public const UInt16 Customized = 0x1020;

        public const UInt16 RequestFailed = 0x2000;

        public const UInt16 ResponseEdit = 0x2001;
        public const UInt16 ResponseFailed = 0x2002;

        public const UInt16 RequestTryGetID = 0x3001;
        public const UInt16 RequestTryGetFunc = 0x3002;
        public const UInt16 RequestGetByWhere = 0x3003;
        public const UInt16 RequestGetDataAll = 0x3004;
        public const UInt16 RequestGetDataIsActivity = 0x3005;
        public const UInt16 RequestGetDataIsActivityByGroup = 0x3006;
        public const UInt16 RequestGetDataNotIsActivity = 0x3007;
        public const UInt16 RequestGetDataIsActivityByBranch = 0x3008;
        public const UInt16 RequestGetDataIsActivityByGroupCode = 0x3009;

        //**----------------------------------------------------------------------
        public const UInt16 RequestGetDataByOption = 0xA001;
        public const UInt16 RequestCheckExistsBySCondition = 0xA002;
        public const UInt16 RequestGetDataBySCondition = 0xA003;
        public const UInt16 RequestTryGetBySCondition = 0xA004;
        //**----------------------------------------------------------------------
        public const UInt16 ResponseGetDataByOption = 0xB001;
        public const UInt16 ResponseCheckExistsBySCondition = 0xB002;
        public const UInt16 ResponseGetDataBySCondition = 0xB003;
        public const UInt16 ResponseTryGetBySCondition = 0xB004;
        //**----------------------------------------------------------------------
        public const UInt16 ResponseTryGetID = 0x3011;
        public const UInt16 ResponseTryGetFunc = 0x3012;
        public const UInt16 ResponseGetByWhere = 0x3013;
        public const UInt16 ResponseGetDataAll = 0x3014;
        public const UInt16 ResponseGetDataIsActivity = 0x3015;
        public const UInt16 ResponseGetDataIsActivityByGroup = 0x3016;
        public const UInt16 ResponseGetDataNotIsActivity = 0x3017;
        public const UInt16 ResponseGetDataIsActivityByBranch = 0x3018;
        public const UInt16 ResponseGetDataIsActivityByGroupCode = 0x3025;

        public const UInt16 ResponseListNotifyAdded = 0x3019;
        public const UInt16 ResponseNotifyAdded = 0x3020;
        public const UInt16 ResponseListNotifyUpdated = 0x3021;
        public const UInt16 ResponseNotifyUpdated = 0x3022;
        public const UInt16 ResponseListNotifyRemoved = 0x3023;
        public const UInt16 ResponseNotifyRemoved = 0x3024;
        public const UInt16 ResponseNotifyCustomized = 0x3026;

        public const UInt16 RequestExecTSQL = 0x4010;
        public const UInt16 ResponseExecTSQL = 0x4011;

        public const UInt16 RequestExecTSQLToEn = 0x4012;
        public const UInt16 ResponseExecTSQLToEn = 0x4013;

        public const UInt16 RequestExecTSQLTable = 0x4014;
        public const UInt16 ResponseExecTSQLTable = 0x415;

        public const UInt16 RequestSearchInfo = 0x416;
        public const UInt16 ResponseSearchInfo = 0x417;

        public const UInt16 RequestExecuteCommand = 0x418;
        public const UInt16 ResponseExecuteCommand = 0x419;

        public const UInt16 RequestPaginateInfo = 0x5001;
        public const UInt16 ResponsePaginateInfo = 0x5011;

        public const UInt16 RequestValidateInfo = 0x5002;
        public const UInt16 ResponseValidateInfo = 0x5012;

        public static string ToString(UInt16 function)
        {
            if (function == SetNotify) return @"SetNotify";
            if (function == SessionReady) return @"SessionReady";
            if (function == RequestList) return @"RequestList";
            if (function == RequestRemove) return @"RequestRemove";
            if (function == RequestUpdate) return @"RequestUpdate";
            if (function == RequestAddList) return @"RequestAdd";
            if (function == RequestRemoveList) return @"RequestRemoveList";
            if (function == RequestUpdateList) return @"RequestUpdateList";
            if (function == ResponseList) return @"ResponseList";
            if (function == Updated) return @"Updated";
            if (function == Removed) return @"Removed";
            if (function == ListAdded) return @"ListAdded";
            if (function == ListUpdated) return @"ListUpdated";
            if (function == ListRemoved) return @"ListRemoved";
            if (function == RequestFailed) return @"RequestFailed";
            if (function == ResponseEdit) return @"ResponseCommand";
            if (function == ResponseFailed) return @"ResponseFailed";
            if (function == RequestInsertOrUpdate) return @"RequestInsertOrUpdate";

            if (function == RequestTryGetID) return @"RequestTryGetID";
            if (function == RequestTryGetFunc) return @"RequestTryGetFunc";
            if (function == RequestGetByWhere) return @"RequestGetByWhere";
            if (function == RequestGetDataAll) return @"RequestGetDataAll";
            if (function == RequestGetDataIsActivity) return @"RequestGetDataIsActivity";
            if (function == RequestGetDataIsActivityByGroup) return @"RequestGetDataIsActivityByGroup";
            if (function == RequestGetDataNotIsActivity) return @"RequestGetDataNotIsActivity";

            if (function == ResponseTryGetID) return @"ResponseTryGetID";
            if (function == ResponseTryGetFunc) return @"ResponseTryGetFunc";
            if (function == ResponseGetByWhere) return @"ResponseGetByWhere";
            if (function == ResponseGetDataAll) return @"ResponseGetDataAll";
            if (function == ResponseGetDataIsActivity) return @"ResponseGetDataIsActivity";
            if (function == ResponseGetDataIsActivityByGroup) return @"ResponseGetDataIsActivityByGroup";
            if (function == ResponseGetDataNotIsActivity) return @"ResponseGetDataNotIsActivity";
            if (function == RequestExecTSQL) return @"RequestExecTSQL";
            if (function == ResponseExecTSQL) return @"ResponseExecTSQL";
            if (function == RequestExecTSQLToEn) return @"RequestExecTSQLToEn";
            if (function == ResponseExecTSQLToEn) return @"ResponseExecTSQLToEn";
            if (function == RequestExecTSQLTable) return @"RequestExecTSQLTable";
            if (function == ResponseExecTSQLTable) return @"ResponseExecTSQLTable";

            if (function == RequestSearchInfo) return @"RequestSearchInfo";
            if (function == ResponseSearchInfo) return @"ResponseSearchInfo";

            if (function == RequestGetDataIsActivityByBranch) return @"RequestGetDataIsActivityByBranch";
            if (function == ResponseGetDataIsActivityByBranch) return @"ResponseGetDataIsActivityByBranch";

            if (function == RequestExecuteCommand) return @"RequestExecuteCommand";
            if (function == ResponseExecuteCommand) return @"ResponseExecuteCommand";

            if (function == RequestPaginateInfo) return @"RequestPaginateInfo";
            if (function == ResponsePaginateInfo) return @"ResponsePaginateInfo";

            if (function == RequestLargeImportInfo) return @"RequestLargeImportInfo";
            if (function == ResponseLargeImportInfo) return @"ResponseLargeImportInfo";

            if (function == RequestListNotifyAdded) return @"RequestListNotifyAdded";
            if (function == RequestNotifyAdded) return @"RequestNotifyAdded";
            if (function == RequestNotifyUpdated) return @"RequestNotifyUpdated";
            if (function == RequestNotifyUpdated) return @"RequestNotifyUpdated";
            if (function == RequestListNotifyRemoved) return @"RequestListNotifyRemoved";
            if (function == RequestNotifyRemoved) return @"RequestNotifyRemoved";
            if (function == ResponseListNotifyAdded) return @"ResponseListNotifyAdded";
            if (function == ResponseNotifyAdded) return @"ResponseNotifyAdded";
            if (function == ResponseListNotifyUpdated) return @"ResponseListNotifyUpdated";
            if (function == ResponseNotifyUpdated) return @"ResponseNotifyUpdated";
            if (function == ResponseListNotifyRemoved) return @"ResponseListNotifyRemoved";
            if (function == ResponseNotifyRemoved) return @"ResponseNotifyRemoved";
            if (function == RequestNotifyCustomized) return @"RequestNotifyCustomized";
            if (function == ResponseNotifyCustomized) return @"ResponseNotifyCustomized";
            if (function == RequestGetDataByOption) return @"RequestGetDataByOption";
            if (function == ResponseGetDataByOption) return @"ResponseGetDataByOption";

            if (function == RequestCheckExistsBySCondition) return @"RequestCheckExistsBySCondition";
            if (function == ResponseCheckExistsBySCondition) return @"ResponseCheckExistsBySCondition";
            if (function == RequestGetDataBySCondition) return @"RequestGetDataBySCondition";
            if (function == ResponseGetDataBySCondition) return @"ResponseGetDataBySCondition";

            if (function == RequestValidateInfo) return @"RequestValidateInfo";
            if (function == ResponseValidateInfo) return @"ResponseValidateInfo";

            if (function == RequestMDAdd) return @"RequestMDAdd";
            if (function == RequestMDUpdate) return @"RequestMDUpdate";
            if (function == RequestMDIorU) return @"RequestMDIorU";
            if (function == ResponseMDAdd) return @"ResponseMDAdd";
            if (function == ResponseMDUpdate) return @"ResponseMDUpdate";
            if (function == ResponseMDIorU) return @"ResponseMDIorU";

            return "";
        }
    }
}
