using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectShowLib;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
namespace ManagementStore.Common
{
    public class CameraCommon
    {
        //public readonly static DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
        //public readonly static List<string> cameraList = devices.Select(device => device.Name).ToList();
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

    }
}
