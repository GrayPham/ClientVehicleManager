
namespace ManagementStore.Form.Camera
{
    partial class FaceCameraControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelFace = new System.Windows.Forms.Panel();
            this.lbFPS = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.pictureBoxSetting = new System.Windows.Forms.PictureBox();
            this.pBoxFace = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelFace.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxFace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFace
            // 
            this.panelFace.Controls.Add(this.pictureBoxSetting);
            this.panelFace.Controls.Add(this.pBoxFace);
            this.panelFace.Location = new System.Drawing.Point(0, 3);
            this.panelFace.Name = "panelFace";
            this.panelFace.Size = new System.Drawing.Size(423, 232);
            this.panelFace.TabIndex = 0;
            // 
            // lbFPS
            // 
            this.lbFPS.AutoSize = true;
            this.lbFPS.BackColor = System.Drawing.Color.Transparent;
            this.lbFPS.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFPS.Location = new System.Drawing.Point(5, 249);
            this.lbFPS.Name = "lbFPS";
            this.lbFPS.Size = new System.Drawing.Size(32, 18);
            this.lbFPS.TabIndex = 3;
            this.lbFPS.Text = "FPS";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Location = new System.Drawing.Point(-10, 241);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(433, 85);
            this.panel1.TabIndex = 4;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(143, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(95, 29);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Earnings";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(143, 45);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(116, 36);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Fee: 0 $";
            // 
            // pictureBoxSetting
            // 
            this.pictureBoxSetting.Image = global::ManagementStore.Properties.Resources.technology_32x32;
            this.pictureBoxSetting.Location = new System.Drawing.Point(375, 3);
            this.pictureBoxSetting.Name = "pictureBoxSetting";
            this.pictureBoxSetting.Size = new System.Drawing.Size(45, 45);
            this.pictureBoxSetting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSetting.TabIndex = 4;
            this.pictureBoxSetting.TabStop = false;
            this.pictureBoxSetting.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSetting_MouseClick);
            // 
            // pBoxFace
            // 
            this.pBoxFace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pBoxFace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBoxFace.Location = new System.Drawing.Point(0, 0);
            this.pBoxFace.Name = "pBoxFace";
            this.pBoxFace.Size = new System.Drawing.Size(423, 232);
            this.pBoxFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pBoxFace.TabIndex = 5;
            this.pBoxFace.TabStop = false;
            this.pBoxFace.Paint += new System.Windows.Forms.PaintEventHandler(this.pBoxFace_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ManagementStore.Properties.Resources.payment2;
            this.pictureBox1.Location = new System.Drawing.Point(281, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(151, 78);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // FaceCameraControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbFPS);
            this.Controls.Add(this.panelFace);
            this.Controls.Add(this.panel1);
            this.Name = "FaceCameraControl";
            this.Size = new System.Drawing.Size(423, 326);
            this.panelFace.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxFace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelFace;
        private System.Windows.Forms.Label lbFPS;
        private System.Windows.Forms.PictureBox pictureBoxSetting;
        private System.Windows.Forms.PictureBox pBoxFace;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
