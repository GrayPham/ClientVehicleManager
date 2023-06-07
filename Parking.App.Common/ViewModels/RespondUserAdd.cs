using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Common.ViewModels
{
    public class RespondUserAdd
    {
        public int CommType { get; set; }
        public int respCommand { get; set; }
        public string SERIAL_NUMBER { get; set; }
        public string MESSAGE { get; set; }
        public bool SUCCESS { get; set; }
    }
}
