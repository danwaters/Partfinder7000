using System;
using System.Collections.Generic;
using Partfinder7000.BusinessLogic;
using Partfinder7000.ViewModels;
using Xamarin.Forms;
using System.Linq;
using System.IO;

namespace Partfinder7000.Pages
{
    public partial class ResultPage : ContentPage
    {
        private IdentificationViewModel model;

        public ResultPage()
        {
            InitializeComponent();
        }

        public ResultPage(IdentificationViewModel model)
            : this()
        {
            this.model = model;
            lblResult.Text = "Predicting ...";
        }

        protected override async void OnAppearing()
        {
            this.image.Source = ImageSource.FromStream(() => new MemoryStream(model.ImageData));
            var result = await model.IdentifyImage();
            lblDone.IsVisible = true;

            var url = ProductSearcher.GetSearchUrlForProduct(result);

            var webView = new WebView
            {
                Source = new UrlWebViewSource { Url = url }
            };

            this.Content = webView;
            //await Navigation.PushAsync(new ContentPage { Content = webView });
        }
    }
}
