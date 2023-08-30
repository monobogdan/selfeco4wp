using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WinPhoneYT.Resources;
using System.Diagnostics;

namespace WinPhoneYT
{
    public partial class MainPage : PhoneApplicationPage
    {
        private YTAPI YTInstance;
        private HistoryManager History;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            YTInstance = new YTAPI();
            History = new HistoryManager();
            History.Load();
            
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), "Исключение :c", MessageBoxButton.OK);
        }

        private void Log(string fmt, params object[] args)
        {
            Debug.WriteLine(fmt, args);
        }

        private void PlayVideo(VideoInfo info, VideoDescriptor desc)
        {
            int[] resTable = { 240, 360, 480, 720, 1080 };
            int desiredResolution = AppSettings.Instance.DesiredQuality == 0 ? (int)Application.Current.Host.Content.ActualHeight : resTable[AppSettings.Instance.DesiredQuality - 1];
            FormatStream strm = YTInstance.PickBestFormat(desc, desiredResolution);
            
            Log("Requested video {0}", desc.videoId);
            Log("Picked format {0}, while desired {1}", strm.resolution, AppSettings.Instance.DesiredQuality);

            Microsoft.Phone.Tasks.MediaPlayerLauncher mpLauncher = new Microsoft.Phone.Tasks.MediaPlayerLauncher();
            mpLauncher.Controls = Microsoft.Phone.Tasks.MediaPlaybackControls.All;
            mpLauncher.Media = new Uri(strm.url);
            mpLauncher.Show();

            if (AppSettings.Instance.SaveHistory)
                History.AddEntry(info);
        }

        private void UpdateResults(List<VideoInfo> videos)
        {
            foreach (VideoInfo info in videos)
            {
                UI.VideoControl uiElement = new UI.VideoControl();
                uiElement.SetInfo(info);
                uiElement.SetPreview(info.videoThumbnails[0].url);

                uiElement.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>((object sender, System.Windows.Input.GestureEventArgs gesture) =>
                {
                    YTInstance.QueryVideoDescription(info, (VideoDescriptor desc) =>
                    {
                        Dispatcher.BeginInvoke((() =>
                        {
                            PlayVideo(info, desc);
                        }));
                    });
                });

                contentMain.Children.Add(uiElement);
            }
        }

        private void RefreshTrending()
        {
            progressFadeIn.Begin();
            if (!YTInstance.HasPendingOperation)
            {
                YTInstance.QueryTrendingList((List<VideoInfo> videos) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        progressFadeOut.Begin();
                        contentMain.Children.Clear();
                        UpdateResults(videos);
                    });
                });
            }
        }

        private void RefreshPopular()
        {
            progressFadeIn.Begin();
            if (!YTInstance.HasPendingOperation)
            {
                YTInstance.QueryPopularList((List<VideoInfo> videos) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        progressFadeOut.Begin();
                        contentMain.Children.Clear();
                        UpdateResults(videos);
                    });
                });
            }
        }

        private void BeginSearch(string searchText)
        {
            progressFadeIn.Begin();
            if (!YTInstance.HasPendingOperation)
            {
                YTInstance.QuerySearch(searchText, (List<VideoInfo> videos) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        progressFadeOut.Begin();
                        contentMain.Children.Clear();
                        UpdateResults(videos);
                    });
                });
            }
        }

        private void RefreshHistory()
        {
            contentMain.Children.Clear();
            UpdateResults(History.CachedHistory);
        }

        private void OnReloadTrends(object sender, RoutedEventArgs e)
        {
            RefreshTrending();
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            App.RootFrame.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void OnReloadPopular(object sender, RoutedEventArgs e)
        {
            RefreshPopular();
        }

        private void OnReloadHistory(object sender, RoutedEventArgs e)
        {
            RefreshHistory();
        }

        private void OnRequestSearch(object sender, RoutedEventArgs e)
        {
            BeginSearch(searchTextBox.Text);
        }
    }
}