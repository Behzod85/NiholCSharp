using Plugin.SimpleAudioPlayer;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nihol
{
    public class MusicPlayerViewModel: ReactiveObject
    {
        #region Properties
        private double _MyDuration;

        public double MyDuration
        {
            get { return _MyDuration; }
            set { this.RaiseAndSetIfChanged(ref _MyDuration, value); }
        }

        private bool _IsPlaying;

        public bool MusicIsPlaying
        {
            get { return _IsPlaying; }
            set { this.RaiseAndSetIfChanged(ref _IsPlaying, value); }
        }

        private double _MyCurrentDuration;

        public double MyCurrentDuration
        {
            get { return _MyCurrentDuration; }
            set { this.RaiseAndSetIfChanged(ref _MyCurrentDuration, value); }
        }






        #endregion

        #region Fields
        ISimpleAudioPlayer player;
        public bool Dragging;
        public string MusicName;
        #endregion

        #region Commands
        public ReactiveCommand<Unit, Task> Back { get; }
        public ReactiveCommand<Unit, Unit> Stop { get; }
        public ReactiveCommand<Unit, Unit> PlayPause { get; }
        #endregion

        #region Methods
        async Task GoBackAsync()
        {
            if (player.IsPlaying)
            {
                player.Pause();
            }
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("Nihol.lowmp3." + filename);

            return stream;
        }

        void StopPlayer()
        {
            player.Stop();
            MusicIsPlaying = false;
        }

        void PlayPausePlayer()
        {
            if(MusicIsPlaying)
            {
                player.Pause();
                MusicIsPlaying = false;
            } else
            {
                player.Play();
                MusicIsPlaying = true;
                Device.StartTimer(TimeSpan.FromSeconds(0.5), UpdatePosition);
            }
        }
        public void Seek(double d)
        {
            if (player.CanSeek)
            {
                player.Seek(d);
                Dragging = false;
            }
                
        }



        #endregion

        public MusicPlayerViewModel(string mn)
        {
            MusicName = mn;
            // init player
            var stream = GetStreamFromFile($"{mn}.mp3");
            player = CrossSimpleAudioPlayer.Current;
            player.Load(stream);
            MyDuration = player.Duration;
            player.Play();
            MusicIsPlaying = true;
            Device.StartTimer(TimeSpan.FromSeconds(0.5), UpdatePosition);
            player.PlaybackEnded += Player_PlaybackEnded;

            Stop = ReactiveCommand.Create(StopPlayer);
            PlayPause = ReactiveCommand.Create(PlayPausePlayer);
            Back = ReactiveCommand.Create(GoBackAsync);
        }

        private void Player_PlaybackEnded(object sender, EventArgs e)
        {
            MusicIsPlaying = false;
            player.Stop();
            MyCurrentDuration = 0;
        }

        private bool UpdatePosition()
        {
            if (Dragging != true)
            {
                MyCurrentDuration = player.CurrentPosition;
            }
            return player.IsPlaying;
        }
    }
}
