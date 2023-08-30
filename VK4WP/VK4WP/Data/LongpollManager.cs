using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Windows.UI.Notifications;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VK4WP.Data
{
    namespace Longpoll
    {
        public class Response
        {
            public string server { get; set; }
            public string key { get; set; }
            public long ts { get; set; }
        }

        public class Root
        {
            public Response response { get; set; }
        }

    }

    public enum LongpollEvent
    {
        AddMessage = 4
    }

    public sealed class LongpollManager
    {
        const int LongPollInterval = 5; // 5 seconds - is longpoll reconnection interval

        private ToastNotifier notifier;
        private Longpoll.Response state;
        
        public LongpollManager()
        {
            state = null;

            notifier = ToastNotificationManager.CreateToastNotifier();
        }

        private void Subscribe()
        {
            Log.WriteLine("Attempting to subscribe for longpoll...");

            VKAPI.Instance.RequestMethod<Longpoll.Root>("messages.getLongPollServer", (Longpoll.Root root) => {
                state = root.response;
            }, (string err) => Log.WriteLine(err), "");
        }

        private void CreateMessageNotify(string msg)
        {
            Notifications.NotificationUtils.ShowMessageNotify("", msg);
        }

        private void ProcessUpdate(JArray root)
        {
            foreach(JArray update in root)
            {
                LongpollEvent ev = (LongpollEvent)update[0].Value<int>();

                switch(ev)
                {
                    case LongpollEvent.AddMessage:
                        {
                            CreateMessageNotify(update[5].Value<string>());

                            break;
                        }
                }   
            }
        }

        private async void LongpollThread()
        {
            while(true) // We assume that longpolling will be available at whole app lifetime
            {
                if (state == null)
                    Subscribe();
                else
                {
                    string fmt = string.Format("https://{0}?act=a_check&key={1}&wait=25&ts={2}&mode=2&version=2", state.server, state.key, state.ts);

                    Log.WriteLine("Assert {0}", fmt);

                    HttpWebRequest req = HttpWebRequest.CreateHttp(fmt);
                    req.Method = "GET";
                    WebResponse response = await req.GetResponseAsync();

                    System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
                    while (!reader.EndOfStream)
                    {
                        string str = reader.ReadToEnd();

                        Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(str);
                        state.ts = (int)obj["ts"];
                        ProcessUpdate((JArray)obj["updates"]);
                    }
                }

                await Task.Delay(LongPollInterval * 1000);
            }
        }

        public void Run()
        {
            Task.Factory.StartNew(LongpollThread);
        }
    }
}
