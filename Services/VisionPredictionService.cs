using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Partfinder7000.ViewModels;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Partfinder7000.Services
{
    public class VisionPredictionService
    {
        public async Task<PredictionResult> Predict(byte[] image)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid subscription key.
            client.DefaultRequestHeaders.Add("Prediction-Key", Keys.PredictionApiKey);

            // Prediction URL - replace this example URL with your valid prediction URL.
            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/06c1c15a-2ecd-4e20-9ab2-37a789080137/image";

            HttpResponseMessage response;

            using (var content = new ByteArrayContent(image))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var predictionResult = JsonConvert.DeserializeObject<PredictionResult>(responseString);
                return predictionResult;
            }
        }
    }
}
