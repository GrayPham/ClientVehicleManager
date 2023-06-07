using System;
using System.Collections.Generic;
using System.Text;
using static Parking.App.Common.Helper.SoundRegulations;

namespace Parking.App.Common
{
    public static class ConfigClass
    {
        public static int StoreNo { get; set; }
        public static int StoreDeviceNo { get; set; }
        public static int SimilarityRate { get; set; }
        public static int MinorNumber { get; set; }
        public static DateTime? TimeStart { get; set; }
        public static DateTime? TimeEnd { get; set; }
        public static EHistoryType isRegister { get; set; } = EHistoryType.NONE;
        public static string RejectMessage { get; set; }
        public static string ApproveMessage { get; set; }
        public static string DeviceKey { get; set; }
        public static string FaceOkDeviceKey { get; set; }
        public static bool isPlayHomeSound { get; set; }
        public static string PublicIp { get; set; }
        public static bool IsAdultEnable { get; set; }
        public static bool IsExistingInStoreEnviromentSetting = false;
    }
}
