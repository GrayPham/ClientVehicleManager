using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Common.ViewModels 
{
    public class OcrDataRequest
    {
        public string lang { get; set; }
        public string requestId { get; set; }
        public long timestamp { get; set; }
        public string resultType { get; set; }
        public string version { get; set; }
        public List<OcrImageData> images { get; set; }




       
    }
}
