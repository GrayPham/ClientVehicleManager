using Connect.Common;
using Connect.Common.Common;
using Connect.Common.Contract;
using Connect.Common.Interface;
using Connect.Common.Languages;
using Connect.SocketClient;
using DevExpress.Images;
using Parking.App.Factory;
using Emgu.CV;
using ManagementStore.Common;
using ManagementStore.Form.Camera;
using Parking.App.Contract.Common;
using Parking.App.Interface.Common;
using Parking.App.Language;
using System;
using Security;
using System.Drawing;
using System.Collections.Generic;

namespace ManagementStore.Form
{
    public partial class DetectClient : DevExpress.XtraBars.Ribbon.RibbonForm, IProgramController
    {
        ILog _log;
        private ISocketClient _client;
        private System.Timers.Timer _timer;
        private int _counter;
        private static int Counter = 10;

        // Connect Socket 
        private SocketDetect Encode = new SocketDetect();
        List<string> dataCamera = new List<string>();
        public DetectClient()
        {
            _log = ProgramFactory.Instance.Log;
            InitializeComponent();
            LoadCamera();
        }
        private void DetectClient_Load(object sender, EventArgs e)
        {
            ProgramFactory.Instance.ProgramController = this;
            _log = ProgramFactory.Instance.Log;
            AddEventCommon();
            barItemIP.Caption = "IP:" + ProgramFactory.Instance.IPServer;
            barItemVersion.Caption = LSystem.LVersion + ApplicationInfo.VersionName;
            barItemPort.Caption = string.Format(LSystem.LPort, ApplicationInfo.PortUser);
            if (dataCamera.Count > 2)
            {
                //Camera LP detection
                PictureControl pictureControl = new PictureControl(1, Encode);
                panelIn.Controls.Add(pictureControl);
                PictureControl pictureControl1 = new PictureControl(-1, Encode);
                panelLPOut.Controls.Add(pictureControl1);
                PictureControl pictureControl2 = new PictureControl(-1, Encode);
                panelLPIn1.Controls.Add(pictureControl2);
                PictureControl pictureControl3 = new PictureControl(-1, Encode);
                panelLPOut1.Controls.Add(pictureControl3);
                //Camera Face 1
                FaceCameraControl faceCameraControl = new FaceCameraControl(0);
                ModelConfig.listFaceCamera.Add(faceCameraControl);
                panelFace.Controls.Add(ModelConfig.listFaceCamera[0]);
                // Camera Face 2
                FaceCameraControl faceCameraControl1 = new FaceCameraControl(-1);
                ModelConfig.listFaceCamera.Add(faceCameraControl1);
                panelFace2.Controls.Add(ModelConfig.listFaceCamera[1]);
            }
            else if (dataCamera.Count == 2)
            {
                PictureControl pictureControl = new PictureControl(0, Encode);
                panelIn.Controls.Add(pictureControl);
            }
            
        }

        private void LoadCamera()
        {
            dataCamera.AddRange(ModelConfig.cameraList);

        }
        #region IProgramController

        public void ConnectSuccess(ServerInfo info)
        {
            if (_client == null) return;

            _client.UpdateIP(info.IPServer);
            _client.UpdatePort(info.Port ?? 0);

            barItemIP.Caption = "IP:" + info.IPServer;
            barItemVersion.Caption = "" + info.Port ?? 0 + "||" + LSystem.LVersion + ApplicationInfo.VersionName;
            _client.Connect();
        }

        public void LoginSuccess(SessionInfo info)
        {

        }

        public void SetStatus(string description)
        {
            barItemConnect.Caption = description;
        }

        protected virtual void OnShown()
        {
            _client.Connect();
        }

        protected virtual void AddEventCommon()
        {
            Onload();
            OnShown();
        }
        protected virtual void Onload()
        {
            barItemConnect.Caption = FWLanguages.LSetupConnect;
            _client = ProgramFactory.Instance.SocketClientServer;
            _client.Connected += OnServerConnected;
            _client.Disconnected += OnServerDisconnected;
            ProgramFactory.Instance.ClientSessionHandler.SessionError += ClientSessionHandler_SessionError;
        }
        protected virtual void TimerOnTick(object sender, EventArgs eventArgs)
        {
            _counter--;

            barItemConnect.Caption = string.Format(FWLanguages.LReConnect, _counter);
            barItemConnect.ItemAppearance.Normal.ForeColor = System.Drawing.Color.HotPink;
            barItemConnect.Glyph = ImageResourceCache.Default.GetImage("images/communication/radio_16x16.png");
            if (_counter > 0) return;

            barItemConnect.Caption = FWLanguages.LSetupConnect + " ";
            barItemConnect.ItemAppearance.Normal.ForeColor = System.Drawing.Color.White;
            barItemConnect.Glyph = ImageResourceCache.Default.GetImage("images/programming/technology_16x16.png");

            _timer.Stop();
            _client.ReConnect();

        }
        protected virtual void OnServerConnected(object sender, EventArgs<ITcpClientHandler> e)
        {
            barItemConnect.Caption = FWLanguages.LConnectSuccessfully;
            barItemConnect.ItemAppearance.Normal.ForeColor = System.Drawing.Color.GreenYellow;
            barItemConnect.Glyph = ImageResourceCache.Default.GetImage("images/tasks/status_16x16.png");
        }
        protected virtual void OnServerDisconnected(object sender, EventArgs<string> e)
        {
            _counter = Counter;
            barItemConnect.Caption = e.Data + string.Format(FWLanguages.LReConnect, _counter);
            barItemConnect.ItemAppearance.Normal.ForeColor = System.Drawing.Color.HotPink;
            barItemConnect.Glyph = ImageResourceCache.Default.GetImage("images/communication/wifi_16x16.png");

            _timer = new System.Timers.Timer { Interval = 1000 };
            _timer.Elapsed -= TimerOnTick;
            _timer.Elapsed += TimerOnTick;
            _timer.Start();


        }
        private void ClientSessionHandler_SessionError(object sender, EventArgs<ConnectionFailedReason> e)
        {
            string mes = "";
            switch (e.Data)
            {
                case ConnectionFailedReason.Unknown:
                    mes = LSystem.LConnectionFailedReason_Unknown;
                    break;
                case ConnectionFailedReason.InvalidSerialNumber:
                    mes = LSystem.LConnectionFailedReason_InvalidSerialNumber;
                    break;
                case ConnectionFailedReason.InvalidLicenseKey:
                    mes = LSystem.LConnectionFailedReason_InvalidLicenseKey;
                    break;
                case ConnectionFailedReason.MacError:
                    mes = LSystem.LConnectionFailedReason_Unknown;
                    break;
                default:
                    mes = LSystem.LConnectionFailedReason_Default;
                    break;
            }
            Console.WriteLine(mes);
            _client.Disconnect();
            _timer.Stop();
        }

        #endregion
    }
}