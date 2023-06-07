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
    public partial class FaceTaken : System.Windows.Forms.UserControl
    {
        public FaceTaken()
        {
            InitializeComponent();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            Utils.Back(ParentForm, "pictureBoxFace", "pictureBoxName", "FullName");

        }
    }
}
