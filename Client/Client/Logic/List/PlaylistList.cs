using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic
{
    public class PlaylistList
    {
        public ObservableCollection<Playlist> Data { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public PlaylistList()
        {
            Data = new ObservableCollection<Playlist>();
            
            foreach(var pl in OfflinePlaylists.Playlists)
            {
                Data.Add(pl);
            }
        }

        public async void Update()
        {
            await Task.Run(() =>
            {
                var playlists = QueryMaster.GetAllPlaylist();

                if (playlists != null)
                {
                    foreach (var pl in playlists)
                    {
                        var imagePath = ConfigurationMaster.DownloadImagePlaylist(pl.Id.Value);

                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            Data.Add(new Playlist(pl.Id, pl.Name, imagePath, pl.Author));
                        });           
                    }
                }
            });          
        }

        public void Add(Playlist playlist)
        {
            Data.Add(playlist);
            OfflinePlaylists.AddOfflinePlaylist(playlist);
        }
    }
}
