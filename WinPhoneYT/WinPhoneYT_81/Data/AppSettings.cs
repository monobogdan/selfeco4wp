using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;

using Newtonsoft.Json;

namespace WinPhoneYT
{
    public enum DesiredVideoQuality
    {
        Auto, // According to screen size
        Q240p,
        Q360p,
        Q480p,
        Q720p,
        Q1080p
    }

    public class AppSettings
    {
        const string ConfigName = "config.json";
        public static AppSettings Instance;

        public bool LoadPreviews;
        public bool SaveHistory;
        public int DesiredQuality;
        
        static AppSettings()
        {
            Instance = new AppSettings();
        }

        public static void Load()
        {
            var storage = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();

            if(storage.FileExists((ConfigName)))
            {
                StreamReader reader = new StreamReader(storage.OpenFile(ConfigName, FileMode.Open));
                string str = "";

                while (!reader.EndOfStream)
                    str += reader.ReadLine();

                reader.Close();
                Instance = JsonConvert.DeserializeObject<AppSettings>(str);
            }
            else
            {
                // Load default settings
                Instance.SaveHistory = true;
                Instance.LoadPreviews = true;
                Instance.DesiredQuality = 0;
            }
            
        }

        public static void Save()
        {
            var storage = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();

            StreamWriter writer = new StreamWriter(storage.CreateFile(ConfigName));
            writer.Write(JsonConvert.SerializeObject(Instance));
            writer.Close();
        }
    }
}
