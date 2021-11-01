using Client.Logic;
using Client.View;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MusicPage.xaml
    /// </summary>
    public partial class MusicPage : UserControl
    {
        public MusicManager m_MusicManager;

        public MusicPage(MusicManager MusicManager)
        {
            InitializeComponent();
            m_MusicManager = MusicManager;

            Binding binding = new Binding();
            binding.Source = m_MusicManager.Music;
            binding.Path = new PropertyPath("Data");
            MusicList.SetBinding(ListBox.ItemsSourceProperty, binding);

            binding = new Binding();
            binding.Source = m_MusicManager;
            binding.Path = new PropertyPath("Search");
            TextSearch.SetBinding(TextBox.TextProperty, binding);           
        }

        private void MusicList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MusicList.SelectedItem != null)
            {
                try
                {
                    m_MusicManager.NewCompositionSelected((Logic.Music)MusicList.SelectedItem);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ошибка при запуске");
                }
            }
        }

        public void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            m_MusicManager.Music.FindMusic(TextSearch.Text);
        }

        //Подгружает музыку в конце скролла
        private void MusicScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (MusicScroll.VerticalOffset == MusicScroll.ScrollableHeight && MusicScroll.ScrollableHeight != 0)
            {
                m_MusicManager.Music.LoadMore();
            }
        }

        private void ButtonRemoveFilter_Click(object sender, RoutedEventArgs e)
        {
            TextSearch.Text = "";

            m_MusicManager.Music.RemoveFilter();
        }

        private void AddMusicInPlaylist(object sender, RoutedEventArgs e)
        {
            var window = new OwnChange(m_MusicManager);
            window.Owner = App.Current.MainWindow;

            var music = (Music)((Button)sender).DataContext;

            List<int> oldPL = new List<int>();
            List<Playlist> offlineOld = new List<Playlist>();           //Старые плейлисты

            //Получение плейлистов, в которых состоит музыка
            //с сервера
            if (music is OnlineMusic)
            {
                oldPL = QueryMaster.GetMusicPlaylists(music.Id);

                foreach(var id in oldPL)
                {
                    window.PlaylistsList.SelectedItems.Add(window.PlaylistsList.Items[id]);
                }
            }
            //оффлайн
            foreach (var pl in OfflinePlaylists.Playlists)
            {
                foreach (var song in pl.Songs)
                {
                    if (song.Equals(music.Uri.OriginalString))
                    {
                        window.PlaylistsList.SelectedItems.Add(window.PlaylistsList.Items[window.PlaylistsList.Items.IndexOf(pl)]);
                        offlineOld.Add(pl);
                    }
                }
            }

            if (window.ShowDialog() == true)
            {
                //async!
                var selectedPL = window.PlaylistsList.SelectedItems;    //Выбранные плейлисты
                List<int> addedPL = new List<int>();                    //Плейлисты к добавлению
                List<int> deletedPL = new List<int>();                  //Плейлисты к удалению       

                foreach (Playlist playlist in selectedPL)
                {
                    if(music is OnlineMusic && playlist.Id.HasValue)
                    {
                        //Условие, что нужно добавить плейлист
                        if (!oldPL.Contains(playlist.Id.Value))
                        {
                            addedPL.Add(playlist.Id.Value);
                        }
                    }
                    else
                    {
                        //Музыка добавлена в оффлайновый новый плейлист
                        if (!playlist.Songs.Contains(music.Uri.OriginalString))
                        {
                            playlist.Songs.Add(music.Uri.OriginalString);                                                      
                        }
                        //Музыка осталась в старом оффлайновом плейлисте
                        else
                        {
                            offlineOld.Remove(playlist);
                        }
                    }
                }

                //удалить для плейлистов, которые больше не выделены
                foreach(var deletedOffline in offlineOld)
                {
                    deletedOffline.Songs.Remove(music.Uri.OriginalString);
                }

                OfflinePlaylists.Update();

                //Поиск для удаления
                foreach (var was in oldPL)
                {
                    foreach (Playlist became in selectedPL)
                    {
                        //если старого плейлиста нет среди новых, то он удаляется
                        if (was == became.Id)
                        {
                            goto Next;
                        }
                    }
                    deletedPL.Add(was);
                    Next: continue;
                }

                Task.Run(() =>
                {
                    if (addedPL.Count > 0)
                        QueryMaster.AddInPlaylist(addedPL, music.Id);
                    if (deletedPL.Count > 0)
                        QueryMaster.DeleteFromPlaylist(deletedPL, music.Id);
                });
            }
        }

        private void DownloadMusic(object sender, RoutedEventArgs e)
        {
            var music = (OnlineMusic)((Button)sender).DataContext;

            Task.Run(() =>
            {                
                AsyncMusicLoader.Download(music);
            });
        }
    }
}
