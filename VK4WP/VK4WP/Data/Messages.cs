using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK4WP.Data.Messages
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Item
    {
        public int date { get; set; }
        public int from_id { get; set; }
        public int id { get; set; }
        public int @out { get; set; }
        public List<object> attachments { get; set; }
        public int conversation_message_id { get; set; }
        public List<object> fwd_messages { get; set; }
        public bool important { get; set; }
        public bool is_hidden { get; set; }
        public int peer_id { get; set; }
        public int random_id { get; set; }
        public string text { get; set; }
        public int? update_time { get; set; }
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


}
