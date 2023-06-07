using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementStore.Form.Camera.Abstract
{
    public abstract class Camera
    {
        protected VideoCapture _camera;
        protected abstract void  ProcessFrame(object sender, EventArgs arg);
        #region Test FPS
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        int currentFrame = 0;
        #endregion
        #region SubForm Edit
        private Point clickPoint;
        protected abstract void pictureBoxSetting_MouseClick(object sender, MouseEventArgs e);

        protected abstract void SubForm_ComboBoxIndexChanged(object sender, EventArgs e);
        #endregion
    }
}
