using DevExpress.XtraEditors;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.Structure;
using ManagementStore.Common;
using ManagementStore.DTO;
using ManagementStore.Services;
using Security;
using Security.VehicleCheckHttpClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementStore.Form.Camera
{
    public partial class PictureControl : DevExpress.XtraEditors.XtraUserControl
    {
        // YOLO Detect
        private YoloDetectServices detect = new YoloDetectServices(CudaInvoke.HasCuda);
        private YoloModelDto dto;
        // Video Setting
        VideoCapture capture;
        List<string> dataCamera = new List<string>();
        //private VideoCapture camera1;
        private SocketDetect encode;
        private bool captureInProgress = false;
        private byte bitSent = 1;
        int cameraindex = -1;
        // API
        VehicleCheck cVehicle = new VehicleCheck();
        private bool accReport = false;
        //Test FPS
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private int waitTime = 0;
        int currentFrame = 0;

        // Brightness
        double bright = 0;
        double dark = 0;
        #region Initialization
        public PictureControl(int index, SocketDetect socket )
        {
            InitializeComponent();
            this.encode = socket;
            loadCamera();
            cameraindex = index;
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
        private void resetPictureBox()
        {
            pictureBoxCamera.Image = Image.FromFile(ModelConfig.constImagePath);
        }
        private void PictureControl_Load(object sender, EventArgs e)
        {
            if(cameraindex >= 0)
            {
                if (capture == null)
                {
                    try
                    {
                        capture = new VideoCapture(cameraindex);
                        captureInProgress = true;
                        Application.Idle += ProcessFrame;
                    }
                    catch (NullReferenceException excpt)
                    {
                        MessageBox.Show(excpt.Message);
                    }
                }
            }
            else
            {
                resetPictureBox();
            }
        }
        #endregion
        private async void ProcessFrame(object sender, EventArgs arg)
        {
            if (captureInProgress)
            {   
                if(encode.SocketStatus() == true) 
                { 
                    // Light treatment
                    double contrast = Math.Pow((100.0 + bright) / 100.0, 2);
                    Image<Bgr, Byte> ImageFrame =  capture.QueryFrame().ToImage<Bgr, byte>();  //line 1
                    CvInvoke.ConvertScaleAbs(ImageFrame, ImageFrame, contrast, bright);
                    Image image = ImageFrame.ToBitmap();
                    dto = detect.detect(image);
                    // Check execution time limit
                    if (dto != null )
                    {
                        // Check Velhike input output
                        if (bitSent == 1 && dto.countListPrediction() > 0)
                        {
                            if (waitTime == 10)
                            {
                                bitSent = 0;
                                using (YoloModelDto dto_sent = new YoloModelDto(dto.getImageBase(), dto.yoloPredictions))
                                {
                                
                                    // return LP
                                    string mess = await encode.request(dto_sent.getImageBase(), dto_sent.yoloPredictions);
                                    textEditLP.Text = mess;
                                    // Handle Sent API CheckINOUT
                                    if (ModelConfig.socketOpen && mess != "")
                                    {
                                        if(mess != "None")
                                        {
                                            ModelConfig.listFaceCamera[0].startFaceDetect();
                                            Image face = ModelConfig.listFaceCamera[0].getFaceImage();
                                            ModelConfig.listFaceCamera[0].endCameraFaceDetect();
                                            if (face != null || pictureBoxCamera.Image != null)
                                            {
                                                Image lp = pictureBoxCamera.Image;
                                                string resultCheckout = await cVehicle.CheckInVehicleAsync(mess, face, lp);
                                                if (resultCheckout == "Successful")
                                                {
                                                    cEditInVehicle.Checked = true;
                                                    await Task.Delay(3000);
                                                    waitTime = 0;
                                                    textEditLP.Text = "";
                                                    cEditInVehicle.Checked = false;
                                                    bitSent = 1;
                                                }
                                                else if(resultCheckout == "Block")
                                                {
                                                    cEditInVehicle.ForeColor = Color.Red;
                                                    accReport = true; // Accept for user can report
                                                }    
                                            }                              
                                        }

                                    }
                                    else
                                    {
                                        // Reopen
                                        while (true)
                                        {
                                            bool connect = await encode.OpenConnectAsync();
                                            if (connect)
                                                break;
                                        }
                                    }

                                }
                                bitSent = 1;
                            }
                            else
                            {
                                waitTime++;
                            }
                        }
                        else if (dto.countListPrediction() == 0)
                        {
                            waitTime = 0;
                        }
                        pictureBoxCamera.Image = dto.getImageDetect();
                        ModelConfig.socketOpen = encode.SocketStatus();
                    }
                }
                // IF connect ->> waitting 
                else
                {
                    await encode.OpenConnectAsync();
                }
            }
        }

        private void pictureBoxCamera_Paint(object sender, PaintEventArgs e)
        {
            currentFrame++;
        }

  
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
            if (dataCamera[cameraindex] == "OFF")
            {
                captureInProgress = false;
                cameraindex = -1;

                Application.Idle -= ProcessFrame;
                resetPictureBox();
                capture.Dispose();
            }
            else
            {
                capture = new VideoCapture(cameraindex);
                captureInProgress = true;
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

        private void cEditInVehicle_Click(object sender, EventArgs e)
        {
            cEditInVehicle.ForeColor = Color.Black;
        }
        #region Report

        private async void panelInFor_DoubleClickAsync(object sender, EventArgs e)
        {
            if(accReport == true)
            {
                ModelConfig.listFaceCamera[0].startFaceDetect();
                Image face = ModelConfig.listFaceCamera[0].getFaceImage();
                ModelConfig.listFaceCamera[0].endCameraFaceDetect();
                if (face != null || pictureBoxCamera.Image != null)
                {
                    bool resultReport = await cVehicle.TrackReportAsync(pictureBoxCamera.Image,face, textEditLP.Text);
                    accReport = false;
                }
            }
        }
        #endregion
    }
}
