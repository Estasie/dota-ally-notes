using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.Win32;

namespace DotaNotes.Core.Common
{
    public class GSIConfigService : IGSIConfigService
    {
        public bool Create()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(ConfigurationManager.AppSettings["RegSteamFolder"]);

            if (regKey != null)
            {
                string gsiFolder = regKey.GetValue("SteamPath") + ConfigurationManager.AppSettings["SteamConfigFolder"];

                Directory.CreateDirectory(gsiFolder);

                string gsiFile = gsiFolder + ConfigurationManager.AppSettings["FileName"];

                if (File.Exists(gsiFile)) return true;

                string[] contentofgsifile =
                {
                    "\"Dota 2 Integration Configuration\"",
                    "{",
                    "    \"uri\"           \"http://localhost:4000\"",
                    "    \"timeout\"       \"5.0\"",
                    "    \"buffer\"        \"0.1\"",
                    "    \"throttle\"      \"0.1\"",
                    "    \"heartbeat\"     \"30.0\"",
                    "    \"data\"",
                    "    {",
                    "        \"provider\"      \"1\"",
                    "        \"map\"           \"1\"",
                    "        \"player\"        \"1\"",
                    "        \"hero\"          \"1\"",
                    "        \"abilities\"     \"1\"",
                    "        \"items\"         \"1\"",
                    "    }",
                    "}",
                };

                File.WriteAllLines(gsiFile, contentofgsifile);
            }
            
            return false;
        }
    }
}