using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaAPI.Model
{
    public class Relation
    {
        [JsonProperty("blocking")]
        public bool Blocking { get; set; }
        [JsonProperty("followed_by")]
        public bool Followed_By { get; set; }
        [JsonProperty("following")]
        public bool Following { get; set; }
        [JsonProperty("incoming_request")]
        public bool Incoming_Request { get; set; }
        [JsonProperty("is_bestie")]
        public bool Is_Bestie { get; set; }
        [JsonProperty("is_blocking_reel")]
        public bool Is_Blocking_Reel { get; set; }
        [JsonProperty("is_muting_reel")]
        public bool Is_Muting_Reel { get; set; }
        [JsonProperty("is_private")]
        public bool Is_Private { get; set; }
        [JsonProperty("is_restricted")]
        public bool Is_Restricted { get; set; }
        [JsonProperty("muting")]
        public bool Muting { get; set; }
        [JsonProperty("outgoing_request")]
        public bool Outgoing_Request { get; set; }
        [JsonProperty("is_feed_favorite")]
        public bool Is_Feed_Favorite { get; set; }
        [JsonProperty("subscribed")]
        public bool Subscribed { get; set; }
        [JsonProperty("is_eligible_to_subscribe")]
        public bool Is_Eligible_To_Subscribe { get; set; }
        [JsonProperty("is_supervised_by_viewer")]
        public bool Is_Supervised_By_Viewer { get; set; }
        [JsonProperty("is_guardian_of_viewer")]
        public bool Is_Guardian_Of_Viewer { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }

    }
}