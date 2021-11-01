using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic
{
    public static class HistoryMusicSearch
    {
        public static List<string> History { get; }

        static HistoryMusicSearch()
        {
            if (File.Exists(ConfigurationMaster.HistoryConfig))
            {
                History = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(ConfigurationMaster.HistoryConfig));
            }
            else
            {
                History = new List<string>();
            }

            History.Add(ConfigurationMaster.m_Cache);
        }

        public static async Task AddAudioDir(string dir)
        {
            if (!History.Contains(dir))
            {
                History.Add(dir);
                using FileStream createStream = File.Create(ConfigurationMaster.HistoryConfig);
                await System.Text.Json.JsonSerializer.SerializeAsync(createStream, History);
            }    
        }
    }
}
