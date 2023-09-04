using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.IsolatedStorage;

using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Media.PhoneExtensions;

namespace WinPhoneYT
{
    public static class MediaUtils
    {
        
        public static void DownloadFileNonBuffered(string url, string fileName, Action<float> progress, Action<string> onReady, Action<string> onError)
        {
            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.Method = "GET";

            req.BeginGetResponse((IAsyncResult res) =>
            {
                try
                {
                    HttpWebResponse response = (HttpWebResponse)req.EndGetResponse(res);
                    Stream reader = response.GetResponseStream();
                    int len = (int)response.ContentLength;
                    const int BlockSize = 16384;
                    byte[] data = new byte[BlockSize];
                    int ptr = 0;
                    int progressThreshold = 0;

                    IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
                    IsolatedStorageFileStream fileStream = file.CreateFile(fileName);
                    
                    
                    while(ptr < len)
                    {
                        int amt = reader.Read(data, 0, ptr + BlockSize < len ? BlockSize : ptr + BlockSize - len);
                        ptr += amt;
                        fileStream.Write(data, 0, amt);

                        progressThreshold--;

                        if (progressThreshold < 0)
                        {
                            progressThreshold = 5;
                            progress((float)ptr / len); // To prevent dispatcher from flooding UI update
                        }
                    }

                    fileStream.Close();
                    reader.Close();

                    onReady(fileName);
                }
                catch (WebException e)
                {
                    onError(e.Message);
                }
            }, null);
        }

        public static void SaveLocalFileToMusic(string fileName, string artist, string title)
        {
            MediaLibrary library = new MediaLibrary();

            SongMetadata metadata = new SongMetadata();
            metadata.ArtistName = artist;
            metadata.Name = title;
            library.SaveSong(new Uri(fileName, UriKind.RelativeOrAbsolute), metadata, SaveSongOperation.CopyToLibrary);
        }

        public static void PlayVideo(VideoDescriptor desc)
        {
            int[] resTable = { 240, 360, 480, 720, 1080 };
            int desiredResolution = AppSettings.Instance.DesiredQuality == 0 ? (int)App.Current.Host.Content.ActualHeight : resTable[AppSettings.Instance.DesiredQuality - 1];
            FormatStream strm = YTAPI.Instance.PickBestFormat(desc, desiredResolution);

            Microsoft.Phone.Tasks.MediaPlayerLauncher mpLauncher = new Microsoft.Phone.Tasks.MediaPlayerLauncher();
            mpLauncher.Controls = Microsoft.Phone.Tasks.MediaPlaybackControls.All;
            mpLauncher.Media = new Uri(strm.url);
            mpLauncher.Show();
        }
    }
}
