using System.Threading.Tasks;
namespace Partfinder7000.ViewModels
{
    public class IdentificationViewModel
    {
        private byte[] originalImageData;

        public IdentificationViewModel()
        {
        }

        public IdentificationViewModel(byte[] imageData)
        {
            originalImageData = imageData;
        }

        public async Task<string> IdentifyImage()
        {
            return "blah";
        }
    }
}
