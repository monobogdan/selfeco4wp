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
        private HistoryManager History;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
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

        private void OpenVideo(VideoInfo info, VideoDescriptor desc)
        {
            UI.VideoCardPage.Descriptior = desc;
            UI.VideoCardPage.Info = info;
            App.RootFrame.Navigate(new Uri("/UI/VideoCardPage.xaml", UriKind.Relative));

            if (AppSettings.Instance.SaveHistory)
                History.AddHistoryEntry(info);
        }

        private void UpdateResults(List<VideoInfo> videos)
        {
            foreach (VideoInfo info in videos)
            {
                UI.VideoControl uiElement = new UI.VideoControl();

               // AnimationGenerator.SmoothSwipeX(uiElement, -800, 0);
                uiElement.SetInfo(info);
                uiElement.SetPreview(info.videoThumbnails[0].url);

                uiElement.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>((object sender, System.Windows.Input.GestureEventArgs gesture) =>
                {
                    progressFadeIn.Begin();

                    YTAPI.Instance.QueryVideoDescription(info, (VideoDescriptor desc) =>
                    {
                        Dispatcher.BeginInvoke((() =>
                        {
                            progressFadeOut.Begin();

                            OpenVideo(info, desc);
                        }));
                    });
                });

                contentMain.Children.Add(uiElement);
            }
        }

        private void RefreshTrending()
        {
            progressFadeIn.Begin();
            if (!YTAPI.Instance.HasPendingOperation)
            {
                YTAPI.Instance.QueryTrendingList((List<VideoInfo> videos) =>
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
            if (!YTAPI.Instance.HasPendingOperation)
            {
                YTAPI.Instance.QueryPopularList((List<VideoInfo> videos) =>
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
            if (!YTAPI.Instance.HasPendingOperation)
            {
                YTAPI.Instance.QuerySearch(searchText, (List<VideoInfo> videos) =>
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

        private void RefreshFavs()
        {
            contentMain.Children.Clear();
            UpdateResults(App.Current.Favorites.WatchLaterList);
        }

        private void OnReloadTrends(object sender, RoutedEventArgs e)
        {
            RefreshTrending();
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            App.RootFrame.Navigate(new Uri("/UI/SettingsPage.xaml", UriKind.Relative));
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

        private void OpenFavs(object sender, RoutedEventArgs e)
        {
            RefreshFavs();
        }
    }
}