namespace DotaNotes.Core.Common.SteamIdConverter
{
    public static class SteamIdConverter
    {
        public static ulong SteamId32To64(int steamId32) => (ulong)steamId32 + 76561197960265728;
        
        public static int SteamId64To32(ulong steamId64) => (int)(76561197960265728 - steamId64);
    }
}