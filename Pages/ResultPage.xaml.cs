using System;
using System.Collections.Generic;
using Partfinder7000.ViewModels;
using Xamarin.Forms;

namespace Partfinder7000.Pages
{
    public partial class ResultPage : ContentPage
    {
        private byte[] image;
        private IdentificationViewModel model;

        public ResultPage()
        {
            InitializeComponent();
        }

        public ResultPage(IdentificationViewModel model)
            : this()
        {
            this.model = model;
        }

        protected override async void OnAppearing()
        {
            await model.IdentifyImage();
        }
    }
}
