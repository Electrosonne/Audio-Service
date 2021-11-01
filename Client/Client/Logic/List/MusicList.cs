using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System;

namespace Client.Logic
{
    public class MusicList
    {
        private int OfflineCount { get; set; }
        private bool NowPlaylist { get; set; }

        private Dictionary<string, Music> m_AllMusicComposition;

        public ObservableCollection<Music> Data { get; set; }

        public string Search { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public MusicList()
        {
            NowPlaylist = false;

            Data = new ObservableCollection<Music>();
            m_AllMusicComposition = new Dictionary<string, Music>();

            Search = "";

            AsyncInitialize();
        }

        private async void AsyncInitialize()
        {
            await Task.Run(() =>
            {
                foreach (var dir in HistoryMusicSearch.History)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        AddByDir(dir);
                    });       
                }

                OfflineCount = Data.Count;
            });
        }

        public async void LoadMore()
        {
            await Task.Run(() =>
            {
                Update(Data.Count - OfflineCount, Data.Count - OfflineCount + 10);
            });
        }

        private void Update(int start, int end)
        {
            if (!NowPlaylist && QueryMaster.Connection)
            {
                try
                {
                    var data = QueryMaster.FindMusic(Search, start, end);

                    SetData(data);
                }
                catch (Exception) { }      
            }          
        }

        private void SetData(List<Music> data)
        {
            if (data != null)
            {
                foreach (var elem in data)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Data.Add(new OnlineMusic(elem.Id, elem.Name, elem.Author, elem.Description,
                            elem.Text, elem.Duration, elem.FrameRate, elem.SampleRate, elem.TagSize, elem.MaximumTicks));
                    });
                }
            }
        }

        public void RemoveFilter()
        {
            if (!Search.Equals("") || NowPlaylist)
            {
                OfflineCount = 0;
                NowPlaylist = false;
                LoadMusic("");
            }             
        }

        public void OfflinePlaylist(Playlist playlist)
        {
            NowPlaylist = true;
            Data.Clear();

            foreach (var song in playlist.Songs)
            {
                Data.Add(new OfflineMusic(song));
            }
        }

        public async void LoadPlaylist(int id)
        {
            NowPlaylist = true;

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                Data.Clear();
            });

            List<Music> data = null;

            data = QueryMaster.GetMusicFromPlaylist(id);

            SetData(data);
        }

        private void Add(string path, Music comp = null)
        {
            if(!m_AllMusicComposition.ContainsKey(path))
            {
                if (comp == null)
                {
                    comp = new OfflineMusic(path);
                }

                Data.Add(comp);
                m_AllMusicComposition.Add(path, comp);
            }  
        }

        public Music AddNewComposition(string path)
        {
            var comp = new OfflineMusic(path);
            Add(path, comp);

            var dir = Path.GetDirectoryName(path);

            AddByDir(dir);

            HistoryMusicSearch.AddAudioDir(dir);

            return comp;
        }

        private void AddByDir(string dir)
        {
            foreach (var file in Directory.GetFiles(dir))
            {
                if (Path.GetExtension(file) == ".mp3")
                {
                    Add(file);
                }
            }
        }

        public void FindMusic(string search)
        {           
            if(!search.Equals(Search))
            {
                LoadMusic(search);
            }
        }

        private void LoadMusic(string search)
        {
            Data.Clear();
            Search = search;

            bool empty = Search == "";

            foreach (var elem in m_AllMusicComposition)
            {
                if (empty || MatchMusic(elem.Value, Search))
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Data.Add(elem.Value);
                    });
                }
            }

            OfflineCount = Data.Count;

            if (OfflineCount < 10)
            {
                Update(0, 10 - OfflineCount);
            }
        }

        public bool MatchMusic(Music music, string search)
        {
            var comparison = StringComparison.CurrentCultureIgnoreCase;

            return music.Name.Contains(search, comparison) || music.Description.Contains(search, comparison) || music.Author.Contains(search, comparison);
        }
    }
}
