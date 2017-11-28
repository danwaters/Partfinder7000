using System.Threading.Tasks;
using Partfinder7000.Services;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Partfinder7000.ViewModels
{
    public class IdentificationViewModel
    {
        private byte[] originalImageData;
        public List<PredictionResult> Predictions { get; set; }

        public IdentificationViewModel()
        {
            Predictions = new List<PredictionResult>();
        }

        public IdentificationViewModel(byte[] imageData)
        {
            originalImageData = imageData;
        }

        public async Task<PredictionResult> IdentifyImage()
        {
            var service = new VisionPredictionService();
            var result = await service.Predict(originalImageData);
            return result;
        }
    }
}
