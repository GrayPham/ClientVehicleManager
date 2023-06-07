using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Connect.RemoteDataProvider.Common
{
    public partial class WindowsAPI
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetSystemMetrics(int smIndex);
    }
    public class ServiceHelpers
    {
        public const int AmountRequestList = 5;
        public const int LoginUnsupportFunctionCode = 8;
        public static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch (Exception)
            {

            }

            return "";
        }
        public static string GetPlatformWindow()
        {
            string _result = "";
            try
            {
                Version vs = Environment.OSVersion.Version;
                bool isServer = IsServerVersion();
                switch (vs.Major)
                {
                    case 3:
                        _result = "Windows NT 3.51";
                        break;
                    case 4:
                        _result = "Windows NT 4.0";
                        break;
                    case 5:
                        if (vs.Minor == 0)
                            _result = "Windows 2000";
                        else if (vs.Minor == 1)
                            _result = "Windows XP";
                        else
                        {
                            if (isServer)
                            {
                                if (WindowsAPI.GetSystemMetrics(89) == 0)
                                    _result = "Windows Server 2003";
                                else
                                    _result = "Windows Server 2003 R2";
                            }
                            else
                                _result = "Windows XP";
                        }
                        break;
                    case 6:
                        if (vs.Minor == 0)
                        {
                            if (isServer)
                                _result = "Windows Server 2008";
                            else
                                _result = "Windows Vista";
                        }
                        else if (vs.Minor == 1)
                        {
                            if (isServer)
                                _result = "Windows Server 2008 R2";
                            else
                                _result = "Windows 7";
                        }
                        else if (vs.Minor == 2)
                            _result = "Windows 8";
                        else
                        {
                            if (isServer)
                                _result = "Windows Server 2012 R2";
                            else
                                _result = "Windows 8.1";
                        }
                        break;
                }
            }
            catch (Exception)
            {

            }

            return _result;
        }
        public static bool IsServerVersion()
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                foreach (ManagementObject managementObject in searcher.Get())
                {
                    // ProductType will be one of:
                    // 1: Workstation
                    // 2: Domain Controller
                    // 3: Server
                    uint productType = (uint)managementObject.GetPropertyValue("ProductType");
                    return productType != 1;
                }
            }
            return false;
        }

    }
}
