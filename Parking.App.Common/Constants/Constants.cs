using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Common.Constants
{
    public static class Constants
    {
        public static string PhoneNumberValidation = @"^[0-9\-\+]{9,15}$";
        public static string PhoneAuthFailure = "인증완료";
        public static string WrongCapcha = "그림문자가 일치하지 않습니다.";
        public static string IsSussessGetData = "KCB Mobile Phone Identity Verification Service Sample 3";
        public static string CodeSussesGetData = "B000";
        public static string RightOTP = "인증번호";
        public static string TypeImg = "data:imag";
        public static string TempMember = "USST02";
        public static string OfficeMember = "USST01";
        public static string CardMethod = "IdCardAuth";
        public static string PhoneMethod = "PhoneAuth";
        public static string AuthMethodHeader1 = "인증 수단 선택";
        public static string AuthMethodHeader2 = "인증 방식을 선택하여 주십시오.";
        public static string CheckUserInforHeader1 = "인증 수단 선택";
        public static string CheckUserInforHeader2 = "인증 방식을 선택하여 주십시오.";
        public static string IdCardHeader1 = "본인 인증";
        public static string IdCardHeader2 = "본인 확인 후 입실이 가능합니다.";

        public static string InputPhoneNumberHeader1 = "신분증 인증";
        public static string InputPhoneNumberHeader2 = "휴대폰 번호를 입력 해 주십시오";

        public static string PhoneAuthHeader1 = "휴대폰 인증";
        public static string PhoneAuthHeader2 = "아래 정보를 입력하여 주십시오.";

        public static string ScannerPhotoCardHeader1 = "본인 인증";
        public static string ScannerPhotoCardHeader2 = "본인 확인 후 입실이 가능합니다.";
        //http://localhost:5001/Kiosk/KioskService/GetData
        //http://192.168.100.45:8080/Kiosk/KioskService/GetData
        //http://api.owlgardien.com:81/Kiosk/KioskService/GetData
        // public static  string ApiServerURL = "http://localhost:5001/Kiosk/KioskService/GetData";
        public static string ApiServerURL = "http://api.owlgardien.com:81/Kiosk/KioskService/GetData";
        public static string ApiVersionURL = "http://api.owlgardien.com:81/System/DataService/CheckVersion?type={0}&versionCode={1}";
        
        public static string ApiOcrURL = "https://9kbg1stkdv.apigw.ntruss.com/custom/v1/19402/87548f888389870f328ab17b3ff6e27b814d62f7cf5caac4b9aad7ece2125df8/general";
        public static string OcrSecretCode = "aUN6UGJPdFlVSk5yTlpEUWlNUGlUY3B6QlllTkdibW8=";
        public static int RequestTimeOut = 20000;

        public static string ApiQrURL = "http://101.101.210.117/qrmpm/start";
        //public const string ApiQrURL = "http://mvadev.mobileid.go.kr:8080/qrmpm/start";
        public static string SRP001 = "SRP001";
        public static string URIActiveMq = "activemq:tcp://101.101.210.117:61616";
        public static string UiPassTextFolder = @"C:\";
        public static string UiPassImageFolder = @"C:\UIPASS";
        public static string PostCallUri = "https://www.owlgardien.com/en/voicefilemgt/GetSourceFileAudio?soundNo";
        public static string PostCallUriAdmgt = "https://www.owlgardien.com/en/AdMgt/GetAdImage?adNo={0}";
        public static string KcbModule = "http://kcbmodule.owlgardien.com:81/phone_popup/phone_popup2";
        public static string PrevArrow = "PrevArrow";
        public static string NextArrow = "NextArrow";

        //  public const string IpServer= "kc192.168.100.23";
        public const string IpServer = "101.101.210.117";
        public const string DeviceType = "DVC001";

        public static bool isEnableAutomaticalyRunApp = true;
        public static string ServerMeetProblemMessage = "프로세스에 문제가 있습니다. 다시 시도하십시오!";
        public static string TokenTelegram = "";
        public static string IdUserTelegram = "";
        public static string RejectCode = "REJMS1";
        public static string ApproveCode = "CHKUS1";
        public static string MinorCode = "MINOR1";
        public static string SimilarityRate = "SRP001";

        public static string ValidPhoneNumber = "올바른 휴대폰번호를 입력해주세요";
        public static string ValidDateOfBirth = "올바른 생년월일을 입력해주세요";
        public static string ValidName = "올바른 성명정보를 입력해주세요";
        public static string RefreshNumber = "0101010101010";
        public static string RefreshDob = "345442";
        public static string RefreshGender = "1";
        public static string RefreshName1 = "asdasd";
        public static string RefreshName2 = "123213";
        public static string RefreshCapcha = "131313";





        #region AutomaticallyRunAppp
        public static string AutoRunApp_Name = "Kiosk.App.Shell.exe";
        public static string AutoRunApp_Desc = "New task create";
        public static string AutoRunApp_StartDate = "04/01/2022 12:10:15 AM";
        public static string AutoRunApp_EndDate = "04/09/2030 12:10:15 AM";
        public static string AutoRunApp_AppName = "Kiosk.App.Shell.exe";
        public static string CardImage = @"IdCardImage\CardImage.JPG";
        public static string UipassImageFolder = @"C:\UIPASS\IMAGE";
        public static string UipassGateFile = @"C:\gate.txt"; 
        public static string DisplayImage = @"IdCardImage\DisplayImage.JPG";
        public static string ScannerFaceRecognizeImage = @"IdCardImage\ScannerRecognizeImage.JPG";
        public static string CardImageBig = @"IdCardImage\CardImageBig.JPG";
        public static string DisplayPhotoBig = @"IdCardImage\DisplayPhotoBig.JPG";
        public static string ImageTaking = @"IdCardImage\ImageTaking.JPG";
        public static double FocusScrollView = 450;
        public static double FocusScrollViewMin = 300;
        public static double FocusScrollViewMax = 600;
        #endregion

        public static string PathAutoUpdate = "\\AutoUpdate";
        public static string FileAutoUpdate = "\\VersionInfo.txt";
        public static string AppAutoUpdate = "\\Kiosk.AutoUpdate.exe";

        public static string Title1 = "개인정보 수집항목 동의";
        public static string Title2 = "안개인정보 이용기간";
        public static string Title3 = "개인정보 처리의 위탁";
        public static string Content1 = "";
        public static string Content2 = "";
        public static string Content3 = "";

        public static int CameraTop = 0;
        public static int CameraBot = 1;
    }
}
