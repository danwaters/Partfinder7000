using System;
using CoreGraphics;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using AVFoundation;
using Foundation;
using UIKit;
using Partfinder7000.ViewModels;
using Partfinder7000.Pages;
using Microsoft.AppCenter.Analytics;

/*
 * AVFoundation Reference: http://red-glasses.com/index.php/tutorials/ios4-take-photos-with-live-video-preview-using-avfoundation/
 * Additional Camera Settings Reference: http://stackoverflow.com/questions/4550271/avfoundation-images-coming-in-unusably-dark
 * Custom Renderers: http://blog.xamarin.com/using-custom-uiviewcontrollers-in-xamarin.forms-on-ios/
 * Code mostly lifted from: https://github.com/pierceboggan/Moments/blob/master/Moments%20-%20XAML/Moments.iOS/Pages/CameraPage.cs
 */

[assembly: ExportRenderer(typeof(Partfinder7000.Pages.CameraPage), typeof(Partfinder7000.iOS.CameraPage))]
namespace Partfinder7000.iOS
{
    public class CameraPage : PageRenderer
    {
        AVCaptureSession captureSession;
        AVCaptureDeviceInput captureDeviceInput;
        UIButton toggleCameraButton;
        UIButton toggleFlashButton;
        UIView liveCameraStream;
        AVCaptureStillImageOutput stillImageOutput;
        UIButton takePhotoButton;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupUi();
            SetupEventHandlers();

            AuthorizeCameraUse();
            SetupLiveCameraStream();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        public async void AuthorizeCameraUse()
        {
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            }
        }

        public void SetupLiveCameraStream()
        {
            captureSession = new AVCaptureSession();

            var viewLayer = liveCameraStream.Layer;
            var videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
            {
                Frame = liveCameraStream.Bounds
            };
            liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

            var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
            ConfigureCameraForDevice(captureDevice);
            captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);

            var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
            stillImageOutput = new AVCaptureStillImageOutput()
            {
                OutputSettings = new NSDictionary()
            };

            captureSession.AddOutput(stillImageOutput);
            captureSession.AddInput(captureDeviceInput);
            captureSession.StartRunning();
        }

        public async void CapturePhoto()
        {
            var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);

            var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);

            SendPhoto(jpegImageAsNsData.ToArray());
        }

        public void ToggleFrontBackCamera()
        {
            var devicePosition = captureDeviceInput.Device.Position;
            if (devicePosition == AVCaptureDevicePosition.Front)
            {
                devicePosition = AVCaptureDevicePosition.Back;
            }
            else
            {
                devicePosition = AVCaptureDevicePosition.Front;
            }

            var device = GetCameraForOrientation(devicePosition);
            ConfigureCameraForDevice(device);

            captureSession.BeginConfiguration();
            captureSession.RemoveInput(captureDeviceInput);
            captureDeviceInput = AVCaptureDeviceInput.FromDevice(device);
            captureSession.AddInput(captureDeviceInput);
            captureSession.CommitConfiguration();
        }

        public void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            var error = new NSError();
            if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
            {
                device.LockForConfiguration(out error);
                device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                device.UnlockForConfiguration();
            }
            else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
            {
                device.LockForConfiguration(out error);
                device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                device.UnlockForConfiguration();
            }
            else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
            {
                device.LockForConfiguration(out error);
                device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                device.UnlockForConfiguration();
            }
        }

        public void ToggleFlash()
        {
            var device = captureDeviceInput.Device;

            var error = new NSError();
            if (device.HasFlash)
            {
                if (device.FlashMode == AVCaptureFlashMode.On)
                {
                    device.LockForConfiguration(out error);
                    device.FlashMode = AVCaptureFlashMode.Off;
                    device.UnlockForConfiguration();

                    toggleFlashButton.SetBackgroundImage(UIImage.FromFile("NoFlashButton.png"), UIControlState.Normal);
                }
                else
                {
                    device.LockForConfiguration(out error);
                    device.FlashMode = AVCaptureFlashMode.On;
                    device.UnlockForConfiguration();

                    toggleFlashButton.SetBackgroundImage(UIImage.FromFile("FlashButton.png"), UIControlState.Normal);
                }
            }
        }

        public AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
        {
            var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

            foreach (var device in devices)
            {
                if (device.Position == orientation)
                {
                    return device;
                }
            }

            return null;
        }

        private void SetupUi()
        {
            var centerButtonX = View.Bounds.GetMidX();
            var topLeftX = View.Bounds.X;
            var topRightX = View.Bounds.Right;
            var bottomButtonY = View.Bounds.Bottom - 200;
            var topButtonY = View.Bounds.Top + 15;
            var buttonWidth = 150;
            var buttonHeight = 60;

            liveCameraStream = new UIView()
            {
                Frame = new CGRect(0f, 0f, View.Bounds.Width, View.Bounds.Height)
            };

            takePhotoButton = new UIButton()
            {
                Frame = new CGRect(centerButtonX - (buttonWidth / 2), bottomButtonY, buttonWidth, buttonHeight)
            };
            takePhotoButton.SetTitle("Identify", UIControlState.Normal);
            takePhotoButton.BackgroundColor = UIColor.FromRGB(22, 22, 108);
            takePhotoButton.Layer.CornerRadius = 10;
            takePhotoButton.Layer.BorderColor = UIColor.White.CGColor;
            takePhotoButton.Layer.BorderWidth = 1;
            takePhotoButton.AccessibilityIdentifier = "takePhotoButton";

            View.Add(liveCameraStream);
            View.Add(takePhotoButton);
        }

        private void SetupEventHandlers()
        {
            takePhotoButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                CapturePhoto();
                Analytics.TrackEvent(Events.PhotoCaptured.ToString());
            };
        }

        public async void SendPhoto(byte[] image)
        {
            IdentificationViewModel model;

            if (App.MockCamera)
            {
                var testGreenImage = UIImage.FromBundle("greenSampleSm.png");
                var imageData = testGreenImage.AsPNG().ToArray();
                model = new IdentificationViewModel(imageData);
            }
            else
            {
                model = new IdentificationViewModel(image);
            }

            await App.Current.MainPage.Navigation.PushAsync(new ResultPage(model));
        }
    }
}