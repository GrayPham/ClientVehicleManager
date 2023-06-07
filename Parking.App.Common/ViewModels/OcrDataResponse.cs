using Parking.App.Common.ViewModels;
using System.Collections.Generic;

namespace Parking.App.Common.ViewModels
{
    public class OcrDataResponse
    {
        public string version { get; set; }
        public string requestId { get; set; }
        public long timestamp { get; set; }
        public List<imageReponse> images { get; set; }


    }
    public class imageReponse
    {
        public string uid { get; set; }
        public string name { get; set; }
        public string inferResult { get; set; }
        public string message { get; set; }
        public List<OcrField> fields { get; set; }
        public validationResult validationResult { get; set; }

    }

    public class validationResult
    {
        public string result { get; set; }
    }




    
}
