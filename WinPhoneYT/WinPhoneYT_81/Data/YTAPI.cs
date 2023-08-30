using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Windows.Foundation;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WinPhoneYT
{

    public sealed class YTAPI
    {
        public static string[] InvidousInstances = {
            "https://google.com/"
                                                 };

        string line = "";

        public bool HasPendingOperation;

        public YTAPI()
        {

        }

        private void RequestURL(string method, Action<string> onReady, string fmt, params object[] args)
        {
            string defaultArgs = string.Format("region=RU&hl={0}", "ru");
            HttpWebRequest req = WebRequest.CreateHttp("https://vid.priv.au/api/v1/" + method + "?" + defaultArgs + string.Format(fmt, args));
            req.Method = "GET";

            req.BeginGetResponse((IAsyncResult res) =>
            {
                HttpWebResponse response = (HttpWebResponse)req.EndGetResponse(res);
                StreamReader reader = new StreamReader(response.GetResponseStream());

                line = reader.ReadToEnd();;
                onReady(line);
            }, null);
        }

        private int ToResolution(string str)
        {
            return int.Parse(str.Substring(0, str.IndexOf("p")));
        }

        public FormatStream PickBestFormat(VideoDescriptor desc, int maxResolution)
        {
            FormatStream pick = null;

            foreach(FormatStream strm in desc.formatStreams)
            {
                if(strm.container.Contains("mp4"))
                {
                    int reso = ToResolution(strm.resolution);

                    if (reso <= maxResolution)
                        pick = strm;
                }
            }

            if (pick == null)
                pick = desc.formatStreams[0];

            return pick;
        }

        public void QueryTrendingList(Action<List<VideoInfo>> onReady)
        {
            HasPendingOperation = true;

            RequestURL("trending", (string json) =>
            {
                List<VideoInfo> ret = JsonConvert.DeserializeObject<List<VideoInfo>>(json);
                
                onReady(ret);
                HasPendingOperation = false;
            }, "");
        }

        public void QueryPopularList(Action<List<VideoInfo>> onReady)
        {
            HasPendingOperation = true;
            RequestURL("popular", (string json) =>
            {
                List<VideoInfo> ret = JsonConvert.DeserializeObject<List<VideoInfo>>(json);

                onReady(ret);
                HasPendingOperation = false;
            }, "");
        }

        public void QueryVideoDescription(VideoInfo vidInfo, Action<VideoDescriptor> onReady)
        {
            HasPendingOperation = true;
            RequestURL("videos/" + vidInfo.videoId, (string json) =>
            {
                VideoDescriptor ret = JsonConvert.DeserializeObject<VideoDescriptor>(json);

                onReady(ret);
                HasPendingOperation = false;
            }, "");
        }

        public void QuerySearch(string query, Action<List<VideoInfo>> onReady)
        {
            HasPendingOperation = true;
            RequestURL("search", (string json) =>
            {
                List<VideoInfo> ret = JsonConvert.DeserializeObject<List<VideoInfo>>(json);

                onReady(ret);
                HasPendingOperation = false;
            }, "&q={0}&type=video", query);
        }
    }
}
