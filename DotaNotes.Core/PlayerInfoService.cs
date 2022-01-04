using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dota2GSI.Nodes;
using DotaNotes.DTO.Context;
using DotaNotes.DTO.Models;

namespace DotaNotes.Core
{
    public class PlayerInfoService
    {
        private const string steamPlayerInfoURL =
            "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}";

        private readonly string apiKey;
        private DatabaseContext _dc;

        public PlayerInfoService()
        {
            apiKey = ConfigurationManager.AppSettings["SteamAPIKey"];
        }

        public IEnumerable<DotaPlayer> GetInfo(IEnumerable<ulong> steamIds64)
        {
            var url = string.Format(steamPlayerInfoURL, apiKey, string.Join(",", steamIds64));
            return SendRequest(url);
        }

        public DotaPlayer GetInfo(ulong steamId64)
        {
            var url = string.Format(steamPlayerInfoURL, apiKey, steamId64);
            return SendRequest(url).ToList()[0];
        }

        private IEnumerable<DotaPlayer> SendRequest(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(url).Result;
                var result = response.Content.ReadAsStringAsync();
                var node = new Node(result.Result);
                var dotaPlayers = new List<DotaPlayer>(10);

                using (_dc = new DatabaseContext())
                {
                    foreach (var steamUser in node.GetSteamUsers())
                    {
                        var dotaPlayer = new DotaPlayer
                        {
                            SteamId64 = steamUser.SteamId64,
                            Name = steamUser.Name,
                            ProfileUrl = steamUser.ProfileUrl,
                            AvatarFullURL = steamUser.AvatarFullURL,
                        };
                        
                        var existsUser = _dc.DotaPlayers.FirstOrDefault(u => u.SteamId64 == steamUser.SteamId64);
                        if (existsUser != null)
                            dotaPlayer.Note = existsUser.Note;
                        
                        dotaPlayers.Add(dotaPlayer);
                    }
                }


                return dotaPlayers;
            }
        }
    }
}