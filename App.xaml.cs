using Xamarin.Forms;
using Partfinder7000.Pages;

namespace Partfinder7000
{
    public partial class App : Application
    {
        public static bool MockCamera { get; set; }

        public App()
        {
            MockCamera = false;
            InitializeComponent();

            MainPage = new NavigationPage(new CameraPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
