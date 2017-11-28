
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using UIKit;

namespace Partfinder7000.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            // Code for starting up the Xamarin Test Cloud Agent
#if DEBUG
			Xamarin.Calabash.Start();
#endif

            AppCenter.Start("ios=1198b035-8664-418b-aff8-d69d35327d93;" + "uwp={Your UWP App secret here};" +
                   "android={Your Android App secret here}",
                   typeof(Analytics), typeof(Crashes));

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        [Export("EnableImageMocking:")]
        public NSString EnableImageMocking(NSString s)
        {
            App.MockCamera = true;
            return NSString.Empty;
        }

        [Export("DisableImageMocking:")]
        public NSString DisableImageMocking(NSString s)
        {
            App.MockCamera = true;
            return NSString.Empty;
        }
    }
}
