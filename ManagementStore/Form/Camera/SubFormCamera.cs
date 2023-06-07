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
using static System.Windows.Forms.ComboBox;

namespace ManagementStore.Form.Camera
{
    public partial class SubFormCamera : DevExpress.XtraEditors.XtraForm
    {
        public event EventHandler ComboBoxIndexChanged;
        public delegate void ValueChangedHandler(int value);
        public event ValueChangedHandler ValueDarkChanged;
        public event ValueChangedHandler ValueBrightChanged;
        public int ComboBoxIndex
        {
            get { return comboBoxCamera.SelectedIndex; }
        }
        
        public SubFormCamera(List<string> datalistcam,int indexcame)
        {
            InitializeComponent();
            comboBoxCamera.Properties.Items.AddRange( datalistcam);
            comboBoxCamera.SelectedIndex = indexcame;
        }
        #region Return data for MainForm
        private void comboBoxCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxIndexChanged?.Invoke(this, e);
        }

        private void trackBarControlDark_EditValueChanged(object sender, EventArgs e)
        {
            if (ValueDarkChanged != null)
            {
                ValueDarkChanged(trackBarControlDark.Value);
            }
        }

        private void trackBarControlBright_EditValueChanged(object sender, EventArgs e)
        {
            if (ValueBrightChanged != null)
            {
                ValueBrightChanged(trackBarControlBright.Value);
            }
        }
        #endregion
    }
}