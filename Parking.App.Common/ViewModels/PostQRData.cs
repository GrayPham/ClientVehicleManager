using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Common.ViewModels
{
    public class PostQRData
    {
        public string cmd { get; set; }
        public string mode { get; set; }
        public string svcCode { get; set; }
        public string branchName { get; set; }
        public string deviceId { get; set; }
    }
}
