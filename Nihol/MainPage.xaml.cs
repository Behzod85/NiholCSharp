using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using static ZXing.Mobile.MobileBarcodeScanningOptions;

namespace Nihol
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPageViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainPageViewModel();
            BindingContext = viewModel;
            var f = new List<BarcodeFormat>();
            f.Add(BarcodeFormat.QR_CODE);
            _scanView.Options.PossibleFormats = f;
            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    _scanView.Options.CameraResolutionSelector = new CameraResolutionSelectorDelegate(SelectLowestResolutionMatchingDisplayAspectRatio);
            //}
            overlay.ShowFlashButton = false;
        }

        public void Handle_OnScanResult(Result result)
        {
            viewModel.OnScanResult(result);
            //_scanView.ScanResultCommand.
            _scanView.IsAnalyzing = false;
            _scanView.IsScanning = false;
            myImage.IsVisible = false;
            Device.BeginInvokeOnMainThread(() =>
            {


                if (viewModel.CanGoToMP)
                {
                    MyImageConditionIdle.IsVisible = false;
                    MyImageConditionSad.IsVisible = false;
                    MyImageConditionSmile.IsVisible = true;

                }
                else
                {
                    MyImageConditionIdle.IsVisible = false;
                    MyImageConditionSad.IsVisible = true;
                    MyImageConditionSmile.IsVisible = false;
                }
            });

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.CanGoToMP = false;
            _scanView.IsAnalyzing = true;
            _scanView.IsScanning = true;
            myImage.IsVisible = false;
            MyImageConditionIdle.IsVisible = true;
            MyImageConditionSad.IsVisible = false;
            MyImageConditionSmile.IsVisible = false;
        }
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            OnAppearing();
            //Device.BeginInvokeOnMainThread(() => { 
            //    viewModel.CanGoToMP = false;
            //    _scanView.IsAnalyzing = true;
            //    _scanView.IsScanning = true;
            //    myImage.IsVisible = false;
            //    MyImageConditionIdle.IsVisible = true;
            //    MyImageConditionSad.IsVisible = false;
            //    MyImageConditionSmile.IsVisible = false;
            //});
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var b = (Button)sender;
            if (viewModel.ButtonControl)
            {
                viewModel.ButtonControl = false;
                _scanView.IsAnalyzing = false;
                _scanView.IsScanning = false;
                myImage.IsVisible = true;
                b.Text = "< Back";
            }
            else
            {
                viewModel.ButtonControl = true;
                _scanView.IsAnalyzing = true;
                _scanView.IsScanning = true;
                myImage.IsVisible = false;
                b.Text = "Where to get QRcode?";
            }
            
        }

        public CameraResolution SelectLowestResolutionMatchingDisplayAspectRatio(List<CameraResolution> availableResolutions)
        {
            CameraResolution result = null;
            //a tolerance of 0.1 should not be recognizable for users
            double aspectTolerance = 0.1;
            //calculating our targetRatio
            var targetRatio = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Width;
            var targetHeight = DeviceDisplay.MainDisplayInfo.Height;
            var minDiff = double.MaxValue;
            //camera API lists all available resolutions from highest to lowest, perfect for us
            //making use of this sorting, following code runs some comparisons to select the lowest resolution that matches the screen aspect ratio
            //selecting the lowest makes QR detection actual faster most of the time
            foreach (var r in availableResolutions)
            {
                //if current ratio is bigger than our tolerance, move on
                //camera resolution is provided landscape ...
                if (Math.Abs(((double)r.Width / r.Height) - targetRatio) > aspectTolerance)
                    continue;
                else
                    if (Math.Abs(r.Height - targetHeight) < minDiff)
                    minDiff = Math.Abs(r.Height - targetHeight);
                result = r;
            }
            return result;
        }
    }
}
