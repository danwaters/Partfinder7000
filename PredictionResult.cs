using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Partfinder7000
{
    [JsonObject]
    public class PredictionResult
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Created { get; set; }

        public List<Prediction> Predictions { get; set; }

        public PredictionResult()
        {
            
        }
    }

    [JsonObject]
    public class Prediction
    {
        public string TagId { get; set; }
        public string Tag { get; set; }
        public double Probability { get; set; }

        public Prediction()
        {
            
        }
    }
}
