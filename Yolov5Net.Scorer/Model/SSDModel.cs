using System;
using System.Collections.Generic;
using System.Text;
using Yolov5Net.Scorer.Models.Abstract;

namespace Yolov5Net.Scorer.Models
{
    public class SSDModel : YoloModel
    {
        public override int Width { get; set; } = 300;
        public override int Height { get; set; } = 300;
        public override int Depth { get; set; } = 3;

        public override int Dimensions { get; set; } = 85;

        public override int[] Strides { get; set; } = new int[] { 8, 16, 32 };

        public override int[][][] Anchors { get; set; } = new int[][][]
        {
            new int[][] { new int[] { 010, 13 }, new int[] { 016, 030 }, new int[] { 033, 023 } },
            new int[][] { new int[] { 030, 61 }, new int[] { 062, 045 }, new int[] { 059, 119 } },
            new int[][] { new int[] { 116, 90 }, new int[] { 156, 198 }, new int[] { 373, 326 } }
        };

        public override int[] Shapes { get; set; } = new int[] { 80, 40, 20 };

        public override float Confidence { get; set; } = 0.20f;
        public override float MulConfidence { get; set; } = 0.25f;
        public override float Overlap { get; set; } = 0.45f;

        public override string[] Outputs { get; set; } = new[] { "output" };

        public override List<YoloLabel> Labels { get; set; } = new List<YoloLabel>()
        {
            new YoloLabel { Id = 1, Name = "license-plate" },
            new YoloLabel { Id = 2, Name = "moto" },
            new YoloLabel { Id = 3, Name = "number" },
            new YoloLabel { Id = 4, Name = "oto" }
        };
        public override bool UseDetect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
