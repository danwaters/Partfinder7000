using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Partfinder7000.Services;
using Xamarin.Forms;

namespace Partfinder7000.ViewModels
{
    public class SearchResultsViewModel
    {
        public ObservableCollection<SearchResultViewModel> SearchResults { get; set; }

        public SearchResultsViewModel(SearchResultList results)
        {
            SearchResults = new ObservableCollection<SearchResultViewModel>();
            foreach(var r in results.Results)
            {
                var excerpt = GetFormattedExcerpt(r.Content, results.OriginalQuery);

                SearchResults.Add(new SearchResultViewModel { Excerpt = excerpt, StorageUrl = r.StoragePath});
            }
        }

        private FormattedString GetFormattedExcerpt(string excerpt, string query)
        {
            excerpt = excerpt.Replace("\r", "").Replace("\n", "").Replace("\t", "");
            var firstIndex = excerpt.IndexOf(query);
            excerpt = excerpt.Substring(firstIndex);

            var fs = new FormattedString();
            fs.Spans.Add(new Span { Text = "... " });
            fs.Spans.Add(new Span { Text = excerpt.Substring(0, query.Length), FontAttributes = FontAttributes.Bold, ForegroundColor=Color.Blue});
            fs.Spans.Add(new Span { Text = " " + excerpt.Substring(query.Length) } );
            return fs;
        }
    }

    public class SearchResultViewModel 
    {
        public FormattedString Excerpt { get; set; }
        public string StorageUrl { get; set; }

        public SearchResultViewModel()
        {
            
        }
    }
}
