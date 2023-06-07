
namespace ManagementStore.Form.Camera
{
    partial class PictureControl
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxSetting = new System.Windows.Forms.PictureBox();
            this.pictureBoxCamera = new System.Windows.Forms.PictureBox();
            this.lbFPS = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textEditLP = new DevExpress.XtraEditors.TextEdit();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.cEditInVehicle = new DevExpress.XtraEditors.CheckEdit();
            this.panelInFor = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditLP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cEditInVehicle.Properties)).BeginInit();
            this.panelInFor.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBoxSetting);
            this.panel1.Controls.Add(this.pictureBoxCamera);
            this.panel1.Location = new System.Drawing.Point(0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(423, 232);
            this.panel1.TabIndex = 1;
            // 
            // pictureBoxSetting
            // 
            this.pictureBoxSetting.Image = global::ManagementStore.Properties.Resources.technology_32x32;
            this.pictureBoxSetting.Location = new System.Drawing.Point(375, 3);
            this.pictureBoxSetting.Name = "pictureBoxSetting";
            this.pictureBoxSetting.Size = new System.Drawing.Size(45, 45);
            this.pictureBoxSetting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSetting.TabIndex = 1;
            this.pictureBoxSetting.TabStop = false;
            this.pictureBoxSetting.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSetting_MouseClick);
            // 
            // pictureBoxCamera
            // 
            this.pictureBoxCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxCamera.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxCamera.Name = "pictureBoxCamera";
            this.pictureBoxCamera.Size = new System.Drawing.Size(423, 232);
            this.pictureBoxCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCamera.TabIndex = 0;
            this.pictureBoxCamera.TabStop = false;
            this.pictureBoxCamera.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxCamera_Paint);
            // 
            // lbFPS
            // 
            this.lbFPS.AutoSize = true;
            this.lbFPS.BackColor = System.Drawing.Color.Gainsboro;
            this.lbFPS.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFPS.Location = new System.Drawing.Point(3, 246);
            this.lbFPS.Name = "lbFPS";
            this.lbFPS.Size = new System.Drawing.Size(32, 18);
            this.lbFPS.TabIndex = 2;
            this.lbFPS.Text = "FPS";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 40);
            this.label1.TabIndex = 5;
            this.label1.Text = "LP:";
            // 
            // textEditLP
            // 
            this.textEditLP.Location = new System.Drawing.Point(80, 36);
            this.textEditLP.Name = "textEditLP";
            this.textEditLP.Properties.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.textEditLP.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEditLP.Properties.Appearance.Options.UseBackColor = true;
            this.textEditLP.Properties.Appearance.Options.UseFont = true;
            this.textEditLP.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textEditLP.Properties.ReadOnly = true;
            this.textEditLP.Size = new System.Drawing.Size(337, 44);
            this.textEditLP.TabIndex = 6;
            // 
            // cEditInVehicle
            // 
            this.cEditInVehicle.Location = new System.Drawing.Point(303, 3);
            this.cEditInVehicle.Name = "cEditInVehicle";
            this.cEditInVehicle.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cEditInVehicle.Properties.Appearance.Options.UseFont = true;
            this.cEditInVehicle.Properties.Caption = "Accept";
            this.cEditInVehicle.Properties.ReadOnly = true;
            this.cEditInVehicle.Size = new System.Drawing.Size(114, 32);
            this.cEditInVehicle.TabIndex = 7;
            this.cEditInVehicle.Click += new System.EventHandler(this.cEditInVehicle_Click);
            // 
            // panelInFor
            // 
            this.panelInFor.BackColor = System.Drawing.Color.Gainsboro;
            this.panelInFor.Controls.Add(this.cEditInVehicle);
            this.panelInFor.Controls.Add(this.label1);
            this.panelInFor.Controls.Add(this.textEditLP);
            this.panelInFor.Location = new System.Drawing.Point(0, 241);
            this.panelInFor.Name = "panelInFor";
            this.panelInFor.Size = new System.Drawing.Size(420, 85);
            this.panelInFor.TabIndex = 8;
            this.panelInFor.DoubleClick += new System.EventHandler(this.panelInFor_DoubleClickAsync);
            // 
            // PictureControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelInFor);
            this.Controls.Add(this.lbFPS);
            this.Controls.Add(this.panel1);
            this.Name = "PictureControl";
            this.Size = new System.Drawing.Size(423, 326);
            this.Load += new System.EventHandler(this.PictureControl_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditLP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cEditInVehicle.Properties)).EndInit();
            this.panelInFor.ResumeLayout(false);
            this.panelInFor.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxCamera;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbFPS;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit textEditLP;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private System.Windows.Forms.PictureBox pictureBoxSetting;
        private DevExpress.XtraEditors.CheckEdit cEditInVehicle;
        private System.Windows.Forms.Panel panelInFor;
    }
}
