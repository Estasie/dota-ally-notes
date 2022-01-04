using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotaNotes.Core.Common.SteamIdConverter;
using Microsoft.Win32;

namespace DotaNotes.Core
{
    public class ServerLogParserService
    {
        private const string SteamIdRegex = @"\d:(\[U:\d:(\d+)])";
        private string _serverLogPath = string.Empty;

        public bool Initialize()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(ConfigurationManager.AppSettings["RegSteamFolder"]);

            if (regKey != null)
            {
                _serverLogPath = regKey.GetValue("SteamPath") + ConfigurationManager.AppSettings["ServerLogPath"];

                if (File.Exists(_serverLogPath)) return true;
            }

            return false;
        }

        public IEnumerable<ulong> GetPlayerSteamIds()
        {
            if (!string.IsNullOrEmpty(_serverLogPath))
            {
                try
                {
                    using (var reader = new StreamReader(_serverLogPath))
                    {
                        var text =  reader.ReadToEnd();
                        var startIndex = text.LastIndexOf("DOTA_GAMEMODE_ALL_DRAFT");
                        var lastIndex = text.IndexOf(")", startIndex);
                        var finishString = text.Substring(startIndex, lastIndex - startIndex);
                        var playerIds = new Regex(SteamIdRegex, RegexOptions.IgnoreCase)
                            .Matches(finishString)
                            .Select(u => SteamIdConverter.SteamId32To64(int.Parse(u.Groups[2].Value)));

                        return playerIds;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return null;
        }
    }
}