using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.VehicleCheckHttpClient.Models
{
    public class TrackReportRespose
    {
        private bool Status { get; }
        public TrackReportRespose(bool _status)
        {
            Status = _status;
        }
        public bool getStatus()
        {
            return Status;
        }
    }
}
