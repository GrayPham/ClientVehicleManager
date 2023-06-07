using DevExpress.XtraEditors;
using ManagementStore.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementStore.Form.User
{
    public partial class FullName : System.Windows.Forms.UserControl
    {
        public FullName()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Utils.Forward(ParentForm, "pictureBoxName", "pictureBoxFace", "FaceTaken");
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            Utils.Back(ParentForm, "pictureBoxName", "pictureBoxInfo", "InformationUser");

        }
    }
}
