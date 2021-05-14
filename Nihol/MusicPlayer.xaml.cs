using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nihol
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicPlayer : ContentPage
    {
        Charts urhoApp;
        public MusicPlayerViewModel musicPlayer;
        public string BookNumber;
        int nextPage = 2;
        public MusicPlayer()
        {
            InitializeComponent();
            
        }
        protected override void OnAppearing()
        {
            StartUrhoApp();
        }

        async void StartUrhoApp()
        {
            if (BookNumber == "5" || BookNumber == "6" || BookNumber == "7" || BookNumber == "8" || BookNumber == "9")
            {
                MyImageFrame.IsVisible = true;
                MyUrhoFrame.IsVisible = false;
                MyIMG.Source = ImageSource.FromResource("Nihol.UIImages.tosura0" + BookNumber + ".jpg");
                return;
            }
            MyUrhoFrame.IsVisible = true;
            MyImageFrame.IsVisible = false;
            //urhoApp.BookNumber = BookNumber;
            urhoApp = await MyUrhoSurface.Show<Charts>(new ApplicationOptions(assetsFolder: "Data") { Orientation = ApplicationOptions.OrientationType.LandscapeAndPortrait });

                urhoApp.BookNumber = BookNumber;
                //urhoApp.Start2();
                Urho.Application.InvokeOnMain(() => urhoApp.Start2());

        }
        protected override void OnDisappearing()
        {
            UrhoSurface.OnDestroy();
            base.OnDisappearing();
        }

        //private void ImageButton_Clicked(object sender, EventArgs e)
        //{
        //    Urho.Application.InvokeOnMain(() => urhoApp.OpenPage());
        //}

        //private void ImageButton_Clicked_1(object sender, EventArgs e)
        //{
        //    Urho.Application.InvokeOnMain(() => urhoApp.ClosePage());
        //}

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //int val = (int)Math.Round(SSS.Value);
            //System.Console.WriteLine($"My val is {val}");
            //Urho.Application.InvokeOnMain(() => urhoApp.ToPage(val));
            //if (SSS.Value != musicPlayer.MyDuration)
            //    musicPlayer.Seek(SSS.Value);
            if (BookNumber == "5" || BookNumber == "6" || BookNumber == "7" || BookNumber == "8" || BookNumber == "9")
                return;
            var p = (musicPlayer.MyCurrentDuration + 0.1) * 4.0 / musicPlayer.MyDuration;
                int page = (int)Math.Ceiling(p);
                if (page > 4) page = 4;
                if (page == nextPage)
                {
                    nextPage = page + 1;
                    Urho.Application.InvokeOnMain(() => urhoApp.OpenPage());
                }
                if (musicPlayer.MyCurrentDuration == 0)
                {
                    Urho.Application.InvokeOnMain(() => urhoApp.ToPage(1));
                    nextPage = 2;
                }
            //if (p == 0) Urho.Application.InvokeOnMain(() => urhoApp.ToPage(1));
        }

        private void SSS_DragCompleted(object sender, EventArgs e)
        {
            
            if (SSS.Value != musicPlayer.MyDuration)
            {
                //musicPlayer.MyCurrentDuration = SSS.Value;
                musicPlayer.Seek(SSS.Value);
            }
            if (BookNumber == "5" || BookNumber == "6" || BookNumber == "7" || BookNumber == "8" || BookNumber == "9")
                return;
            var p = (musicPlayer.MyCurrentDuration + 0.1) * 4.0 / musicPlayer.MyDuration;
                int page = (int)Math.Ceiling(p);
                if (page > 4) page = 4;
                Urho.Application.InvokeOnMain(() => urhoApp.ToPage(page));
                nextPage = page + 1;
        }

        private void SSS_DragStarted(object sender, EventArgs e)
        {
            musicPlayer.Dragging = true;
        }

        //bool UpdatePosition()
        //{
        //    SSS.ValueChanged -= Slider_ValueChanged;
        //    musicPlayer.MyCurrentDuration;
        //    SSS.ValueChanged += Slider_ValueChanged;

        //    return musicPlayer.MusicIsPlaying;
        //}
    }
}