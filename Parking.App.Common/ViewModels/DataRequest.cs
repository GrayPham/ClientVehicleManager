using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Common.ViewModels
{
    public class DataRequest
    {
        public UInt32 Signature { get; set; }
        public int FrameID { get; set; }
        public UInt16 FunctionCode { get; set; }
        public UInt32 DataLength { get; set; }
        public object Data { get; set; }
    }
}
