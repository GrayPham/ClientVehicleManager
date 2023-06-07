using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.VehicleCheckHttpClient.Models
{
    public class TrackReportRequest
    {
        private string platenum { get; set; }
        private string typeTransport { get; set; }
        private string typeLP { get; set; }
        private string ImagePlate { get; set; }
        private string ImageFace { get; set; }

    }
}
