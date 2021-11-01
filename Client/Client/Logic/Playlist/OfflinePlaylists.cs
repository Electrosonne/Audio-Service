using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace Client.Logic
{
    public static class OfflinePlaylists
    {
        public static List<Playlist> Playlists { get; }

        static OfflinePlaylists()
        {
            if (File.Exists(ConfigurationMaster.SavedPlaylists))
            {
                Playlists = JsonConvert.DeserializeObject<List<Playlist>>(File.ReadAllText(ConfigurationMaster.SavedPlaylists));
            }
            else
            {
                Playlists = new List<Playlist>();
            }
        }

        public static async Task AddOfflinePlaylist(Playlist playlist)
        {
            if (!Playlists.Contains(playlist))
            {
                Playlists.Add(playlist);
                Update();
            }
        }

        public static async Task Update()
        {
            using FileStream createStream = File.Create(ConfigurationMaster.SavedPlaylists);
            await System.Text.Json.JsonSerializer.SerializeAsync(createStream, Playlists);           
        }
    }
}
