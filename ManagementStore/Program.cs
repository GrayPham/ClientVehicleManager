﻿using DevExpress.UserSkins;
using ManagementStore.Form;
using System;
using System.Windows.Forms;

namespace ManagementStore
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            Application.Run(new DetectClient());
        }
    }
}
