using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VK4WP.Data
{
    public struct ErrorDescription
    {
        public int Code;
        public string Error;
    }

    public sealed class VKAPI
    {
        const string APIUrl = "https://api.vk.com";
        const string AudioRelayUrl = "http://90.156.209.92/audiorelay.php";
        const string APIVersion = "5.131"; 
        public static VKAPI Instance;

        const String AppID = "2274003";
        const String AppSecret = "hHbZxrka2uZ6jB1inYsH";

        public static void Initialize()
        {
            if (Instance == null)
                Instance = new VKAPI();
        }

        private VKAPI()
        {

        }

        public bool IsAuthorized()
        {
            return true;
        }

        public void IsTokenValid(Action<bool> response)
        {

        }

        public void DirectAuth(string username, string password, Action<bool, string, string> onReady)
        {
            string fmt = string.Format("https://oauth.vk.com/token?grant_type=password&validate_token=true&client_id={0}&client_secret={1}&username={2}&password={3}",
                AppID, AppSecret, Uri.EscapeUriString(username), Uri.EscapeUriString(password));

            Log.WriteLine(fmt);
            Log.WriteLine("Direct authorization flow");

            AuthRequest(new Uri(fmt), (string response) =>
            {
                JObject obj = JObject.Parse(response);

                if(obj["error"] != null)
                {
                    string errText = obj["error_description"].Value<string>();
                    onReady(false, errText, null);
                }
                else
                {
                    Log.WriteLine("Authorization succeded");
                    onReady(true, obj["access_token"].Value<string>(), obj["user_id"].Value<string>());
                }
            });
        }

        // Workaround for 401 response from oauth
        private void AuthRequest(Uri url, Action<string> onReady)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = "GET";

            webRequest.BeginGetResponse((IAsyncResult res) =>
            {
                try
                {
                    Log.WriteLine("Async response");
                    WebResponse response = webRequest.EndGetResponse(res);
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string data = reader.ReadToEnd();

                    onReady(data);
                }
                catch (WebException e)
                {
                    WebResponse response = e.Response;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string data = reader.ReadToEnd();

                    onReady(data);
                }
            }, null);
        }

        public void PureRequest(Uri url, Action<string> onReady, Action<string> onError)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = "GET";

            webRequest.BeginGetResponse((IAsyncResult res) =>
            {
                try
                {
                    Log.WriteLine("Async response");
                    WebResponse response = webRequest.EndGetResponse(res);
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string data = reader.ReadToEnd();

                    onReady(data);
                }
                catch (WebException e)
                {
                    Log.WriteLine("WebException error occured");
                    onError("WebException: " + e.Message);
                }
            }, null);
        }

        public void PureRequest(string method, Action<string> onReady, Action<string> onError, string argFormat, params object[] args)
        {
            string token = Data.Config.Instance.GetParameter<string>("usertoken");
            string fmt = string.Format("{0}/method/{1}?access_token={2}&v={3}{4}", APIUrl, method, token, APIVersion, string.Format(argFormat, args));

            PureRequest(new Uri(fmt), onReady, onError);
        }

        public void RequestMethod<T>(string method, Action<T> onReady, Action<string> onError, string argFormat, params object[] args)
        {
            string token = Data.Config.Instance.GetParameter<string>("usertoken");
            string url = string.Format("{0}/method/{1}?access_token={2}&v={3}{4}", APIUrl, method, token, APIVersion, string.Format(argFormat, args));

            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = "GET";
             
            Log.WriteLine("Requested method {0}", method);

            webRequest.BeginGetResponse((IAsyncResult res) =>
            {
                try
                {
                    WebResponse response = webRequest.EndGetResponse(res);
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string json = reader.ReadToEnd();

                    JObject obj = JObject.Parse(json);
                    if(obj.Property("error") != null)
                    {
                        Log.WriteLine("API error occured: {0} {1}", obj["error"]["error_code"], obj["error"]["error_msg"]);
                        onError(obj["error"]["error_msg"].ToString());
                    }
                    else 
                    {
                        T t = JsonConvert.DeserializeObject<T>(json);

                        if (t != null)
                            onReady(t); 
                        else
                            onError("Deserialization failed");
                    }
                }
                catch (WebException e)
                {
                    Log.WriteLine("WebException error occured");
                    onError("WebException: " + e.Message);
                }
            }, null);
        }

        public void AudioRelayRequest<T>(string action, Action<T> onReady, Action<string> onError, string argFormat, params object[] args)
        {
            string url = string.Format("{0}?act={1}{2}", AudioRelayUrl, action, string.Format(argFormat, args));
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = "GET";

            Log.WriteLine("AudioRequest: {0}", url);

            webRequest.BeginGetResponse((IAsyncResult res) =>
            {
                try
                {
                    WebResponse response = webRequest.EndGetResponse(res);
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string json = reader.ReadToEnd();

                    JObject obj = JObject.Parse(json);
                    if (obj.Property("error") != null)
                    {
                        Log.WriteLine("API error occured: {0} {1}", obj["error"]["error_code"], obj["error"]["error_msg"]);
                        onError(obj["error"]["error_msg"].ToString());
                    }
                    else
                    {
                        T t = JsonConvert.DeserializeObject<T>(json);

                        if (t != null)
                            onReady(t);
                        else
                            onError("Deserialization failed");
                    }
                }
                catch (WebException e)
                {
                    Log.WriteLine("WebException error occured");
                    onError("WebException: " + e.Message);
                }
                catch(JsonException e)
                {
                    onError("JsonExcepiton: " + e.Message);
                }
            }, null);
        }
    }
}
