using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagementStore.Form.User;

namespace ManagementStore.Extensions
{
    public static class Utils
    {
        public static string GetVideoId(string url)
        {
            var yMatch = new Regex(@"youtu(?:\be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)").Match(url);
            return yMatch.Success ? yMatch.Groups[1].Value : string.Empty;
        }

        public static void Forward(System.Windows.Forms.Form parentForm, string currentPictureBox, string nextPictureBox, string nextPageName)
        {
            PictureBox curImage = (PictureBox)parentForm.Controls.Find(currentPictureBox, true)[0];
            curImage.Image = Properties.Resources.completed;
            PictureBox preImage = (PictureBox)parentForm.Controls.Find(nextPictureBox, true)[0];
            preImage.Image = Properties.Resources.current;
            parentForm.Controls.Find("panelSlider", true)[0].Controls.Find(nextPageName, true)[0].BringToFront();
        }

        public static void Back(System.Windows.Forms.Form parentForm, string currentPictureBox, string prevPictureBox, string prevPageName)
        {
            PictureBox curImage = (PictureBox)parentForm.Controls.Find(currentPictureBox, true)[0];
            curImage.Image = Properties.Resources.pending;
            PictureBox preImage = (PictureBox)parentForm.Controls.Find(prevPictureBox, true)[0];
            preImage.Image = Properties.Resources.current;
            parentForm.Controls.Find("panelSlider", true)[0].Controls.Find(prevPageName, true)[0].BringToFront();
        }
    }
}
