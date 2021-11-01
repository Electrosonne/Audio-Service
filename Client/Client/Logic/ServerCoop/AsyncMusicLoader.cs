using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Logic
{
    static class AsyncMusicLoader
    {
        //Константа формата mp3
        const double Limpls = 1152;
        //Сколько секунд надо скачать, чтобы начать воспроизводить
        const double SecundsForStart = 5;

        public delegate void LoadProgress(string uri);
        public static event LoadProgress LoadIsFull;

        public static void Download(OnlineMusic music)
        {
            string path = ConfigurationMaster.m_Cache + music.Name + ".mp3";

            if (File.Exists(path))
            {
                return;
            }

            ConfigurationMaster.WriteFile(path, QueryMaster.GetMusicFromServer(music.Id));
        }
    }
}
