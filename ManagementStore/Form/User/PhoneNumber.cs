using ManagementStore.Extensions;
using System;
using System.Collections.Generic;

namespace ManagementStore.Form.User
{
    public partial class PhoneNumber : System.Windows.Forms.UserControl
    {
        public List<string> Num;
        public PhoneNumber()
        {
            Num = new List<String>();
            InitializeComponent();
        }
        private void PhoneNumber_Load(object sender, EventArgs e)
        {
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
             Utils.Forward(ParentForm, "pictureBoxPhone", "pictureBoxOTP", "PhoneOTP");
        }

        private void btnNum1_Click(object sender, EventArgs e)
        {
            if(Num.Count!=10) Num.Add("1");
            DisplayPhoneNumber();
        }

        private void btnNum2_Click(object sender, EventArgs e)
        {
            if (Num.Count != 10) Num.Add("2");
            DisplayPhoneNumber();

        }

        private void btnNum3_Click(object sender, EventArgs e)
        {
            if (Num.Count != 10) Num.Add("3");
            DisplayPhoneNumber();

        }

        private void btnNum4_Click(object sender, EventArgs e)
        {
            if (Num.Count != 10) Num.Add("4");
            DisplayPhoneNumber();

        }

        private void btnNum5_Click(object sender, EventArgs e)
        {
            if (Num.Count != 10) Num.Add("5");
            DisplayPhoneNumber();

        }

        private void btnNum6_Click(object sender, EventArgs e)
        {
            if (Num.Count != 10) Num.Add("6");
            DisplayPhoneNumber();

        }

        private void btnNum7_Click(object sender, EventArgs e)
        {
            if (Num.Count != 10) Num.Add("7");
            DisplayPhoneNumber();

        }

        private void btnNum8_Click(object sender, EventArgs e)
        {
            if (Num.Count != 10) Num.Add("8");
            DisplayPhoneNumber();

        }

        private void btnNum9_Click(object sender, EventArgs e)
        {
            if (Num.Count != 10) Num.Add("9");
            DisplayPhoneNumber();

        }

        private void btnNum0_Click(object sender, EventArgs e)
        {
            if (Num.Count != 10) Num.Add("0");
            DisplayPhoneNumber();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if(Num.Count > 0)
            {
                Num.RemoveAt(Num.Count - 1);
            }
            else
            {
                phoneTxt.Text = "";
            }
            DisplayPhoneNumber();

        }

        private void btnAC_Click(object sender, EventArgs e)
        {
            Num = new List<string>();
            DisplayPhoneNumber();
        }
        private void DisplayPhoneNumber()
        {
            phoneTxt.Text = string.Join("", Num.ToArray());
        }


    }
}
