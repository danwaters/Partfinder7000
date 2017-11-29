using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Partfinder7000.Services
{
    public class EnterpriseSearchService
    {
        public async Task<SearchResultList> Search(string query)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("api-key", Keys.AppQueryKey);
            query = System.Net.WebUtility.UrlEncode(query);
            var searchUrl = $"{Keys.SearchEndpoint}&search={query}";

            var searchResultsJson = await client.GetStringAsync(searchUrl);
            var searchResults = JsonConvert.DeserializeObject<SearchResultList>(searchResultsJson);
            searchResults.OriginalQuery = query;
            return searchResults;
        }
    }

    public class SearchResultList
    {
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonProperty("value")]
        public List<SearchResult> Results { get; set; }



        [JsonIgnore]
        public string OriginalQuery { get; set; }
    }

    public class SearchResult
    {
        [JsonProperty("@search.score")]
        public double Score { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("metadata_storage_path")]
        public string StoragePath { get; set; }

        public SearchResult()
        {
            
        }
    }
}
