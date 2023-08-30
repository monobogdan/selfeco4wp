using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace VK4WP.Data
{
    public sealed class Preferences
    {
        public bool LoadAvatars;
    }

    public class Config
    {
        public static Config Instance;

        public Preferences Preferences;
        private ApplicationDataContainer container;

        public static void Initialize()
        {
            Instance = new Config();
        }

        private Config()
        {
            container = ApplicationData.Current.LocalSettings;
            
        }

        public void SetParameter<T>(string key, T value)
        {
            container.Values[key] = value;
        }

        public T GetParameter<T>(string key)
        {
            return (T)container.Values[key];
        }
        
        public bool IsAuthorized()
        {
            return container.Values.ContainsKey("usertoken");
        }
    }
}
