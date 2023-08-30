using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.Media.Playback;
using Windows.UI.Xaml.Navigation;


namespace VK4WP
{
    public sealed partial class MusicPage : Page
    {
        private MediaPlayer playerInstance;
        private DispatcherTimer updateTimer;
        private Windows.Media.SystemMediaTransportControls smtc;

        private Data.Audio.Root userAudio;

        public bool IsDownloaderBusy // Downloading multiple audios in same time causes SEHException (i.e SIGSEGV in external code).
        {
            get;
            set;
        }

        public bool IsMediaBusy
        {
            get;
            set;
        }

        private bool slideProgressFlag;

        public MusicPage()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            header.SetTitle("Музыка");
            header.AttachMenuToDrawer(drawer);

            updateTimer = new DispatcherTimer();
            updateTimer.Tick += OnUpdateSlider;
            updateTimer.Interval = TimeSpan.FromSeconds(1);

            playerInstance = BackgroundMediaPlayer.Current;
            playerInstance.CurrentStateChanged += OnMediaStateChanged;
            playerInstance.AutoPlay = true;
            BackgroundMediaPlayer.MessageReceivedFromBackground += OnReceiveAudioMessage;

            playerInstance.MediaFailed += OnFailed;

            /*smtc = Windows.Media.SystemMediaTransportControls.GetForCurrentView();
            smtc.IsEnabled = true;
            smtc.IsPlayEnabled = true;
            smtc.DisplayUpdater.Type = Windows.Media.MediaPlaybackType.Music;*/
        }

        private void OnReceiveAudioMessage(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            Utils.ShowError(args.Error.ToString());
        }

        private void OnUpdateSlider(object sender, object e)
        {
            if (playerInstance.CurrentState == MediaPlayerState.Playing)
            {
                int secs = (int)playerInstance.Position.TotalSeconds;
                int totalSecs = (int)playerInstance.NaturalDuration.TotalSeconds;

                if (secs < 1 || totalSecs < 1)
                    return;

                Log.WriteLine("{0} {1}", secs, totalSecs);

                slideProgressFlag = true;
                mediaSlider.Value = ((float)secs / totalSecs) * 100.0f;
                slideProgressFlag = false;
            }
        }

        private void UpdateUI(Data.Audio.Root root)
        {
            contentMain.Children.Clear();

            int num = 0;
            foreach (Data.Audio.Item item in root.response.items)
            {
                UI.MusicCardView uiElement = new UI.MusicCardView(this);
                uiElement.Tapped += (object sender, TappedRoutedEventArgs args) => {
                    BeginPlaying(item);
                };
                uiElement.SetInfo(item);

                contentMain.Children.Add(uiElement);
            }
        }

        public void RequestUpdate()
        {
            header.SetLoadingState(true);

            Data.VKAPI.Instance.AudioRelayRequest<Data.Audio.Root>("get", (Data.Audio.Root root) =>
            {
                Utils.SubmitOnUiThread(() => {
                    header.SetLoadingState(false);

                    userAudio = root;
                    UpdateUI(root);
                });
            }, (string msg) => Utils.ShowError(msg), "&uid={0}", 294692358);
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RequestUpdate();
        }

        private void OnStateChanged(object sender, RoutedEventArgs e)
        {

        }

        public void BeginPlaying(Data.Audio.Item item)
        {
            if (playerInstance.CurrentState == MediaPlayerState.Opening || playerInstance.CurrentState == MediaPlayerState.Buffering)
                return;

            Data.VKAPI.Instance.AudioRelayRequest<Data.Audio.DetailsRoot>("getDetails", (Data.Audio.DetailsRoot root) =>
            {
                Utils.SubmitOnUiThread(() =>
                {
                    /*playerInstance.SetUriSource(new Uri(root.response[0].url));
                    playerInstance.Play();*/

                    ValueSet cmd = new ValueSet();
                    cmd["method"] = "play";
                    cmd["url"] = root.response[0].url;
                    BackgroundMediaPlayer.SendMessageToBackground(cmd);

                    // Update media UI
                    mediaName.Text = string.Format("{0} - {1}", item.title, item.artist);

                    ValueSet cmd2 = new ValueSet();
                    cmd2["method"] = "info";
                    cmd2["artist"] = root.response[0].artist;
                    cmd2["title"] = root.response[0].title;
                    BackgroundMediaPlayer.SendMessageToBackground(cmd2);

                });
            }, (string error) => Utils.ShowError(error), "&id={0}_{1}", item.owner_id, item.id);
        }

        private void OnMediaStateChanged(MediaPlayer sender, object args)
        {
            if (playerInstance.CurrentState == MediaPlayerState.Playing)
            {
                Utils.SubmitOnUiThread(() =>
                {
                    mediaPlay.Content = "Pause";
                    updateTimer.Start();
                });
                
            }

            if (playerInstance.CurrentState == MediaPlayerState.Paused || playerInstance.CurrentState == MediaPlayerState.Stopped)
            {
                Utils.SubmitOnUiThread(() =>
                {
                    mediaPlay.Content = "Pause";
                    updateTimer.Stop();
                });
            }
        }

        private void OnPlayPause(object sender, RoutedEventArgs e)
        {
            if (playerInstance.CurrentState == MediaPlayerState.Paused || playerInstance.CurrentState == MediaPlayerState.Stopped)
                playerInstance.Play();

            if (playerInstance.CurrentState == MediaPlayerState.Playing)
                playerInstance.Pause();
        }

        private void OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (slideProgressFlag)
                return;

            if (mediaSlider != null)
            {
                float val = (float)mediaSlider.Value / 100.0f;
                playerInstance.Position = TimeSpan.FromSeconds(val * (int)playerInstance.NaturalDuration.TotalSeconds);
            }
        }

        private void OnSearch(object sender, RoutedEventArgs e)
        {
            if(query.Text.Length > 0)
            {
                header.SetLoadingState(true);

                Data.VKAPI.Instance.AudioRelayRequest<Data.Audio.Root>("search", (Data.Audio.Root root) =>
                {
                    Utils.SubmitOnUiThread(() => {
                        header.SetLoadingState(false);
                        
                        UpdateUI(root);
                    });
                }, (string msg) => Utils.ShowError(msg), "&query={0}", query.Text.Replace(' ', '_'));
            }
        }

        private void OnQueryChanged(object sender, TextChangedEventArgs e)
        {
            if (query.Text.Length == 0)
                UpdateUI(userAudio);
        }
    }
}
