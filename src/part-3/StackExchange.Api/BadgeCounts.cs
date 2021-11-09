using Newtonsoft.Json;

namespace StackExchange.Api
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BadgeCounts
    {
        [JsonProperty(PropertyName = "gold")]
        public int Gold { get; set; }

        [JsonProperty(PropertyName = "silver")]
        public int Silver { get; set; }

        [JsonProperty(PropertyName = "bronze")]
        public int Bronze { get; set; }
    }
}
