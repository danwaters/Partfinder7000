using System.Linq;
using System.Collections.Generic;

namespace Partfinder7000.BusinessLogic
{
    public static class ProductSearcher
    {
        private static Dictionary<string, string> productSearches = new Dictionary<string, string>()
        {
            ["100RED"]="red AWG wire spool",
            ["100GRN"]="green AWG wire spool",
            ["200RED"]="precision screwdriver set",
            ["300SLV"]="helping hands tool"
        };

        public static string GetSearchUrlForProduct(PredictionResult result)
        {
            string query = "42";
            if (result.Predictions != null)
            {
                var predictions = result.Predictions
                                        .Where(p => p.Probability >= 0.9)
                                        .OrderByDescending(p => p.Probability);

                foreach (var key in productSearches.Keys)
                {
                    if (predictions.Any(p => p.Tag == key))
                    {
                        query = productSearches[key];
                        break;
                    }
                }
            }

            query = System.Net.WebUtility.UrlEncode(query);
            return $"https://www.bing.com/search?q={query}";
        }
    }
}
