using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Common.ViewModels
{
    public class RequestUserAddInfo
    {
        public int CommType { get; set; }
        public int reqCommand { get; set; }
        public string KEY { get; set; }
        public string SERIAL_NUMBER { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string START_TIME { get; set; }
        public string END_TIME { get; set; }
        public Byte[] FACE_DATA { get; set; }
    }
}
