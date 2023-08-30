using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK4WP.Data.Audio
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Ads
    {
        public string content_id { get; set; }
        public string duration { get; set; }
        public string account_age_type { get; set; }
        public string puid22 { get; set; }
    }

    public class Album
    {
        public int id { get; set; }
        public string title { get; set; }
        public int owner_id { get; set; }
        public string access_key { get; set; }
        public Thumb thumb { get; set; }
    }

    public class Item
    {
        public string artist { get; set; }
        public int id { get; set; }
        public int owner_id { get; set; }
        public string title { get; set; }
        public int duration { get; set; }
        public string access_key { get; set; }
        public Ads ads { get; set; }
        public bool is_explicit { get; set; }
        public bool is_focus_track { get; set; }
        public bool is_licensed { get; set; }
        public string track_code { get; set; }
        public string url { get; set; }
        public int date { get; set; }
        public Album album { get; set; }
        public List<MainArtist> main_artists { get; set; }
        public bool short_videos_allowed { get; set; }
        public bool stories_allowed { get; set; }
        public bool stories_cover_allowed { get; set; }
        public int? lyrics_id { get; set; }
        public int? genre_id { get; set; }
        public int? no_search { get; set; }
    }

    public class MainArtist
    {
        public string name { get; set; }
        public string domain { get; set; }
        public string id { get; set; }
        public bool is_followed { get; set; }
        public bool can_follow { get; set; }
    }

    public class Response
    {
        public int count { get; set; }
        public List<Item> items { get; set; }
    }

    public class Root
    {
        public Response response { get; set; }
    }

    public class DetailsRoot
    {
        public List<Item> response { get; set; }
    }

    public class Thumb
    {
        public int width { get; set; }
        public int height { get; set; }
        public string photo_34 { get; set; }
        public string photo_68 { get; set; }
        public string photo_135 { get; set; }
        public string photo_270 { get; set; }
        public string photo_300 { get; set; }
        public string photo_600 { get; set; }
        public string photo_1200 { get; set; }
    }




}
