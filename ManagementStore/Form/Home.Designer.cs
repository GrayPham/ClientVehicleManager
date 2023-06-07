
namespace ManagementStore.Form
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraEditors.TileItemElement tileItemElement13 = new DevExpress.XtraEditors.TileItemElement();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            DevExpress.XtraEditors.TileItemElement tileItemElement14 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement15 = new DevExpress.XtraEditors.TileItemElement();
            this.cameraControl1 = new DevExpress.XtraEditors.Camera.CameraControl();
            this.tileBar1 = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroup2 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.tileBarItem3 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.tileBarGroup1 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.tileBarItem2 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.tileBarGroup4 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.tileBarItem1 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.webBrowserVideo = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cameraControl1
            // 
            this.cameraControl1.Location = new System.Drawing.Point(5, 5);
            this.cameraControl1.Name = "cameraControl1";
            this.cameraControl1.Size = new System.Drawing.Size(821, 321);
            this.cameraControl1.TabIndex = 0;
            this.cameraControl1.Text = "cameraControl1";
            // 
            // tileBar1
            // 
            this.tileBar1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tileBar1.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.tileBar1.Groups.Add(this.tileBarGroup2);
            this.tileBar1.Groups.Add(this.tileBarGroup1);
            this.tileBar1.Groups.Add(this.tileBarGroup4);
            this.tileBar1.Location = new System.Drawing.Point(134, 833);
            this.tileBar1.MaxId = 5;
            this.tileBar1.Name = "tileBar1";
            this.tileBar1.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.tileBar1.Size = new System.Drawing.Size(560, 113);
            this.tileBar1.TabIndex = 2;
            this.tileBar1.Text = "12";
            // 
            // tileBarGroup2
            // 
            this.tileBarGroup2.Items.Add(this.tileBarItem3);
            this.tileBarGroup2.Name = "tileBarGroup2";
            // 
            // tileBarItem3
            // 
            this.tileBarItem3.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement13.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage")));
            tileItemElement13.Text = "Register Account";
            this.tileBarItem3.Elements.Add(tileItemElement13);
            this.tileBarItem3.Id = 2;
            this.tileBarItem3.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.tileBarItem3.Name = "tileBarItem3";
            this.tileBarItem3.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.tileBarItem3_ItemClick);
            // 
            // tileBarGroup1
            // 
            this.tileBarGroup1.Items.Add(this.tileBarItem2);
            this.tileBarGroup1.Name = "tileBarGroup1";
            // 
            // tileBarItem2
            // 
            this.tileBarItem2.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement14.Text = "";
            this.tileBarItem2.Elements.Add(tileItemElement14);
            this.tileBarItem2.Id = 4;
            this.tileBarItem2.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.tileBarItem2.Name = "tileBarItem2";
            // 
            // tileBarGroup4
            // 
            this.tileBarGroup4.Items.Add(this.tileBarItem1);
            this.tileBarGroup4.Name = "tileBarGroup4";
            // 
            // tileBarItem1
            // 
            this.tileBarItem1.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement15.Text = "";
            this.tileBarItem1.Elements.Add(tileItemElement15);
            this.tileBarItem1.Id = 3;
            this.tileBarItem1.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.tileBarItem1.Name = "tileBarItem1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.webBrowserVideo);
            this.panelControl1.Controls.Add(this.tileBar1);
            this.panelControl1.Controls.Add(this.cameraControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(826, 958);
            this.panelControl1.TabIndex = 0;
            // 
            // webBrowserVideo
            // 
            this.webBrowserVideo.Location = new System.Drawing.Point(5, 332);
            this.webBrowserVideo.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserVideo.Name = "webBrowserVideo";
            this.webBrowserVideo.Size = new System.Drawing.Size(816, 495);
            this.webBrowserVideo.TabIndex = 3;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 958);
            this.Controls.Add(this.panelControl1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Home";
            this.Load += new System.EventHandler(this.Home_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Camera.CameraControl cameraControl1;
        private DevExpress.XtraBars.Navigation.TileBar tileBar1;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup2;
        private DevExpress.XtraBars.Navigation.TileBarItem tileBarItem3;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup1;
        private DevExpress.XtraBars.Navigation.TileBarItem tileBarItem2;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup4;
        private DevExpress.XtraBars.Navigation.TileBarItem tileBarItem1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.WebBrowser webBrowserVideo;
    }
}