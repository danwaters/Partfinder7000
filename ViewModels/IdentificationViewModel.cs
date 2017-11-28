using System.Threading.Tasks;
using Partfinder7000.Services;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Partfinder7000.ViewModels
{
    public class IdentificationViewModel
    {
        public byte[] ImageData;
        public List<PredictionResult> Predictions { get; set; }

        public Image Image { get; private set; }

        public IdentificationViewModel()
        {
            
        }

        public IdentificationViewModel(byte[] imageData)
        {
            ImageData = imageData;
            Image = new Image
            {
                Source = ImageSource.FromStream(() => new MemoryStream(imageData) { Position = 0 })
            };
        }

        public async Task<PredictionResult> IdentifyImage()
        {
            Predictions = new List<PredictionResult>();
            var service = new VisionPredictionService();
            var result = await service.Predict(ImageData);
            return result;
        }
    }
}
