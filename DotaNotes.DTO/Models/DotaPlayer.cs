using DotaNotes.DTO.Models.Base;

namespace DotaNotes.DTO.Models
{
    public class DotaPlayer : BaseEntity
    {
        public string SteamId64 { get; set; }
        
        public string Name { get; set; }
       
        public string ProfileUrl { get; set; }
    
        public string AvatarFullURL { get; set; }

        public string Note { get; set; }
    }
}