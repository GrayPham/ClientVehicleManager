using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Yolov5Net.Scorer.Extensions
{
    public static class RectangleExtensions
    {
        public static float Area(this RectangleF source)
        {
            return source.Width * source.Height;
        }
    }
}
