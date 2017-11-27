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
        public void CanViewCameraScreen()
        {
            if (TestEnvironment.Platform != TestPlatform.Local)
            {
                AppResult[] results = app.WaitForElement("OK");
                Assert.IsTrue(results.Any());
                app.Screenshot("Camera Prompt");
                app.Tap("OK");
            }

            app.WaitForElement(e => e.Id("takePhotoButton"));
            app.Screenshot("Camera View");
            app.Tap(e => e.Id("takePhotoButton"));
            app.WaitForElement("resultPageLabel");
            app.Screenshot("On the Results page");
        }
    }
}
