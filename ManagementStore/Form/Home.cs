using System;
using System.Windows.Forms;
using ManagementStore.Extensions;
namespace ManagementStore.Form
{
    public partial class Home : DevExpress.XtraEditors.XtraForm
    {

        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            string html = "<html><head>";
            string url = "https://www.youtube.com/watch?v=Z9uEn2IVPkQ";
            html += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
            html += "<iframe id='video' src= 'https://www.youtube.com/embed/{0}?autoplay=1' width='785' height='463' frameborder='0' allowfullscreen></iframe>";
            html += "</body></html>";
            this.webBrowserVideo.DocumentText = string.Format(html, Utils.GetVideoId(url));
        }

        private void tileBarItem3_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

        }
    }
}