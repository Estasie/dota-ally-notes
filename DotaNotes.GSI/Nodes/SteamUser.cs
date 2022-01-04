using Newtonsoft.Json;

namespace Dota2GSI.Nodes
{
    public class SteamUser
    {
        [JsonProperty("steamid")]
        public string SteamId64 { get; set; }

        [JsonProperty("personaname")]
        public string Name { get; set; }
        
        [JsonProperty("profileurl")]
        public string ProfileUrl { get; set; }
        
        [JsonProperty("avatarfull")]
        public string AvatarFullURL { get; set; }
    }
}