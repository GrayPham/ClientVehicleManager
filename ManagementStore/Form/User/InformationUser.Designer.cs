
namespace ManagementStore.Form.User
{
    partial class InformationUser
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
            this.btnPrev = new DevExpress.XtraEditors.SimpleButton();
            this.btnNext = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnPrev
            // 
            this.btnPrev.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnPrev.Appearance.BorderColor = System.Drawing.Color.White;
            this.btnPrev.Appearance.Font = new System.Drawing.Font("Tahoma", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Appearance.Options.UseBackColor = true;
            this.btnPrev.Appearance.Options.UseBorderColor = true;
            this.btnPrev.Appearance.Options.UseFont = true;
            this.btnPrev.Location = new System.Drawing.Point(21, 635);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(234, 92);
            this.btnPrev.TabIndex = 44;
            this.btnPrev.Text = "Previous";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Appearance.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnNext.Appearance.BorderColor = System.Drawing.Color.White;
            this.btnNext.Appearance.Font = new System.Drawing.Font("Tahoma", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Appearance.Options.UseBackColor = true;
            this.btnNext.Appearance.Options.UseBorderColor = true;
            this.btnNext.Appearance.Options.UseFont = true;
            this.btnNext.Location = new System.Drawing.Point(333, 635);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(234, 92);
            this.btnNext.TabIndex = 43;
            this.btnNext.Text = "Next";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // InformationUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnNext);
            this.Name = "InformationUser";
            this.Size = new System.Drawing.Size(589, 749);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnPrev;
        private DevExpress.XtraEditors.SimpleButton btnNext;
    }
}
