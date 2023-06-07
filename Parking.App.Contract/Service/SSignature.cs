using System;

namespace Parking.App.Contract.Service
{
    public class SSignature
    {
        public const uint LoginService = 999; // Nót 
        public const uint ServerService = 999; // Nót 
        public const uint tblClientSoundMgtService = 100;
        public const uint tblAdMgtService = 101;
        public const uint tblDeviceRequestConnectService = 103;
        public const uint tblUserService = 104;
        public const uint tblUserPhotoService = 105;
        public const uint tblStoreDeviceService = 106;
        public const uint tblUserMgtStoreService = 107;
        public const uint tblStoreEnvironmentSettingService = 111;
        public const uint tblStoreMasterService = 117;
        public const uint tblCommonService = 118;

        public static String ToString(uint signature)
        {
            switch (signature)
            {
                case tblClientSoundMgtService:
                    return nameof(tblClientSoundMgtService);
                case tblAdMgtService:
                    return nameof(tblAdMgtService);
                case tblStoreDeviceService:
                    return nameof(tblStoreDeviceService);
                case tblDeviceRequestConnectService:
                    return nameof(tblDeviceRequestConnectService);
                case tblUserService:
                    return nameof(tblUserService);
                case tblUserPhotoService:
                    return nameof(tblUserPhotoService);
                case tblUserMgtStoreService:
                    return nameof(tblUserMgtStoreService);
                case tblStoreEnvironmentSettingService:
                    return nameof(tblStoreEnvironmentSettingService);
                case tblStoreMasterService:
                    return nameof(tblStoreMasterService);
                case tblCommonService:
                    return nameof(tblCommonService);
                case ServerService:
                    return nameof(ServerService);
                default:
                    return "";
            }
        }
    }
}
