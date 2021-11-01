using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic
{
    public static class ConfigurationMaster
    {
        public static string m_Cache { get; }
        public static string m_ImageChache { get; }

        public static string AudioConfig { get; }
        public static string HistoryConfig { get; }
        public static string SavedPlaylists { get; }
        public static string StandartImage { get; }

        static ConfigurationMaster()
        {
            AudioConfig = Directory.GetCurrentDirectory() + "\\Audio Configuration.json";
            SavedPlaylists = Directory.GetCurrentDirectory() + "\\SavedPlaylists.json";
            HistoryConfig = Directory.GetCurrentDirectory() + "\\Audio History.json";
            m_Cache = Directory.GetCurrentDirectory() + "\\Cache\\";
            StandartImage = Directory.GetCurrentDirectory() + "\\Items\\nice.jpg";

            if (!Directory.Exists(m_Cache))
            {
                Directory.CreateDirectory(m_Cache);
            }

            m_ImageChache = m_Cache + "\\ImageCache\\";

            if (!Directory.Exists(m_ImageChache))
            {
                Directory.CreateDirectory(m_ImageChache);
            }
        }

        public static string CachedImage(string name, byte[] data)
        {
            var path = m_ImageChache + name + ".png";

            if (!File.Exists(path))
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Cache\"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Cache\");
                }

                WriteFile(path, data);
            }

            return path;
        }

        public static string LocalImage(string name, string filePath)
        {
            var path = m_ImageChache + name + ".png";

            if (!File.Exists(path))
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Cache\"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Cache\");
                }

                TagLib.File musicData = TagLib.File.Create(filePath);

                if(musicData.Tag.Pictures.Length > 0)
                {
                    WriteFile(path, musicData.Tag.Pictures[0].Data.Data);
                }
                else
                {
                    path = StandartImage;
                }
            }

            return path;
        }

        public static string GetServerImage(int id)
        {
            string path = m_ImageChache + id + ".png";

            if (File.Exists(path))
            {
                return path;
            }

            WriteFile(path, QueryMaster.GetImageFromServer(id));

            return path;
        }

        public static string DownloadImagePlaylist(int id)
        {
            string path = m_ImageChache + "Playlist " + id + ".png";

            if (File.Exists(path))
            {
                return path;
            }

            WriteFile(path, QueryMaster.GetPlaylistImageFromServer(id));

            return path;
        }

        private static void WriteFile(string path, byte[] data)
        {
            using (FileStream fs = File.Create(path))
            {
                fs.Write(data, 0, data.Length);
            }
        }

        public static void WriteFile(string path, Stream data)
        {
            byte[] buffer = new byte[32768];
            int read;

            using (FileStream fstream = System.IO.File.Open(path, FileMode.Create))
            {
                while ((read = data.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fstream.Write(buffer, 0, read);
                }
            }
        }

        public static async Task ClearCache()
        {
            File.Delete(AudioConfig);
            File.Delete(SavedPlaylists);
            File.Delete(HistoryConfig);
            File.Delete(AudioConfig);
            Directory.Delete(m_Cache, true);
            Directory.CreateDirectory(m_Cache);
            Directory.CreateDirectory(m_ImageChache);
        }
    }
}
