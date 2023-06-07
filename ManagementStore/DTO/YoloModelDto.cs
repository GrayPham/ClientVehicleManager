using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yolov5Net.Scorer;

namespace ManagementStore.DTO
{
    public class YoloModelDto:IDisposable
    {
        private Image _ImageDetect;
        private List<YoloPrediction> _predictions;
        private Image _ImagwBase;
        public YoloModelDto()
        {

        }
        public YoloModelDto(Image ImagwBase, List<YoloPrediction> predictions)
        {
            _ImagwBase = ImagwBase;
            _predictions = predictions;
        }
        public List<YoloPrediction> yoloPredictions { get { return _predictions; } }
        public YoloModelDto(Image ImageDetect, List<YoloPrediction> Predictions, Image ImagwBase)
        {
            _ImageDetect = ImageDetect;
            _predictions = Predictions;
            _ImagwBase = ImagwBase;
        }
        public Image getImageDetect()
        {
            return _ImageDetect;
        }
        public void setImagwBase(Image ImageBase)
        {
            _ImagwBase = ImageBase;
        }
        public void setImageDetect(Image ImageDetect)
        {
            _ImageDetect = ImageDetect;
        }
        public Image getImageBase()
        {
            return _ImagwBase;
        }
        public int countListPrediction()
        {
            return _predictions.Count;
        }
        public void setDataPredict(Image ImagwBase, List<YoloPrediction> predictions)
        {
            _ImagwBase = ImagwBase;
            _predictions = predictions;
        }

        public void Dispose()
        {
            if (_ImageDetect != null)
            {
                _ImageDetect.Dispose();
            }
            if (_ImagwBase != null)
            {
                _ImagwBase.Dispose();
            }
        }

        ~YoloModelDto()
        {

        }
    }
}
