using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Net;

namespace VK4WP.Data
{
    public enum MusicDownloadState
    {
        NotDownloaded,
        NoAccess,
        Downloaded
    }

    public sealed class LocalMusicManager
    {
        public static LocalMusicManager Instance;
        private StorageFolder musicDir;

        public static void Initialize()
        {
            Instance = new LocalMusicManager();
        }

        private LocalMusicManager()
        {
            musicDir = KnownFolders.MusicLibrary;
            Log.WriteLine("Music library path: {0}", musicDir.Path);
        }

        /*public async Windows.Foundation.IAsyncOperation<StorageFile> GetCachedAudio(string fileName)
        {
            StorageFolder subFolder = await musicDir.CreateFolderAsync("MiniVK", CreationCollisionOption.OpenIfExists);

            try
            {
                return subFolder.GetFileAsync(fileName);
            }
            catch (System.IO.FileNotFoundException e)
            {
                return null;
            }
        }*/
        
        private T WaitAsync<T>(Task<T> t) where T : class
        {
            t.Wait();

            return t.Result;
        }

        public void IsDownloaded(string fileName, Action<MusicDownloadState, StorageFile> onReady)
        {
            try
            {
                StorageFile file = WaitAsync<StorageFile>(musicDir.GetFileAsync(fileName).AsTask());

                onReady(MusicDownloadState.Downloaded, file);
            }
            catch (AggregateException e)
            {
                onReady(e.InnerException is System.IO.FileNotFoundException ? MusicDownloadState.NotDownloaded : MusicDownloadState.NoAccess, null);
            }
        }

        public void StartDownload(string fileName, string url, Data.Audio.Item desc, Action<float> onProgress, Action<bool, string> onComplete)
        {
            Log.WriteLine("Scheduled download of {0}", fileName);

            HttpWebRequest webReq = WebRequest.CreateHttp(url);
            webReq.Method = "GET";

            webReq.BeginGetResponse((IAsyncResult res) =>
            {
                try
                {
                    HttpWebResponse response = (HttpWebResponse)webReq.EndGetResponse(res);
                    System.IO.Stream stream = response.GetResponseStream();
                    byte[] buffer = new byte[response.ContentLength + 128]; // 128 - ID3v1 length
                    int ptr = 0;
                    
                    while(ptr < response.ContentLength)
                    {
                        int len = 4096;

                        if (response.ContentLength - ptr < 4096)
                            len = (int)response.ContentLength - ptr;

                        ptr += stream.Read(buffer, ptr, len);
                        onProgress(ptr / response.ContentLength);
                    }

                    Log.WriteLine("File downloaded, prepairing to flush data in {0}", fileName);
                    Utils.ID3Tag.Fill(desc.artist, desc.title, "Неизвестен").CopyTo(buffer, (int)response.ContentLength);

                    // Personally, i don't like full synchronous file API removal, since downloading are implemented in separate worker
                    // Tbh, there is no meaning to spawn 100500 workers for really enough quick operations like OpenFile :)
                    StorageFile f = WaitAsync<StorageFile>(musicDir.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists).AsTask());
                    FileIO.WriteBytesAsync(f, buffer).AsTask().Wait();

                    onComplete(true, "OK");
                }
                catch(WebException e)
                {
                    onComplete(false, e.Message);
                }
                catch(AggregateException e)
                {
                    onComplete(false, "Не удалось получить доступ к файлу. Попробуйте закрыть плеер.");
                    Log.WriteLine("AggregateException: {0}", e.InnerException.ToString());
                }
            }, null);
        }
    }
}
