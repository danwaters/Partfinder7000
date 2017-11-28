using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Partfinder7000.UITests
{
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void CanIdentifyMockImage()
        {
            app.WaitForElement(e => e.Id("takePhotoButton"));
            app.Screenshot("Camera View");

            app.Invoke("EnableImageMocking:", "");

            app.Tap(e => e.Id("takePhotoButton"));
            app.WaitForElement("resultPageLabel");
            app.Screenshot("On the Results page");
            app.WaitForElement(e => e.WebView());
            app.Screenshot("Search results loading");
            app.WaitForElement(e => e.WebView().Css(".b_searchbox"));
            app.Screenshot("Search results are displayed");
        }
    }
}
