using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Diagnostics;
using ManagementStore.Common;

namespace ManagementStore.Form.Camera
{
    public partial class FaceCameraControl : DevExpress.XtraEditors.XtraUserControl
    {
        #region Test FPS
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        int currentFrame = 0;
        #endregion
        #region Model detect
        // Declare Variables
        CascadeClassifier faceDetect;
        Image<Bgr, Byte> frame;
        VideoCapture _camera;
        public bool startDetect = false;
        string ipMac = "127.1.1.1";
        int countFace = 0;
        int cameraindex;
        List<string> dataCamera = new List<string>();
        // Brightness
        double bright = 0;
        double dark = 0;
        #endregion
        public FaceCameraControl(int cameraIndex)
        {
            InitializeComponent();
            loadCamera();
            if (cameraIndex >= 0)
            {
                this.cameraindex = cameraIndex;
                _camera = new VideoCapture(cameraIndex);
                faceDetect = new CascadeClassifier(Application.StartupPath + "/Assets/HaarCascade/haarcascade_frontalface_default.xml");
                Application.Idle += ProcessFrame;
            }
            else
            {
                pBoxFace.Image = Image.FromFile(ModelConfig.constImagePath);
            }
            timer.Interval = 1000;
            timer.Tick += (sender, e) =>
            {
                int previousFrame = currentFrame;
                currentFrame = 0;

                // Calculate and display the number of FPS
                double fps = (double)(previousFrame) / 1;
                lbFPS.Text = string.Format("{0:0.00} FPS", fps);
            };
            // Start Timer
            timer.Start();

        }
        private void loadCamera()
        {
            dataCamera.AddRange(ModelConfig.cameraList);
        }
        private void ProcessFrame(object sender, EventArgs arg)
        {
            frame = _camera.QueryFrame().ToImage<Bgr, Byte>();
            if (frame != null)
            {
                if(startDetect == true)
                {
                    var grayframe = frame.Convert<Gray, byte>();
                    var faces = faceDetect.DetectMultiScale(grayframe, 1.1, 10, Size.Empty); //the actual face detection happens here
                    
                    foreach (var face in faces)
                    {
                        countFace++;
                        frame.Draw(face, new Bgr(Color.BurlyWood), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them
                    }
                }
                pBoxFace.Image = frame.ToBitmap();

            }
            
        }
        public bool startFaceDetect()
        {
            startDetect = true;
            return true;
        }
        public void endCameraFaceDetect()
        {
            startDetect = false;
            return;
        }
        public void endCameraFace(string ip)
        {
            if(ipMac == ip)
            {
                Application.Idle -= ProcessFrame;
                return;
            }    
        }
        
        private void pBoxFace_Paint(object sender, PaintEventArgs e)
        {
            currentFrame++;
        }
        #region Get Face Image Test
        public Image getFaceImage()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.Elapsed.TotalSeconds < 5)
            {
                if (pBoxFace.Image != null && countFace > 0)
                    break;
            }
            stopwatch.Stop();
            stopwatch.Reset();
            if (pBoxFace.Image != null)
                return pBoxFace.Image;
            return null;
        }
        #endregion
        #region SubForm Edit
        private Point clickPoint;
        private void pictureBoxSetting_MouseClick(object sender, MouseEventArgs e)
        {
            clickPoint = e.Location;
            SubFormCamera subFormCamera = new SubFormCamera(dataCamera, cameraindex);
            subFormCamera.Location = new Point(this.Location.X + clickPoint.X, this.Location.Y + clickPoint.Y);
            subFormCamera.BringToFront();
            subFormCamera.ComboBoxIndexChanged += SubForm_ComboBoxIndexChanged;
            subFormCamera.ValueDarkChanged += SubForm_ValueDarkChanged;
            subFormCamera.ValueBrightChanged += SubForm_ValueBrightChanged;
            this.Enabled = false;
            subFormCamera.ShowDialog();
            this.Enabled = true;
        }
        private void SubForm_ComboBoxIndexChanged(object sender, EventArgs e)
        {
            // Cập nhật giá trị của comboBoxIndex khi giá trị trên form con thay đổi
            cameraindex = ((SubFormCamera)sender).ComboBoxIndex;
            // Cập nhật các thành phần trên form cha sử dụng giá trị của comboBoxIndex
            if (dataCamera[cameraindex] == "None")
            {
                
                cameraindex = -1;

                Application.Idle -= ProcessFrame;
                pBoxFace.Image = null;
                _camera.Dispose();
            }
            else
            {
                _camera.Dispose();
                _camera = new VideoCapture(cameraindex);
                //capture = new VideoCapture(cameraindex); // BUG
                Application.Idle += ProcessFrame;
            }
        }
        private void SubForm_ValueDarkChanged(int value)
        {
            dark = value;
        }
        private void SubForm_ValueBrightChanged(int value)
        {
            bright = value;
        }
        #endregion
    }
}
