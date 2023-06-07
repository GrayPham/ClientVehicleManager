using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Common.ViewModels
{
    public class GetSoundsVm
    {
        public int soundNo { get; set; }
        public string soundType { get; set; }
        public byte[] source { get; set; }

        public string extension { get; set; }
    }
}
