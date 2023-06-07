using System;
using System.Collections.Generic;
using System.Text;

namespace Parking.App.Common.ViewModels
{
    public class OcrField
    {
        public string valueType { get; set; }
        public boundingPoly boundingPoly { get; set; }
        public string inferText { get; set; }
        public double inferConfidence { get; set; }
    }
    public class boundingPoly
    {
        public List<vertice> vertices { get; set; }
    }

    public class vertice
    {
        public double x { get; set; }
        public double y { get; set; }

    }
}
