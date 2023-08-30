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
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пользовательского элемента управления задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234236

namespace VK4WP.UI
{
    public sealed partial class MusicCardView : UserControl
    {
        private MusicPage parent;
        private Data.Audio.Item info;

        public MusicCardView(MusicPage parentPage)
        {
            this.InitializeComponent();

            parent = parentPage;

            downloadProgress.Visibility = Visibility.Collapsed;
            downloadButton.Visibility = Visibility.Collapsed;
        }

        private string GetFileName()
        {
            return string.Format("{0} - {1} (MiniVK - {2}).mp3", info.artist, info.title, info.id);
        }

        public void SetInfo(Data.Audio.Item info)
        {
            this.info = info;

            
            name.Text = info.title;
            artist.Text = info.artist;

            /*Data.LocalMusicManager.Instance.IsDownloaded(GetFileName(), (Data.MusicDownloadState state, Windows.Storage.StorageFile file) =>
            {
                if (state == Data.MusicDownloadState.NotDownloaded)
                    downloadButton.Visibility = Visibility.Visible;
            });*/
        }

        private void OnRequestCache(object sender, RoutedEventArgs e)
        {
            if (parent.IsDownloaderBusy)
                return;

            parent.IsDownloaderBusy = true;
            downloadButton.IsEnabled = false;

            Data.VKAPI.Instance.AudioRelayRequest<Data.Audio.DetailsRoot>("getDetails", (Data.Audio.DetailsRoot root) =>
            {
                Data.LocalMusicManager.Instance.StartDownload(GetFileName(), root.response[0].url, info, (float progress) =>
                {
                    Utils.SubmitOnUiThread(() =>
                    {
                        downloadProgress.Visibility = Visibility.Visible;
                        downloadProgress.Value = (int)(progress * 100);
                    });
                }, (bool succeded, string msg) =>
                {
                    parent.IsDownloaderBusy = false;

                    if (!succeded)
                        Utils.ShowError("Ошибка при кэшировании трека: " + msg);
                    else
                        Utils.SubmitOnUiThread(() => { downloadProgress.Visibility = Visibility.Collapsed; });
                });
            }, (string error) => Utils.ShowError(error), "&id={0}_{1}", info.owner_id, info.id);
        }

        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
