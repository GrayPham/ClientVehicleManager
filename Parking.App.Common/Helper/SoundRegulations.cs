using System.Collections.Generic;
namespace Parking.App.Common.Helper
{
    public static class SoundRegulations
    {
        public static Dictionary<string, string> ListSoundType = new Dictionary<string, string>()
        {
            {"SOUN01","AuthMethod"},
            {"SOUN02","IdCardAuth"},
            {"SOUN03","PhoneAuth1"},
            {"SOUN04","PhoneAuth2"},
            {"SOUN05","PhoneAuth3"},
            {"SOUN06","PhoneAuth4"},
            {"SOUN07","PhoneAuth5"},
            {"SOUN09","ScannerImage"},
            {"SOUN10","PhoneInput"},
            {"SOUN11","PhotoShootAndBack"},
            {"SOUN12","DisplayImage"},
            {"SOUN13","TermsConfirm"},
            {"SOUN14","DeviceGuide"},
            {"SOUN15","CancelMessage"},
            {"SOUN16","RegisterdMember"},
            {"SOUN17","CompleteRegister"},
            {"SOUN18","FaceCheckDeviceGuideOK"},
            {"SOUN19","Home_Sound"},
            {"SOUN20","PhoneAuth6"},
            {"SOUN21","UseIdCardCamera"},
            {"SOUN22","UserAlreadyRegistered"}
        };


        public static EHistoryType S2EHistoryType(string value)
        {
            switch ((string.Empty + value).ToUpper())
            {
                case nameof(EHistoryType.FACEOK):
                    return EHistoryType.FACEOK;
                case nameof(EHistoryType.REGIEST):
                    return EHistoryType.REGIEST;
                case nameof(EHistoryType.OPENDOOR):
                    return EHistoryType.OPENDOOR;
                case nameof(EHistoryType.NONE):
                default:
                    return EHistoryType.NONE;
            }
        }

        public enum EDeviceType { UNKNOWN, KIOSK, DVC001, DVC002 }
        public enum EHistoryType { NONE, FACEOK, REGIEST, OPENDOOR }
        public enum EDisplayScreenTop { AD, CAMERA }

    }

}


