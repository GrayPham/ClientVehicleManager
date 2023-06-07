using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.VehicleCheckHttpClient.Interface
{
    public interface IVehicleManagement
    {
        Task<string> CheckInVehicleAsync(string platenum,  Image faceImage, Image lpImag, string typeTransport, string typeLP);
    }
}
