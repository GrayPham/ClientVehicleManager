namespace ManagementStore.Form.User
{
    public partial class RegisterUser : DevExpress.XtraEditors.XtraForm
    {
        public RegisterUser()
        {
            InitializeComponent();
            panelSlider.Controls.Add(new PhoneNumber());
            panelSlider.Controls.Add(new InformationUser());
            panelSlider.Controls.Add(new FaceTaken());
            panelSlider.Controls.Add(new PhoneOTP());
            panelSlider.Controls.Add(new FullName());
        }
    }
}