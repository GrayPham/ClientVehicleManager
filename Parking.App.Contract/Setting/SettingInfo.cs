using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.App.Contract.Setting
{
    public class SettingInfo
    {
        //**----------------------------------------------------------------------
        public int PortUser { get; set; }
        public string IPServer { get; set; }
        public string VersionName { get; set; }
        public int VersionCode { get; set; }
        public string VersionBy { get; set; }
        public string VersionDate { get; set; }
        public string Company { get; set; }
        public string Copyright { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AppName { get; set; }
        public string Author { get; set; }
        public string GuiD { get; set; }
        public string Phone { get; set; }
        public string SeriKey { get; set; }
        public string LicenseKey { get; set; }
        public string SoftwareKey { get; set; }
        public string PCID { get; set; }
        public string CpuIdKey { get; set; }
        public string InstallID { get; set; }

        //**----------------------------------------------------------------------
        public string PhoneNumberValidation { get; set; }
        public string PhoneAuthFailure { get; set; }
        public string WrongCapcha { get; set; }
        public string IsSussessGetData { get; set; }
        public string CodeSussesGetData { get; set; }
        public string RightOTP { get; set; }
        public string TypeImg { get; set; }
        public string TempMember { get; set; }
        public string OfficeMember { get; set; }
        public int Minors { get; set; }
        public string CardMethod { get; set; }
        public string PhoneMethod { get; set; }
        public string AuthMethodHeader1 { get; set; }
        public string AuthMethodHeader2 { get; set; }
        public string CheckUserInforHeader1 { get; set; }
        public string CheckUserInforHeader2 { get; set; }
        public string IdCardHeader1 { get; set; }
        public string IdCardHeader2 { get; set; }
        public string InputPhoneNumberHeader1 { get; set; }
        public string InputPhoneNumberHeader2 { get; set; }
        public string PhoneAuthHeader1 { get; set; }
        public string PhoneAuthHeader2 { get; set; }
        public string ScannerPhotoCardHeader1 { get; set; }
        public string ScannerPhotoCardHeader2 { get; set; }
        public string ApiServerURL { get; set; }
        public string ApiVersionURL { get; set; }
        public string ApiOcrURL { get; set; }
        public string OcrSecretCode { get; set; }
        public string ApiQrURL { get; set; }
        public string SRP001 { get; set; }
        public string URIActiveMq { get; set; }
        public string UiPassTextFolder { get; set; }
        public string UiPassImageFolder { get; set; }
        public string PostCallUri { get; set; }
        public string KcbModule { get; set; }
        public string PrevArrow { get; set; }
        public string NextArrow { get; set; }

        public string TokenTelegram { get; set; }
        public string IdUserTelegram { get; set; }
        public bool isEnableAutomaticalyRunApp = false;
        public string PathAutoUpdate { get; set; }
        public string DeviceType { get; set; }
        public string FileAutoUpdate { get; set; }
        public string AppAutoUpdate { get; set; }

        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Title3 { get; set; }
        public string Content1 { get; set; }
        public string Content2 { get; set; }
        public string Content3 { get; set; }

        public int CameraTop { get; set; }
        public int CameraBot { get; set; }

        //**----------------------------------------------------------------------
    }
}

