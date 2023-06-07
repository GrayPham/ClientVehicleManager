
namespace ManagementStore.Form.Camera
{
    partial class SubFormCamera
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxCamera = new DevExpress.XtraEditors.ComboBoxEdit();
            this.trackBarControlDark = new DevExpress.XtraEditors.TrackBarControl();
            this.trackBarControlBright = new DevExpress.XtraEditors.TrackBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxCamera.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControlDark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControlDark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControlBright)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControlBright.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(12, 42);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(77, 24);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Camera";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(12, 115);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(93, 24);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Darkness";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(12, 185);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(108, 24);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "Brightness";
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.Location = new System.Drawing.Point(147, 46);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxCamera.Size = new System.Drawing.Size(125, 22);
            this.comboBoxCamera.TabIndex = 5;
            this.comboBoxCamera.SelectedIndexChanged += new System.EventHandler(this.comboBoxCamera_SelectedIndexChanged);
            // 
            // trackBarControlDark
            // 
            this.trackBarControlDark.EditValue = null;
            this.trackBarControlDark.Location = new System.Drawing.Point(140, 121);
            this.trackBarControlDark.Name = "trackBarControlDark";
            this.trackBarControlDark.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.trackBarControlDark.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.trackBarControlDark.Size = new System.Drawing.Size(132, 56);
            this.trackBarControlDark.TabIndex = 6;
            this.trackBarControlDark.EditValueChanged += new System.EventHandler(this.trackBarControlDark_EditValueChanged);
            // 
            // trackBarControlBright
            // 
            this.trackBarControlBright.EditValue = null;
            this.trackBarControlBright.Location = new System.Drawing.Point(140, 191);
            this.trackBarControlBright.Name = "trackBarControlBright";
            this.trackBarControlBright.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.trackBarControlBright.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.trackBarControlBright.Size = new System.Drawing.Size(132, 56);
            this.trackBarControlBright.TabIndex = 7;
            this.trackBarControlBright.EditValueChanged += new System.EventHandler(this.trackBarControlBright_EditValueChanged);
            // 
            // SubFormCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 259);
            this.Controls.Add(this.trackBarControlBright);
            this.Controls.Add(this.trackBarControlDark);
            this.Controls.Add(this.comboBoxCamera);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "SubFormCamera";
            this.ShowIcon = false;
            this.Text = "Edit Camera";
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxCamera.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControlDark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControlDark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControlBright.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControlBright)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TrackBarControl trackBarControlDark;
        private DevExpress.XtraEditors.TrackBarControl trackBarControlBright;
        internal DevExpress.XtraEditors.ComboBoxEdit comboBoxCamera;
    }
}