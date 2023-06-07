
namespace ManagementStore.Form.User
{
    partial class FaceTaken
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
            this.btnDone = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrev = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnDone
            // 
            this.btnDone.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.btnDone.Appearance.BorderColor = System.Drawing.Color.White;
            this.btnDone.Appearance.Font = new System.Drawing.Font("Tahoma", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDone.Appearance.Options.UseBackColor = true;
            this.btnDone.Appearance.Options.UseBorderColor = true;
            this.btnDone.Appearance.Options.UseFont = true;
            this.btnDone.Location = new System.Drawing.Point(322, 630);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(248, 92);
            this.btnDone.TabIndex = 43;
            this.btnDone.Text = "Hoàn tất";
            // 
            // btnPrev
            // 
            this.btnPrev.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnPrev.Appearance.BorderColor = System.Drawing.Color.White;
            this.btnPrev.Appearance.Font = new System.Drawing.Font("Tahoma", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Appearance.Options.UseBackColor = true;
            this.btnPrev.Appearance.Options.UseBorderColor = true;
            this.btnPrev.Appearance.Options.UseFont = true;
            this.btnPrev.Location = new System.Drawing.Point(20, 630);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(234, 92);
            this.btnPrev.TabIndex = 44;
            this.btnPrev.Text = "Previous";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // FaceTaken
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnDone);
            this.Name = "FaceTaken";
            this.Size = new System.Drawing.Size(589, 749);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnDone;
        private DevExpress.XtraEditors.SimpleButton btnPrev;
    }
}
