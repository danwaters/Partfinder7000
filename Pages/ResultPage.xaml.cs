using System;
using System.Collections.Generic;
using Partfinder7000.ViewModels;
using Xamarin.Forms;

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
            var result = await model.IdentifyImage();
            lblResult.Text = result.Predictions[0].Tag;
            lblDone.IsVisible = true;
        }
    }
}
