using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dota2GSI.Nodes
{
    public class Node
    {
        protected Newtonsoft.Json.Linq.JObject _ParsedData;

        public Node(string json_data)
        {
            if (json_data.Equals(""))
            {
                json_data = "{}";
            }
            _ParsedData = Newtonsoft.Json.Linq.JObject.Parse(json_data);
        }

        protected string GetString(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value))
                return value.ToString();
            else
                return "";
        }

        public IEnumerable<SteamUser> GetSteamUsers()
        {
            return _ParsedData.Value<JObject>("response").Value<JArray>("players").ToObject<IEnumerable<SteamUser>>();
        }

        protected int GetInt(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value))
                return Convert.ToInt32(value.ToString());
            else
                return -1;
        }

        protected long GetLong(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;

            if(_ParsedData.TryGetValue(Name, out value))
                return Convert.ToInt32(value.ToString());
            else
                return -1;
        }

        protected T GetEnum<T>(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value) && !String.IsNullOrWhiteSpace(value.ToString()))
                return (T)Enum.Parse(typeof(T), value.ToString(), true);
            else
                return (T)Enum.Parse(typeof(T), "Undefined", true);
        }

        protected bool GetBool(string Name)
        {
            Newtonsoft.Json.Linq.JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value) && value.ToObject<bool>())
                return value.ToObject<bool>();
            else
                return false;
        }
    }
}
