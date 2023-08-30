using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Media.Playback;

namespace VKAudioManager
{
    public sealed class BackgroundMusicTask : Windows.ApplicationModel.Background.IBackgroundTask
    {
        private BackgroundTaskDeferral deferral;
        private Windows.Media.SystemMediaTransportControls smtc;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += OnTaskCancel;
            taskInstance.Task.Completed += OnTaskComplete;

            smtc = Windows.Media.SystemMediaTransportControls.GetForCurrentView();
            smtc.IsEnabled = true;
            smtc.ButtonPressed += SMTCButton;
            smtc.PropertyChanged += SMTCProperty;
            smtc.DisplayUpdater.Type = Windows.Media.MediaPlaybackType.Music;
            smtc.IsPlayEnabled = true;
            smtc.IsPauseEnabled = true;

            BackgroundMediaPlayer.MessageReceivedFromForeground += OnReceiveMessage;
            BackgroundMediaPlayer.Current.CurrentStateChanged += OnStateChanged;

            deferral = taskInstance.GetDeferral();
        }

        private void SMTCProperty(Windows.Media.SystemMediaTransportControls sender, Windows.Media.SystemMediaTransportControlsPropertyChangedEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void SMTCButton(Windows.Media.SystemMediaTransportControls sender, Windows.Media.SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            if(args.Button == Windows.Media.SystemMediaTransportControlsButton.Play)
            {
                BackgroundMediaPlayer.Current.Play();
            }

            if (args.Button == Windows.Media.SystemMediaTransportControlsButton.Pause)
                BackgroundMediaPlayer.Current.Pause();
        }

        private void OnTaskComplete(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            deferral.Complete();
        }

        private void OnTaskCancel(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            deferral.Complete();
        }

        private void OnStateChanged(MediaPlayer sender, object args)
        {
            if (sender.CurrentState == MediaPlayerState.Playing)
                smtc.PlaybackStatus = Windows.Media.MediaPlaybackStatus.Playing;

            if (sender.CurrentState == MediaPlayerState.Stopped || sender.CurrentState == MediaPlayerState.Paused)
                smtc.PlaybackStatus = Windows.Media.MediaPlaybackStatus.Paused;
        }

        private void BeginPlay(string url)
        {
            BackgroundMediaPlayer.Current.SetUriSource(new Uri(url));
            BackgroundMediaPlayer.Current.AutoPlay = true;
        }

        private void UpdateSMTC(string track, string artist)
        {
            smtc.IsEnabled = true;
            smtc.IsPlayEnabled = true;
            smtc.PlaybackStatus = Windows.Media.MediaPlaybackStatus.Playing;

            smtc.DisplayUpdater.MusicProperties.Title = track;
            smtc.DisplayUpdater.MusicProperties.Artist = artist;
            smtc.DisplayUpdater.Update();
        }

        private void OnReceiveMessage(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            string method = (string)e.Data["method"];

            switch (method)
            {
                case "play":
                    string url = (string)e.Data["url"];
                    BeginPlay(url);

                    break;
                case "info":
                    string track = (string)e.Data["title"];
                    string artist = (string)e.Data["artist"];
                    UpdateSMTC(track, artist);

                    break;
                case "pause":
                    BackgroundMediaPlayer.Current.Pause();
                    break;
            }
        }
    }
}
