using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing;

namespace Nihol
{
    public class MainPageViewModel: ReactiveObject
    {
        #region Properties

        public string MyFileName { get; set; }
        private bool _canGoToMP;

        public bool CanGoToMP
        {
            get { return _canGoToMP; }
            set { this.RaiseAndSetIfChanged(ref _canGoToMP, value); }
        }
        

        private List<string> _ExistingBooks = new List<string>() {"ToSuRaEn01", "ToSuRaEn02", "ToSuRaEn03", "ToSuRaEn04", "ToSuRaUz01", "ToSuRaUz02", "ToSuRaUz03", "ToSuRaUz04", "ToSuRaRu01", "ToSuRaRu02", "ToSuRaRu03", "ToSuRaRu04", "ToSuRaKg01", "ToSuRaKg02", "ToSuRaKg03", "ToSuRaKg04", "ToSuRaKz01", "ToSuRaKz02", "ToSuRaKz03", "ToSuRaKz04", "ToSuRaQq01", "ToSuRaQq02", "ToSuRaQq03", "ToSuRaQq04", "ToSuRaTj01", "ToSuRaTj02", "ToSuRaTj03", "ToSuRaTj04", "ToSuRaTm01", "ToSuRaTm02", "ToSuRaTm03", "ToSuRaTm04"};

        public List<string> ExistingBooks
        {
            get { return _ExistingBooks; }
            set { this.RaiseAndSetIfChanged(ref _ExistingBooks, value); }
        }

        private bool _buttonControl = true;

        public bool ButtonControl
        {
            get { return _buttonControl; }
            set { this.RaiseAndSetIfChanged(ref _buttonControl, value); }
        }


        #endregion

        #region Commands
        public ReactiveCommand<Unit, Task> Next { get; }
        #endregion

        #region Methods
        async Task OpenPlayerAsync()
        {
            var mp = new MusicPlayer();
            Console.WriteLine($"Opening MP with FileName = {MyFileName}");
            mp.BookNumber = GetLastChar(MyFileName);
            mp.musicPlayer = new MusicPlayerViewModel(MyFileName.ToLower());
            mp.BindingContext = mp.musicPlayer;

            await Application.Current.MainPage.Navigation.PushAsync(mp);
        }

        string GetLastChar(string fn)
        {
            return fn.Substring(fn.Length - 1);
        }
        #endregion

        public MainPageViewModel()
        {
            Next = ReactiveCommand.Create(OpenPlayerAsync);
        }
        
        public void OnScanResult(Result result)
        {
            if (ExistingBooks.Contains(result.Text))
            {
                MyFileName = result.Text;
                CanGoToMP = true;
            }

        }
    }
}
