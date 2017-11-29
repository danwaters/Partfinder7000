using Microsoft.AppCenter.Analytics;
using Partfinder7000.Services;
using Partfinder7000.ViewModels;
using Xamarin.Forms;

namespace Partfinder7000.Pages
{
    public partial class EnterpriseSearchResultPage : ContentPage
    {
        public EnterpriseSearchResultPage(SearchResultList results)
        {
            InitializeComponent();

            var model = new SearchResultsViewModel(results);
            this.lblOriginalQuery.Text = $"Your search: {results.OriginalQuery}";
            this.BindingContext = model;
        }

        public EnterpriseSearchResultPage()
        {
            InitializeComponent();

            SearchResultList sampleList = new SearchResultList
            {
                Results = new System.Collections.Generic.List<SearchResult>()
                {
                    new SearchResult() { Score = 0.9, Content="Test search item 1", StoragePath="https://www.google.com"},
                    new SearchResult() { Score = 0.7, Content="Test search item 2", StoragePath="https://www.bing.com"}
                },
                OriginalQuery = "search"
            };
            this.lblOriginalQuery.Text = sampleList.OriginalQuery;

            var sampleViewModel = new SearchResultsViewModel(sampleList);
            this.BindingContext = sampleViewModel;
        }

        async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            Analytics.TrackEvent(Events.DocumentSearchHit.ToString());
            var item = (SearchResultViewModel)e.Item;
            var url = item.StorageUrl;

            var webView = new WebView { Source = new UrlWebViewSource { Url = url } };
            await Navigation.PushAsync(new ContentPage { Content = webView });
        }
    }
}
