using ManagementStore.Form.Camera;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using DirectShowLib;
namespace ManagementStore.Common
{
    public class ModelConfig
    {
        public static string dataFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Weights");
        public static string constImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", ""), "Assets/Images/hcmute.jpg");
        public static string socketFastAPI = "ws://localhost:8005/ws";
        public static bool socketOpen = false;
        public readonly static string checkInSuccess = "Successful";
        public readonly static string checkInError = "Error";
        public readonly static string cardEthernet = "Ethernet";
        public static List<FaceCameraControl> listFaceCamera = new List<FaceCameraControl>();
        //Get the list of network cards in the computer
        public static NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        public readonly static DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
        public readonly static List<string> cameraList = devices.Select(device => device.Name).Concat(new List<string> { "OFF" }).ToList();
        
        // Get Mac adress 
        public static string getMacAddress()
        {
            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    if(networkInterface.Name == cardEthernet)
                    {
                        return networkInterface.GetPhysicalAddress().ToString();
                    }
                }
            }
            return "";
        }
    }
}
