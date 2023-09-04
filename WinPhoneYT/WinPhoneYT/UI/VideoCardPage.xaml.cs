using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Microsoft.Xna.Framework.Media;

namespace WinPhoneYT.UI
{
    public partial class VideoCardPage : PhoneApplicationPage
    {
        public static VideoDescriptor Descriptior;
        public static VideoInfo Info;

        private MediaLibrary mediaLibrary;

        public VideoCardPage()
        {
            InitializeComponent();

            mediaLibrary = new MediaLibrary();
        }

        private string VisualizeCount(int count)
        {
            if (count < 1000)
                return count.ToString();

            if (count > 1000000)
                return (count / 1000000) + "KK";

            return (count / 1000) + "K";
        }

        private bool IsInFavorites()
        {
            foreach(VideoInfo fav in App.Current.Favorites.WatchLaterList)
            {
                if (fav.videoId == Info.videoId)
                    return true;
            }

            return false;
        }

        private void UpdateWatchLaterButton()
        {
            watchLater.Content = IsInFavorites() ? "Remove" : "Watch later";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            

            date.Text = Descriptior.publishedText;
            title.Text = Descriptior.title;
            description.Text = Descriptior.description;

            viewCount.Text = VisualizeCount(Descriptior.viewCount);
            likeCount.Text = VisualizeCount(Descriptior.likeCount);

            author.Text = Descriptior.author;
            preview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(Descriptior.videoThumbnails[4].url));

            UpdateWatchLaterButton();
        }

        private void OnDownloadAsAudio(object sender, RoutedEventArgs e)
        {
            AdaptiveFormat fmt = YTAPI.Instance.PickBestAudio(Descriptior);

            if (fmt != null)
            {
                progressPanel.Visibility = Visibility.Visible;

                string fileName = Descriptior.title + ".mp3";

                MediaUtils.DownloadFileNonBuffered(fmt.url, fileName, (float value) =>
                {
                    Dispatcher.BeginInvoke(() => { downloadProgress.Value = (int)(value * 100); });
                }, (string path) =>
                {
                    Dispatcher.BeginInvoke(() => { downloadProgress.Value = 100; });

                    MediaUtils.SaveLocalFileToMusic(fileName, Descriptior.author, Descriptior.title);
                }, (string err) =>
                {
                    MessageBox.Show(err);
                });

            }
       }

        private void OnRequestPlay(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MediaUtils.PlayVideo(Descriptior);
        }

        private void OnAddFavorites(object sender, RoutedEventArgs e)
        {
            if (!IsInFavorites())
            {
                App.Current.Favorites.AddEntry(Info);
            }
            else
            {
                App.Current.Favorites.RemoveEntry(Info);
            }

            UpdateWatchLaterButton();
            App.Current.Favorites.Save();
        }
    }
}