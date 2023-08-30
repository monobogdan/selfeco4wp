using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VK4WP.Data.Conversations
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Acl
    {
        public bool can_change_info { get; set; }
        public bool can_change_invite_link { get; set; }
        public bool can_change_pin { get; set; }
        public bool can_invite { get; set; }
        public bool can_promote_users { get; set; }
        public bool can_see_invite_link { get; set; }
        public bool can_moderate { get; set; }
        public bool can_copy_chat { get; set; }
        public bool can_call { get; set; }
        public bool can_use_mass_mentions { get; set; }
        public bool can_change_style { get; set; }
        public bool can_send_reactions { get; set; }
    }

    public class Ads
    {
        public string content_id { get; set; }
        public string duration { get; set; }
        public string account_age_type { get; set; }
        public string puid1 { get; set; }
        public string puid22 { get; set; }
    }

    public class Attachment
    {
        public string type { get; set; }
        public Link link { get; set; }
        public Photo photo { get; set; }
        public Video video { get; set; }
        public Audio audio { get; set; }
    }

    public class Audio
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
        public int genre_id { get; set; }
        public bool short_videos_allowed { get; set; }
        public bool stories_allowed { get; set; }
        public bool stories_cover_allowed { get; set; }
    }

    public class CanWrite
    {
        public bool allowed { get; set; }
    }

    public class ChatSettings
    {
        public string title { get; set; }
        public int members_count { get; set; }
        public int owner_id { get; set; }
        public PinnedMessage pinned_message { get; set; }
        public string state { get; set; }
        public Photo photo { get; set; }
        public List<int> admin_ids { get; set; }
        public List<int> active_ids { get; set; }
        public bool is_group_channel { get; set; }
        public Acl acl { get; set; }
        public bool is_disappearing { get; set; }
        public bool is_service { get; set; }
    }

    public class Conversation
    {
        public Peer peer { get; set; }
        public int last_message_id { get; set; }
        public int in_read { get; set; }
        public int out_read { get; set; }
        public SortId sort_id { get; set; }
        public int last_conversation_message_id { get; set; }
        public int in_read_cmid { get; set; }
        public int out_read_cmid { get; set; }
        public bool is_marked_unread { get; set; }
        public bool important { get; set; }
        public PushSettings push_settings { get; set; }
        public CanWrite can_write { get; set; }
        public ChatSettings chat_settings { get; set; }
        public string style { get; set; }
        public CurrentKeyboard current_keyboard { get; set; }
        public int? unread_count { get; set; }
    }

    public class CurrentKeyboard
    {
        public bool one_time { get; set; }
        //public List<List<>> buttons { get; set; }
        public int author_id { get; set; }
        public bool inline { get; set; }
    }

    public class FirstFrame
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Group
    {
        public int id { get; set; }
        public string name { get; set; }
        public string screen_name { get; set; }
        public int is_closed { get; set; }
        public string type { get; set; }
        public int is_admin { get; set; }
        public int is_member { get; set; }
        public int is_advertiser { get; set; }
        public string photo_50 { get; set; }
        public string photo_100 { get; set; }
        public string photo_200 { get; set; }
    }

    public class Image
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int with_padding { get; set; }
    }

    public class Item
    {
        public Conversation conversation { get; set; }
        public LastMessage last_message { get; set; }
    }

    public class LastMessage
    {
        public int date { get; set; }
        public int from_id { get; set; }
        public int id { get; set; }
        public int @out { get; set; }
        public List<Attachment> attachments { get; set; }
        public int conversation_message_id { get; set; }
        public List<object> fwd_messages { get; set; }
        public bool important { get; set; }
        public bool is_hidden { get; set; }
        public int peer_id { get; set; }
        public int random_id { get; set; }
        public string text { get; set; }
        public string @ref { get; set; }
        public string ref_source { get; set; }
        public ReplyMessage reply_message { get; set; }
        public string message_tag { get; set; }
    }

    public class Link
    {
        public string url { get; set; }
        public string title { get; set; }
        public string caption { get; set; }
        public string description { get; set; }
        public Photo photo { get; set; }
        public bool is_favorite { get; set; }
    }

    public class OnlineInfo
    {
        public bool visible { get; set; }
        public int last_seen { get; set; }
        public bool is_online { get; set; }
        public int app_id { get; set; }
        public bool is_mobile { get; set; }
        public string status { get; set; }
    }

    public class Peer
    {
        public int id { get; set; }
        public string type { get; set; }
        public int local_id { get; set; }
    }

    public class Photo
    {
        public int album_id { get; set; }
        public int date { get; set; }
        public int id { get; set; }
        public int owner_id { get; set; }
        public List<Size> sizes { get; set; }
        public string text { get; set; }
        public int user_id { get; set; }
        public bool has_tags { get; set; }
        public string photo_50 { get; set; }
        public string photo_100 { get; set; }
        public string photo_200 { get; set; }
        public bool is_default_photo { get; set; }
        public bool is_default_call_photo { get; set; }
        public string access_key { get; set; }
        public string web_view_token { get; set; }
    }

    public class PinnedMessage
    {
        public int id { get; set; }
        public int date { get; set; }
        public int from_id { get; set; }
        public List<Attachment> attachments { get; set; }
        public int conversation_message_id { get; set; }
        public List<object> fwd_messages { get; set; }
        public int peer_id { get; set; }
        public string text { get; set; }
    }

    public class Profile
    {
        public int id { get; set; }
        public int sex { get; set; }
        public string screen_name { get; set; }
        public string photo_50 { get; set; }
        public string photo_100 { get; set; }
        public OnlineInfo online_info { get; set; }
        public int online { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool can_access_closed { get; set; }
        public bool is_closed { get; set; }
        public int? online_mobile { get; set; }
        public int? online_app { get; set; }
    }

    public class PushSettings
    {
        public bool disabled_forever { get; set; }
        public bool no_sound { get; set; }
        public bool disabled_mentions { get; set; }
        public bool disabled_mass_mentions { get; set; }
    }

    public class ReplyMessage
    {
        public int date { get; set; }
        public int from_id { get; set; }
        public string text { get; set; }
        public List<object> attachments { get; set; }
        public int conversation_message_id { get; set; }
        public int id { get; set; }
        public int peer_id { get; set; }
    }

    public class Response
    {
        public int count { get; set; }
        public int unread_count { get; set; }
        public List<Item> items { get; set; }
        public List<Profile> profiles { get; set; }
        public List<Group> groups { get; set; }
    }

    public class Root
    {
        public Response response { get; set; }
    }

    public class Size
    {
        public int height { get; set; }
        public string type { get; set; }
        public int width { get; set; }
        public string url { get; set; }
    }

    public class SortId
    {
        public int major_id { get; set; }
        public int minor_id { get; set; }
    }

    public class Video
    {
        public string response_type { get; set; }
        public string access_key { get; set; }
        public int can_add { get; set; }
        public int comments { get; set; }
        public int date { get; set; }
        public string description { get; set; }
        public int duration { get; set; }
        public List<Image> image { get; set; }
        public List<FirstFrame> first_frame { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int id { get; set; }
        public int owner_id { get; set; }
        public string title { get; set; }
        public bool is_favorite { get; set; }
        public string track_code { get; set; }
        public string type { get; set; }
        public int views { get; set; }
        public int local_views { get; set; }
        public int can_dislike { get; set; }
    }





}
