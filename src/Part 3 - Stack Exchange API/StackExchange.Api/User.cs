using System;
using Newtonsoft.Json;

namespace StackExchange.Api
{
    [WrapperObject("users")]
    [JsonObject(MemberSerialization.OptIn)]
    public class User
    {
        [JsonProperty(PropertyName = "user_id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        [JsonProperty(PropertyName = "reputation")]
        public int Reputation { get; set; }

        [JsonProperty(PropertyName = "last_access_date")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime LastAccessDate { get; set; }

        [JsonProperty(PropertyName = "website_url")]
        public string Website { get; set; }

        [JsonProperty(PropertyName = "badge_counts")]
        public BadgeCounts BadgeCounts { get; set; }

        [JsonProperty(PropertyName = "view_count")]
        public int Views { get; set; }
    }
}
