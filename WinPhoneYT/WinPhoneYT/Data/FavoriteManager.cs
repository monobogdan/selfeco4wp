using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

namespace WinPhoneYT
{
    public sealed class FavoriteManager
    {
        const string FileName = "favorites.json";
        
        public List<VideoInfo> WatchLaterList;
        private bool readyState;

        public FavoriteManager()
        {
            WatchLaterList = new List<VideoInfo>();
        }

        public void Clear()
        {
            WatchLaterList.Clear();
        }

        public void AddEntry(VideoInfo desc)
        {
            if(desc != null)
                WatchLaterList.Add(desc);
        }
    
        public void RemoveEntry(VideoInfo info)
        {
            if (info != null)
            {
                VideoInfo target = null;

                foreach(VideoInfo i in WatchLaterList)
                {
                    if (i.videoId == info.videoId)
                        target = i;
                }

                if (target != null)
                    WatchLaterList.Remove(target);
            }
        }

        public bool IsReady()
        {
            return readyState;
        }

        public Task Load()
        {
            readyState = false;

            return Task.Factory.StartNew(() =>
            {
                var storage = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();

                if (storage.FileExists(FileName))
                {
                    StreamReader reader = new StreamReader(storage.OpenFile(FileName, FileMode.Open));
                    string str = "";

                    while (!reader.EndOfStream)
                        str += reader.ReadLine();

                    reader.Close();
                    WatchLaterList = JsonConvert.DeserializeObject<List<VideoInfo>>(str);

                    readyState = true;
                }
            });
        }

        public void Save()
        {
            var storage = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
            
            StreamWriter writer = new StreamWriter(storage.CreateFile(FileName));
            writer.Write(JsonConvert.SerializeObject(WatchLaterList));
            writer.Close();
        }
    }
}

