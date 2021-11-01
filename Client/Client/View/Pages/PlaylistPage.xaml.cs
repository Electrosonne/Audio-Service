using Client.Logic;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для PlaylistPage.xaml
    /// </summary>
    public partial class PlaylistPage : UserControl
    {
        public const int DecodePixelHeight = 210;
        public MusicManager m_MusicManager;
        
        public PlaylistPage(MusicManager MusicManager) : base()
        {
            InitializeComponent();
            m_MusicManager = MusicManager;

            Binding binding = new Binding();
            binding.Source = m_MusicManager.Playlist;
            binding.Path = new PropertyPath("Data");
            PlaylistsList.SetBinding(ListBox.ItemsSourceProperty, binding);
        }

        private async void PlaylistDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var playlist = ((Client.Logic.Playlist)((ListBox)sender).SelectedItem);

                if(playlist.Id.HasValue)
                {
                    await Task.Run(() =>
                    {
                        m_MusicManager.Music.LoadPlaylist(playlist.Id.Value);
                    });

                    m_MusicManager.Search = playlist.Name;

                    if (m_MusicManager.Music.Data.Count > 0)
                    {
                        m_MusicManager.NewCompositionSelected(m_MusicManager.Music.Data[0]);
                    }
                }
                else
                {
                    m_MusicManager.Music.OfflinePlaylist(playlist);

                    m_MusicManager.Search = playlist.Name;

                    if (m_MusicManager.Music.Data.Count > 0)
                    {
                        m_MusicManager.NewCompositionSelected(m_MusicManager.Music.Data[0]);
                    }
                }               
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка при запуске");
            }
        }
    }

    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string path = value.ToString();

            if (!File.Exists(path))
            {
                return new BitmapImage();
            }

            var result = new BitmapImage(new Uri(path))
            {
                CacheOption = BitmapCacheOption.OnLoad,
                DecodePixelHeight = PlaylistPage.DecodePixelHeight,
            };
            
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
