using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

namespace WinPhoneYT
{
    public sealed class HistoryManager
    {
        const string FileName = "history.json";

        public List<VideoInfo> CachedHistory;
        private bool readyState;

        public HistoryManager()
        {
            CachedHistory = new List<VideoInfo>();
        }

        public void Clear()
        {
            CachedHistory.Clear();
        }

        public void AddEntry(VideoInfo info)
        {
            if(info != null)
                CachedHistory.Add(info);
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
                    CachedHistory = JsonConvert.DeserializeObject<List<VideoInfo>>(str);

                    readyState = true;
                }
            });
        }

        public void Save()
        {
            var storage = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();

            int historyLimit = 30;
            List<VideoInfo> shrinkedList = new List<VideoInfo>(historyLimit);
            
            foreach(VideoInfo info in CachedHistory)
            {
                if (historyLimit == 0)
                    break;

                shrinkedList.Add(info);
                historyLimit--;
            }

            StreamWriter writer = new StreamWriter(storage.CreateFile(FileName));
            writer.Write(JsonConvert.SerializeObject(shrinkedList));
            writer.Close();
        }
    }
}

